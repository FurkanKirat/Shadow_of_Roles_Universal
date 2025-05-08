using game.models;
using Networking.DataTransferObjects;

namespace Networking.Interfaces
{
    public interface IServer
    {
        public void ReceiveClientInfo(ClientInfoDto clientInfoDto);
        public void SendGameState();
    }
}