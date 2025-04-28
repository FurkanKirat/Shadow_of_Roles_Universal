using System.Collections.Generic;
using game.Constants;
using game.models.player;
using game.models.player.properties;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class PlayerNamesController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNamesText, playerCountText;
        [SerializeField] private PlayerNamesContainer playerNamesContainer;
        [SerializeField] private Button startGameButton, increasePCountButton, decreasePCountButton;
        [SerializeField] private RolePackPanel rolePackPanel;
        public List<Player> Players { get; } = new ();
        private void Start()
        {
            playerNamesText.text = TextCategory.PlayerNames.GetTranslation("player_names");
            startGameButton.onClick.AddListener(StartGameClicked);
            increasePCountButton.onClick.AddListener(AddPlayer);
            decreasePCountButton.onClick.AddListener(RemovePlayer);
            UpdatePlayerCount();
            
        }

        private void AddPlayer()
        {
            playerNamesContainer.AddPlayer();
            UpdatePlayerCount();
        }

        private void RemovePlayer()
        {
            playerNamesContainer.RemovePlayer();
            UpdatePlayerCount();
        }

        private void UpdatePlayerCount()
        {
            playerCountText.text = playerNamesContainer.PlayerNames.Count.ToString();
        }

        private void StartGameClicked()
        {
            var playerNames = playerNamesContainer.PlayerNames;
            
            bool humanPlayerExist = false;
            for (int i = 0; i < playerNames.Count; i++)
            {
                bool isAIPlayer = playerNames[i].GetIsAI();
                string playerName = playerNames[i].GetPlayerName();
                PlayerType playerType = isAIPlayer ? PlayerType.AI : PlayerType.Human;
                Player player = Player.PlayerFactory.CreatePlayer(i + 1, playerName, playerType);
                Players.Add(player);
                
                if (!isAIPlayer)
                {
                    humanPlayerExist = true;
                }
            }
            
            if (!humanPlayerExist)
            {
                return;
            }
            
            rolePackPanel.gameObject.SetActive(true);
            
        }
        
        
    }
}