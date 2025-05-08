using game.models;
using Networking.DataTransferObjects;
using SceneControllers.GameScene;

namespace Networking.Interfaces
{
    public interface IClient
    {
        public void InitUIUpdater(GameController gameController);
        public void SendClientInfo();
        public void ReceiveGameState(IGameInformation gameInformation);
        public IGameInformation GetCurrentGameInformation();
        public ClientInfoDto GetCurrentClientInfo();
        public void ResetClientInfo();
    }
}
