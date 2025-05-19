using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using game.Services.GameServices;
using Managers;
using Networking.Client;
using Networking.DataTransferObjects;
using Networking.Interfaces;
using UnityEngine;

namespace Networking.Server
{
    public class LocalServer : IServer
    {
        private LocalClient ConnectedClient { get; set; }
        private LocalGameService GameService { get; set; }

        public bool InitGameService(List<Player> players, GameSettings gameSettings)
        {
            GameService = (LocalGameService)StartGameManager.InitializeGameService(players, gameSettings);
            if (GameService == null) return false;
            SendGameState();
            return true;
        }
        
        public void SetClient(LocalClient client)
        {
            ConnectedClient = client;
        }

        public void ReceiveClientInfo(ClientInfoDto clientInfoDto)
        {
            GameService.ReceiveInfo(clientInfoDto);
            SendGameState();
            
        }

        public void SendGameState()
        {
            ConnectedClient?.ReceiveGameState(GameService.DtoProvider.GetGameInformationFor(GameService.CurrentPlayer.Number));
        }
    }

}
