using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Networking.Managers;
using SceneControllers.PlayerNames;
using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.OnlineMode
{
    public class GameLobbyController : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject playerEntryPrefab;
        [SerializeField] private TextMeshProUGUI lobbyIdText;
        [SerializeField] private Button startButton;
        [SerializeField] private RolePackPanel rolePackPanel;

        private readonly List<GameObject> _spawnedEntries = new();
        private LobbyManager _lobbyManager;
        private const float RefreshInterval = 3f;
        private bool isRefreshing = true;

        public void RefreshPlayerList()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            var players = _lobbyManager.Lobby.Players;

            foreach (var player in players)
            {
                GameObject item = Instantiate(playerEntryPrefab, content);
                string pName = (player.Data != null && player.Data.ContainsKey("name"))
                    ? player.Data["name"].Value
                    : "Unnamed";

                item.GetComponentInChildren<TextMeshProUGUI>().text = pName;
            }
        }

        public void JoinGame(LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
            lobbyIdText.text = $"LobbyId: {_lobbyManager.LobbyCode}";
            StartPollingLobby();
            
            startButton.onClick.AddListener(OnStartClicked);
        }
        
        private async void StartPollingLobby()
        {
            while (_lobbyManager.Lobby != null && isRefreshing)
            {
                var lobby = await LobbyService.Instance.GetLobbyAsync(_lobbyManager.Lobby.Id);
                _lobbyManager.Lobby  = lobby;
                RefreshPlayerList();

                await Task.Delay(TimeSpan.FromSeconds(RefreshInterval));
            }
        }

        private void OnStartClicked()
        {
            rolePackPanel.GameMode = GameMode.Online;
            int humanNumber = 1;
            foreach (var player in _lobbyManager.Lobby.Players)
            {
                rolePackPanel.Players.Add(Player.PlayerFactory.CreatePlayer(humanNumber, player.Data["name"].Value, PlayerType.Human));
                ++humanNumber;
            }

            int botName = 1;
            for (int i = _lobbyManager.Lobby.Players.Count + 1; i <= 5; i++)
            {
                rolePackPanel.Players.Add(Player.PlayerFactory.CreatePlayer(i, "Bot " + botName, PlayerType.AI));
                ++botName;
            }
            
            rolePackPanel.gameObject.SetActive(true);
            isRefreshing = false;
        }
    }
}
