using game.models.gamestate;
using game.models.player;
using Unity.Netcode;
using UnityEngine;
using game.Services.GameServices;
using Managers;
using System.Collections.Generic;
using Networking.DataTransferObjects;
using Networking.Interfaces;
using Newtonsoft.Json;

namespace Networking.Server
{
    public class OnlineServer : NetworkBehaviour, IServer
    {
        public static OnlineServer Instance { get; private set; }

        private OnlineGameService _gameService;

        // Bağlı oyuncuların clientId ↔ playerNumber eşlemesi
        private readonly Dictionary<ulong, int> _clientIdToPlayerNumberMap = new();

        private void Awake()
        {
            Instance = this;
        }
        
        public void InitGameService(List<Player> players, GameSettings gameSettings)
        {
            _gameService = (OnlineGameService)StartGameManager.InitializeGameService(players, gameSettings);

            // Oyuncuları bağlandıkları sıraya göre eşle
            int i = 0;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                _clientIdToPlayerNumberMap[clientId] = players[i].Number;
                i++;
            }
        }

        public void ReceiveClientInfoFromClient(ClientInfoDto clientInfoDto, ulong senderClientId)
        {
            if (!IsServer) return;

            _gameService.ReceiveInfo(clientInfoDto);
            SendGameStateToClient(senderClientId);
        }

        public void ReceiveClientInfo(ClientInfoDto clientInfoDto)
        {
            // IServer üzerinden çağrıldığında (local mantıkla)
            foreach (var pair in _clientIdToPlayerNumberMap)
            {
                if (pair.Value == clientInfoDto.Number)
                {
                    ReceiveClientInfoFromClient(clientInfoDto, pair.Key);
                    break;
                }
            }
        }

        public void SendGameState()
        {
            if (!IsServer) return;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SendGameStateToClient(clientId);
            }
        }

        public void SendGameStateToClient(ulong clientId)
        {
            if (!_clientIdToPlayerNumberMap.ContainsKey(clientId)) return;

            int playerNumber = _clientIdToPlayerNumberMap[clientId];
            var dto = _gameService.DtoProvider.GetGameInformationFor(playerNumber);
            string json = JsonConvert.SerializeObject(dto);

            SendGameStateToClientRpc(json, new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
            });
            Debug.Log(json);
        }

        [ClientRpc]
        private void SendGameStateToClientRpc(string json, ClientRpcParams rpcParams = default)
        {
            var dto = JsonConvert.DeserializeObject<GameDto>(json);
            ServiceLocator.Get<IClient>().ReceiveGameState(dto);
        }
    }
}