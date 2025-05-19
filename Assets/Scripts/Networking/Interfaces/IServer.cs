using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.player;
using Networking.DataTransferObjects;

namespace Networking.Interfaces
{
    public interface IServer
    {
        public void ReceiveClientInfo(ClientInfoDto clientInfoDto);
        public void SendGameState();
        public bool InitGameService(List<Player> players, GameSettings gameSettings);
    }
}