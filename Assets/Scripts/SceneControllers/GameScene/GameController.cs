using System;
using System.Collections.Generic;
using game.Constants;
using game.models;
using game.models.gamestate;
using game.models.player;
using game.Services.GameServices;
using game.Utils;
using Managers;
using Managers.enums;
using SceneControllers.GameScene.Graveyard;
using SceneControllers.GameScene.Messages;
using SceneControllers.GameScene.RoleBook;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Time = game.models.gamestate.Time;

namespace SceneControllers.GameScene
{
    public class GameController : MonoBehaviour
    {
   
        [SerializeField] private Button messagesButton, roleBookButton, graveyardButton, passTurnButton;
        [SerializeField] private TextMeshProUGUI timeText, nameText, numberText, roleText, rolePackText;
        [SerializeField] private Image backgroundImage, messageNotificationImage;
        [SerializeField] private AlivePlayersLayout alivePlayersLayout;
        [SerializeField] private GraveyardLayout graveyardLayout;
        [SerializeField] private MessagesLayout messagesLayout;
        [SerializeField] private RoleBookPanel roleBookPanel;
        private PanelController _panelController;
        private AlphaThresholdManager _alphaThresholdManager;
        
        private Sprite _daySprite, _nightSprite, _votingSprite;
        
        private IDataProvider _gameInformation;
        private GameSettings _gameSettings;
        private readonly Dictionary<int, TimePeriod> _messagesLastCheck = new ();

        private void Start()
        {
            InitScripts();
            InitTexts();
            LoadSprites();
            InitMessagesLastCheck();
            ChangePlayerUI();
            ToggleTimeCycleUI();
            SetOnClickListeners();
        }

        private void InitMessagesLastCheck()
        {
            var timePeriod = TimePeriod.Start();
            foreach (var player in _gameInformation.GetAllPlayers())
            {
                _messagesLastCheck[player.Number] = timePeriod;
            }
        }

        private void InitScripts()
        {
            _gameInformation = ServiceLocator.Get<StartGameManager>().GameService;
            _gameSettings = _gameInformation.GetGameSettings();
            _panelController = GetComponent<PanelController>();
            roleBookPanel.Initialize(_gameInformation.GetGameSettings().RolePack);
            _alphaThresholdManager = ServiceLocator.Get<AlphaThresholdManager>();
        }
        
        private void InitTexts()
        {
            rolePackText.text = string.Format(
                TextCategory.RolePackTexts.GetTranslation("current_role_pack")
                , TextCategory.RolePack.GetEnumTranslation(_gameSettings.RolePack));
        }
        
        private void LoadSprites()
        {
            _daySprite = Resources.Load<Sprite>("Canvas/Phase/day0");
            _nightSprite = Resources.Load<Sprite>("Canvas/Phase/night0");
            _votingSprite = Resources.Load<Sprite>("Canvas/Phase/voting0");
        }

        private void ChangePlayerUI()
        {
            Player currentPlayer = _gameInformation.GetCurrentPlayer();
            
            string numberString = TextCategory.General.GetTranslation("player_number");
            
            numberText.text = string.Format(numberString, currentPlayer.Number);
            nameText.text = currentPlayer.Name;
            roleText.text = currentPlayer.Role.Template.GetName();
            
            alivePlayersLayout.RefreshAllBoxes(currentPlayer);
            ManageNotification();
        }

        private void SetOnClickListeners()
        {
            GameMode gameMode = _gameSettings.GameMode;
            switch (gameMode)
            {
                case GameMode.Online:
                    passTurnButton.gameObject.SetActive(false);
                    break;
                case GameMode.Offline:
                    passTurnButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        TextCategory.General.GetTranslation( "pass_turn");
                    passTurnButton.onClick.AddListener(PassTurn);
                    break;
                default : 
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, "Unknown game mode!");
            }
            
            messagesButton.onClick.AddListener(ShowMessagesPanel);
            graveyardButton.onClick.AddListener(ShowGraveyardPanel);
            roleBookButton.onClick.AddListener(ShowRoleBookPanel);
            
            _alphaThresholdManager.SetAlphaThreshold(messagesButton);
            _alphaThresholdManager.SetAlphaThreshold(graveyardButton);
            _alphaThresholdManager.SetAlphaThreshold(roleBookButton);
            
        }
        
        private void ShowMessagesPanel()
        {
            _panelController.ShowPanel("MessagesPanel");
            messagesLayout.RefreshLayout(_gameInformation.GetCurrentPlayer(), _gameInformation.GetMessages());
            _messagesLastCheck[_gameInformation.GetCurrentPlayer().Number] = (TimePeriod)_gameInformation.GetTimePeriod().Clone();
            ManageNotification();
        }
        
        private void ShowRoleBookPanel()
        {
            _panelController.ShowPanel("RoleBookPanel");
            roleBookPanel.SelectRole(_gameInformation.GetCurrentPlayer().Role.Template);
        }

        private void ShowGraveyardPanel()
        {
            const string panel = "GraveyardPanel";
            _panelController.ShowPanel(panel);
            graveyardLayout.RefreshLayout(_gameInformation.GetDeadPlayers());
        }

        private void ManageNotification()
        {
            int currentPlayerNum = _gameInformation.GetCurrentPlayer().Number;
            TimePeriod lastCheckTimePeriod = _messagesLastCheck.GetValueOrDefault(currentPlayerNum);
            bool isChecked = lastCheckTimePeriod.GetPrevious() >= _gameInformation.GetLastMessagePeriod();
            messageNotificationImage.gameObject.SetActive(!isChecked);
            
        }

        private void PassTurn()
        {
            var gameService = _gameInformation as SingleDeviceGameService;
            
            if (gameService.PassTurn())
            {
                ToggleTimeCycleUI();
            }
            ChangePlayerUI();
            _panelController.ShowPanel("PassTurnPanel");
            var passTurnController = _panelController.GetComponent<PassTurnPanelController>("PassTurnPanel");
            passTurnController.UpdatePanel(_gameInformation.GetCurrentPlayer(), _gameInformation.GetTimePeriod().Time);
        }

        private void ToggleTimeCycleUI()
        {
            if (_gameInformation.GetGameStatus() == GameStatus.Ended)
            {
                var sceneChanger = ServiceLocator.Get<SceneChanger>();
                sceneChanger.LoadScene(SceneType.GameEnd);
                return;
            }
            
            TimePeriod timePeriod = _gameInformation.GetTimePeriod();
            Time time = timePeriod.Time;
            string timeString = TextCategory.Time.GetTranslation(timePeriod.Time.FormatEnum());
            timeText.text = string.Format(timeString, timePeriod.DayCount);

            alivePlayersLayout.RefreshLayout(_gameInformation.GetCurrentPlayer(), timePeriod.Time);
            
            backgroundImage.sprite = time switch
            {
                Time.Day => _daySprite,
                Time.Night => _nightSprite,
                Time.Voting => _votingSprite,
                _ => throw new ArgumentOutOfRangeException(nameof(time), time, "Unknown time")
            };
        }
    }
}