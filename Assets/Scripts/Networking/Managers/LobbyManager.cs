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
        public string LobbyJoinCode { get; private set; }   // Kullanıcıların lobby'e girmesi için
        private string RelayJoinCode { get; set; }   // Relay bağlantısı için

        /// <summary>
        /// Host lobby + create Relay + start host
        /// </summary>
        public async Task CreateLobbyWithRelayAsync()
        {
            await OnlineServicesManager.InitializeAndSignInAsync();

            // 1. Create Relay Allocation
            var allocation = await RelayService.Instance.CreateAllocationAsync(GameConstants.MaxPlayerCount);
            RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            // 2. Create Lobby (with relayJoinCode stored as member-visible)
            var player = new Player(
                id: AuthenticationService.Instance.PlayerId,
                data: GetPlayerData()
            );

            Lobby = await LobbyService.Instance.CreateLobbyAsync("ShadowOfRoles", GameConstants.MaxPlayerCount, new CreateLobbyOptions
            {
                Player = player,
                Data = new Dictionary<string, DataObject>
                {
                    { "joinCode", new DataObject(DataObject.VisibilityOptions.Member, RelayJoinCode) }
                }
            });

            LobbyJoinCode = Lobby.LobbyCode;

            // 3. Setup Unity Transport with relay
            var serverData = new RelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.ConnectionData,
                allocation.ConnectionData, // host's own
                allocation.Key,
                true // DTLS
            );

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(serverData);

            NetworkManager.Singleton.StartHost();
        }

        /// <summary>
        /// Join lobby by lobby code + get Relay join code from lobby data + start client
        /// </summary>
        public async Task JoinLobbyWithRelayAsync(string lobbyCode)
        {
            await OnlineServicesManager.InitializeAndSignInAsync();

            LobbyJoinCode = lobbyCode;

            var joinOptions = new JoinLobbyByCodeOptions
            {
                Player = new Player(
                    id: AuthenticationService.Instance.PlayerId,
                    data: GetPlayerData()
                )
            };

            // 1. Join lobby
            Lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinOptions);

            // 2. Get relay join code from lobby's data
            RelayJoinCode = Lobby.Data["joinCode"].Value;

            // 3. Join relay using code from lobby
            var joinAlloc = await RelayService.Instance.JoinAllocationAsync(RelayJoinCode);

            var serverData = new RelayServerData(
                joinAlloc.RelayServer.IpV4,
                (ushort)joinAlloc.RelayServer.Port,
                joinAlloc.AllocationIdBytes,
                joinAlloc.ConnectionData,
                joinAlloc.HostConnectionData,
                joinAlloc.Key,
                true
            );

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(serverData);

            NetworkManager.Singleton.StartClient();
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
