using game.models;
using Networking.DataTransferObjects;
using Networking.Interfaces;
using SceneControllers.GameScene;
using UnityEngine;

namespace Networking.Client
{
    public class LocalClient : IClient
    {

        private readonly IServer _server;
        private GameController _uiUpdater;
        private IGameInformation _currentGameInformation;
        private ClientInfoDto _currentClientInfo = new ();
        public LocalClient(IServer server)
        {
            _server = server;
        }

        public void InitUIUpdater(GameController gameController)
        {
            _uiUpdater = gameController;
            _uiUpdater?.Initialize(_currentGameInformation);
        }

        public void SendClientInfo()
        {
            _server.ReceiveClientInfo(_currentClientInfo);
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
