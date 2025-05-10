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
    public class LobbyManager
    {
        public Lobby Lobby { get; set; }
        public string LobbyCode {get; private set;}
        
        /// <summary>
        /// Host lobby + create Relay + start host
        /// </summary>
        public async Task CreateLobbyWithRelayAsync()
        {
            await OnlineServicesManager.InitializeAndSignInAsync();

            // Create Relay Allocation
            var allocation = await RelayService.Instance.CreateAllocationAsync(GameConstants.MaxPlayerCount);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            var player = new Player(
                id: AuthenticationService.Instance.PlayerId,
                data: GetPlayerData()
            );
            // Create Lobby with joinCode
            var lobby = await LobbyService.Instance.CreateLobbyAsync("ShadowOfRoles", GameConstants.MaxPlayerCount, new CreateLobbyOptions
            {
                Player = player,
                Data = new Dictionary<string, DataObject>
                {
                    { "joinCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                }
            });

            Lobby = lobby;
            LobbyCode = lobby.LobbyCode;

            // Setup RelayServerData
            var serverData = new RelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.ConnectionData,
                allocation.ConnectionData, // host uses own
                allocation.Key,
                true // isSecure (DTLS)
            );

            // Set relay to transport and start host
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(serverData);
            NetworkManager.Singleton.StartHost();
        }

        /// <summary>
        /// Join lobby + connect to Relay + start client
        /// </summary>
        public async Task JoinLobbyWithRelayAsync(string lobbyCode)
        {
            await OnlineServicesManager.InitializeAndSignInAsync();
         
            LobbyCode = lobbyCode;
            var joinOptions = new JoinLobbyByCodeOptions
            {
                Player = new Player(
                    id: AuthenticationService.Instance.PlayerId,
                    connectionInfo: null,
                    data: GetPlayerData()
                )
            };
            
            // Join Lobby
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinOptions);
            string joinCode = lobby.Data["joinCode"].Value;

            // Join Relay
            var joinAlloc = await RelayService.Instance.JoinAllocationAsync(joinCode);

            var serverData = new RelayServerData(
                joinAlloc.RelayServer.IpV4,
                (ushort)joinAlloc.RelayServer.Port,
                joinAlloc.AllocationIdBytes,
                joinAlloc.ConnectionData,
                joinAlloc.HostConnectionData,
                joinAlloc.Key,
                true // isSecure
            );

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(serverData);
            NetworkManager.Singleton.StartClient();
            Lobby = lobby;
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
