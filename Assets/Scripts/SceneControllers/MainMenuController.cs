using Managers;
using Managers.enums;
using SceneControllers.GameGuide;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SceneControllers
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button startGameButton, 
            settingsButton, onlineButton, gameGuideButton, creditsButton, quitButton;

        [SerializeField] private Transform buttonsContainer;
        [SerializeField] private GameGuideController gameGuideController;
        private SceneChanger _sceneChanger;
        private AlphaThresholdManager _alphaThresholdManager;
        private void Start()
        {
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
            _alphaThresholdManager = ServiceLocator.Get<AlphaThresholdManager>();
            
            var buttons = buttonsContainer.GetComponentsInChildren<Button>();
            
            InitText(startGameButton, "offline");
            InitText(onlineButton, "online");
            InitText(gameGuideButton, "game_guide");
            InitText(creditsButton, "credits");
            InitText(quitButton, "quit");
            
            foreach (var button in buttons)
            {
               AddAlphaThreshold(button);
            }
            AddAlphaThreshold(settingsButton);
            
            AddListener(startGameButton, OnStartGameClicked);
            AddListener(onlineButton, OnOnlineModeClicked);
            AddListener(settingsButton, OnSettingsClicked);
            AddListener(quitButton, OnQuitClicked);
            AddListener(gameGuideButton, OnGameGuideClicked);
            AddListener(creditsButton, OnCreditsClicked);
        }

        

        private void InitText(Button button, string key)
        {
            InitText(button.GetComponentInChildren<TextMeshProUGUI>(), key);
        }
        private void InitText(TextMeshProUGUI text, string key)
        {
            text.text = TextManager.Translate($"main_menu.{key}");
        }
        private void AddListener(Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
        private void AddAlphaThreshold(Button button)
        {
            _alphaThresholdManager.SetAlphaThreshold(button);
        }

        private void LoadScene(SceneType sceneType)
        {
            _sceneChanger.LoadScene(sceneType);
        }
        private void OnStartGameClicked()
        {
            LoadScene(SceneType.PlayerNames);
        }

        private void OnSettingsClicked()
        {
            LoadScene(SceneType.Settings);
        }
        
        private void OnOnlineModeClicked()
        {
            LoadScene(SceneType.OnlineMode);
        }

        private void OnGameGuideClicked()
        {
            gameGuideController.gameObject.SetActive(true);
        }
        
        private void OnCreditsClicked()
        {
            _sceneChanger.LoadScene(SceneType.Credits);
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }

        
    }
}