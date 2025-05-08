using System.Collections.Generic;
using game.models.player;
using Managers;
using Networking.Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers.OnlineMode
{
    public class GameLobbyController : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject playerEntryPrefab;
        [SerializeField] private TextMeshProUGUI lobbyIdText;

        private readonly List<GameObject> _spawnedEntries = new();
        private LobbyManager _lobbyManager;

        public void RefreshPlayerList(List<LobbyPlayer> players)
        {
            foreach (var entry in _spawnedEntries)
            {
                Destroy(entry);
            }
            _spawnedEntries.Clear();
            
            foreach (var player in players)
            {
                var entry = Instantiate(playerEntryPrefab, content);
                var text = entry.GetComponentInChildren<TextMeshProUGUI>();
                text.text = player.Name;
                _spawnedEntries.Add(entry);
            }
        }

        public void JoinGame(LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
            lobbyIdText.text = $"LobbyId: {_lobbyManager.LobbyCode}";
        }
    }
}
