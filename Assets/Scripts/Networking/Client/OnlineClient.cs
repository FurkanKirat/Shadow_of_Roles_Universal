using game.models;
using Managers;
using Networking.DataTransferObjects;
using Networking.Interfaces;
using Networking.Server;
using Newtonsoft.Json;
using SceneControllers.GameScene;
using Unity.Netcode;
using UnityEngine;

namespace Networking.Client
{
    public class OnlineClient : NetworkBehaviour, IClient
    {
        private GameController _uiUpdater;
        private ClientInfoDto _currentClientInfo = new();
        private IGameInformation _currentGameInformation;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                ServiceLocator.Register<IClient>(this);
                Debug.Log("OnlineClient registered.");

                // Şu anda client hazır, serverdan info isteyebilir
                RequestInitialGameStateServerRpc(); // örnek
            }
        }
        
        [ServerRpc]
        private void RequestInitialGameStateServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            OnlineServer.Instance.SendGameStateToClient(clientId); // veya başka bir şey
        }
        
        
        public void InitUIUpdater(GameController gameController)
        {
            _uiUpdater = gameController;
            _uiUpdater?.Initialize(_currentGameInformation);
        }

        public void SendClientInfo()
        {
            if (!IsOwner) return;

            string json = JsonConvert.SerializeObject(_currentClientInfo);
            Debug.Log("Server is sending game state to client: " + json);
            SendClientInfoToServerRpc(json);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendClientInfoToServerRpc(string json, ServerRpcParams rpcParams = default)
        {
            var info = JsonConvert.DeserializeObject<ClientInfoDto>(json);
            OnlineServer.Instance.ReceiveClientInfoFromClient(info, rpcParams.Receive.SenderClientId);
        }

        public void ReceiveGameState(IGameInformation gameInformation)
        {
            _currentGameInformation = gameInformation;
            _uiUpdater?.UpdateUI(gameInformation);
        }
        

        public IGameInformation GetCurrentGameInformation() => _currentGameInformation;
        public ClientInfoDto GetCurrentClientInfo() => _currentClientInfo;
        public void ResetClientInfo() => _currentClientInfo = new ClientInfoDto();
    }
}