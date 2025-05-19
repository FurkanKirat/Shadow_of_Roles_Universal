using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Managers;
using TMPro;
using UI;
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
        [SerializeField] private BackButton backButton;
        private SceneChanger _sceneChanger;
        private void Start()
        {
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
            playerNamesText.text = TextManager.Translate("player_names.player_names");
            startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = TextManager.Translate("player_names.start_game");
            startGameButton.onClick.AddListener(StartGameClicked);
            increasePCountButton.onClick.AddListener(AddPlayer);
            decreasePCountButton.onClick.AddListener(RemovePlayer);
            backButton.AddListener(GoBack);
            UpdatePlayerCount();
            
        }
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            GoBack();
            
        }

        private void GoBack()
        {
            if(rolePackPanel.gameObject.activeSelf) rolePackPanel.ChangeVisibility(false);
            else _sceneChanger.GoBack();
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
            rolePackPanel.GameMode = GameMode.Local;
            var playerNames = playerNamesContainer.PlayerNames;
            rolePackPanel.Players.Clear();
            bool humanPlayerExist = false;
            for (int i = 0; i < playerNames.Count; i++)
            {
                bool isAIPlayer = playerNames[i].GetIsAI();
                string playerName = playerNames[i].GetPlayerName();
                PlayerType playerType = isAIPlayer ? PlayerType.AI : PlayerType.Human;
                Player player = Player.PlayerFactory.CreatePlayer(i + 1, playerName, playerType);
                rolePackPanel.Players.Add(player);
                
                if (!isAIPlayer)
                {
                    humanPlayerExist = true;
                }
            }
            
            if (!humanPlayerExist)
            {
                return;
            }
            
            rolePackPanel.ChangeVisibility(true);
            
        }
        
        
    }
}