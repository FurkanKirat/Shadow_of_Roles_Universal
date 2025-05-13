using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using game.Constants;
using Managers;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;

namespace Networking.Managers
{
    public class LobbyManager : MonoBehaviour
    {
        public Lobby Lobby { get; private set; }
        public string LobbyJoinCode { get; private set; }
        private string RelayJoinCode { get; set; }

        
        private void Awake()
        {
            NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
            }
        }

        private void OnTransportFailure()
        {
            Debug.LogError("Relay Transport failure occurred. Restart required.");
        }
        /// <summary>
        /// Host lobby + create Relay + start host
        /// </summary>
        public async Task CreateLobbyWithRelayAsync()
        {
            await OnlineServicesManager.InitializeAndSignInAsync();

            try
            {
                Debug.Log("[Relay] Relay allocation is being created...");
                var allocation = await RelayService.Instance.CreateAllocationAsync(GameConstants.MaxPlayerCount);
                RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                if (string.IsNullOrEmpty(RelayJoinCode))
                {
                    Debug.LogError("[Relay] Relay join code is null or empty. Aborting lobby creation.");
                    return;
                }

                Debug.Log($"[Relay] Relay join code created: {RelayJoinCode}");

                var player = new Player(
                    id: AuthenticationService.Instance.PlayerId,
                    data: GetPlayerData()
                );

                Debug.Log("[Lobby] Creating lobby...");
                string lobbyName = "shadow_of_roles" + UnityEngine.Random.Range(1000, 9999);
                Lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, GameConstants.MaxPlayerCount, new CreateLobbyOptions
                {
                    Player = player,
                    Data = new Dictionary<string, DataObject>
                    {
                        { "joinCode", new DataObject(DataObject.VisibilityOptions.Member, RelayJoinCode) }
                    }
                });

                if (Lobby == null || string.IsNullOrEmpty(Lobby.LobbyCode))
                {
                    Debug.LogError("[Lobby] Lobby creation failed or lobby code is empty.");
                    return;
                }

                LobbyJoinCode = Lobby.LobbyCode;
                Debug.Log($"[Lobby] Lobby created successfully with code: {LobbyJoinCode}");

                var serverData = new RelayServerData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.ConnectionData,
                    allocation.ConnectionData,
                    allocation.Key,
                    false // DTLS
                );

                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(serverData);

                Debug.Log("[Netcode] Starting host...");
                if (!NetworkManager.Singleton.IsListening)
                {
                    NetworkManager.Singleton.StartHost();
                }
                
            }
            catch (RelayServiceException e)
            {
                Debug.LogError($"[Relay ERROR] Failed to create allocation: {e.Message}\n{e}");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"[Lobby ERROR] Failed to create lobby: {e.Message}\n{e}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[GENERAL ERROR] Unexpected error during lobby creation: {e.Message}\n{e}");
            }
        }

        /// <summary>
        /// Join lobby by lobby code + get Relay join code from lobby data + start client
        /// </summary>
        public async Task JoinLobbyWithRelayAsync(string lobbyCode)
        {
            await OnlineServicesManager.InitializeAndSignInAsync();

            try
            {
                Debug.Log($"[JoinLobby] Attempting to join lobby with code: {lobbyCode}");
                LobbyJoinCode = lobbyCode;

                var joinOptions = new JoinLobbyByCodeOptions
                {
                    Player = new Player(
                        id: AuthenticationService.Instance.PlayerId,
                        data: GetPlayerData()
                    )
                };

                Lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinOptions);

                if (Lobby == null)
                {
                    Debug.LogError("[JoinLobby] Failed to join lobby. Lobby is null.");
                    return;
                }

                if (!Lobby.Data.ContainsKey("joinCode") || string.IsNullOrEmpty(Lobby.Data["joinCode"].Value))
                {
                    Debug.LogError("[JoinLobby] joinCode not found or empty in lobby data. Relay may not have been set.");
                    return;
                }

                RelayJoinCode = Lobby.Data["joinCode"].Value;
                Debug.Log($"[JoinLobby] Relay join code retrieved: {RelayJoinCode}");

                var joinAlloc = await RelayService.Instance.JoinAllocationAsync(RelayJoinCode);

                var serverData = new RelayServerData(
                    joinAlloc.RelayServer.IpV4,
                    (ushort)joinAlloc.RelayServer.Port,
                    joinAlloc.AllocationIdBytes,
                    joinAlloc.ConnectionData,
                    joinAlloc.HostConnectionData,
                    joinAlloc.Key,
                    false
                );

                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(serverData);

                Debug.Log("[Netcode] Starting client...");
                if (!NetworkManager.Singleton.IsListening)
                {
                    NetworkManager.Singleton.StartClient();
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"[Lobby ERROR] Failed to join lobby: {e.Message}\n{e}");
            }
            catch (RelayServiceException e)
            {
                Debug.LogError($"[Relay ERROR] Failed to join Relay: {e.Message}\n{e}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[GENERAL ERROR] Unexpected error while joining lobby: {e.Message}\n{e}");
            }
        }

        
        public async Task RefreshLobbyAsync()
        {
            if (Lobby != null)
            {
                Lobby = await LobbyService.Instance.GetLobbyAsync(Lobby.Id);
            }
        }

        private Dictionary<string, PlayerDataObject> GetPlayerData()
        {
            var settings = ServiceLocator.Get<SettingsManager>().UserSettings;

            return new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, settings.Username) },
                { "isAI", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "false") }
            };
        }
    }
}
