using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Networking.Managers;
using SceneControllers.PlayerNames;
using TMPro;
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

        public void JoinGame(LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
            lobbyIdText.text = $"Lobby Code: {_lobbyManager.LobbyJoinCode}";
            RefreshPlayerList();
            StartPollingLobby();
            
            startButton.onClick.AddListener(OnStartClicked);
        }

        private void RefreshPlayerList()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            var players = _lobbyManager.Lobby.Players;
            foreach (var player in players)
            {
                var item = Instantiate(playerEntryPrefab, content);
                string pName = player.Data != null && player.Data.ContainsKey("name")
                    ? player.Data["name"].Value
                    : "Unnamed";

                item.GetComponentInChildren<TextMeshProUGUI>().text = pName;
            }
        }

        private async void StartPollingLobby()
        {
            while (_lobbyManager.Lobby != null && isRefreshing)
            {
                await _lobbyManager.RefreshLobbyAsync(); // 👍 YENİ: Soyutlama eklendi
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
                string name = player.Data["name"].Value;
                rolePackPanel.Players.Add(Player.PlayerFactory.CreatePlayer(humanNumber++, name, PlayerType.Human));
            }

            int botName = 1;
            for (int i = _lobbyManager.Lobby.Players.Count + 1; i <= 5; i++)
            {
                rolePackPanel.Players.Add(Player.PlayerFactory.CreatePlayer(i, $"Bot {botName++}", PlayerType.AI));
            }

            rolePackPanel.gameObject.SetActive(true);
            isRefreshing = false;
        }
    }
}
