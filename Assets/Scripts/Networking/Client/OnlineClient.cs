using game.models;
using Networking.DataTransferObjects;
using Networking.Interfaces;
using Networking.Server;
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

        public void InitUIUpdater(GameController gameController)
        {
            _uiUpdater = gameController;
            _uiUpdater?.Initialize(_currentGameInformation);
        }

        public void SendClientInfo()
        {
            if (!IsOwner) return;

            string json = JsonUtility.ToJson(_currentClientInfo);
            SendClientInfoToServerRpc(json);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendClientInfoToServerRpc(string json, ServerRpcParams rpcParams = default)
        {
            var info = JsonUtility.FromJson<ClientInfoDto>(json);
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