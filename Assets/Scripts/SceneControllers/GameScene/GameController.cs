using System;
using System.Collections;
using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.Services;
using game.Utils;
using Managers;
using Managers.enums;
using Networking.DataTransferObjects;
using Networking.Interfaces;
using SceneControllers.GameScene.Graveyard;
using SceneControllers.GameScene.Messages;
using SceneControllers.GameScene.RoleBook;
using SceneControllers.GameScene.SpecialRoles;
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
        [SerializeField] private TextMeshProUGUI timeText, nameText, numberText, roleText, rolePackText, clockText;
        [SerializeField] private Image backgroundImage, messageNotificationImage;
        [SerializeField] private AlivePlayersLayout alivePlayersLayout;
        [SerializeField] private GraveyardLayout graveyardLayout;
        [SerializeField] private MessagesLayout messagesLayout;
        [SerializeField] private RoleBookPanel roleBookPanel;
        [SerializeField] private SpecialRolesContainer specialRolesContainer;
        [SerializeField] private PhaseCountdownUI phaseCountdownUI;
        private PanelController _panelController;
        private AlphaThresholdManager _alphaThresholdManager;
        
        private Sprite _daySprite, _nightSprite, _votingSprite;
        
        private IGameInformation _gameInformation;
        private IClient _client;
        private GameSettings _gameSettings;
        private SceneChanger _sceneChanger;
        private readonly Dictionary<int, TimePeriod> _messagesLastCheck = new ();
        private Time _lastTime;

        private void Start()
        {
            ServiceLocator.Register(this);
    
            // StartCoroutine çağrısından önce client var mı diye kontrol et
            if (ServiceLocator.TryGet<IClient>(out var client))
            {
                _client = client;
                _client.InitUIUpdater(this);
                StartCoroutine(WaitForGameInformation());
            }
            else
            {
                Debug.LogError("IClient not registered yet. Delaying StartCoroutine...");
                StartCoroutine(WaitForClientAndThenStart());
            }
        }

        private IEnumerator WaitForClientAndThenStart()
        {
            while (!ServiceLocator.TryGet(out _client))
                yield return null;

            StartCoroutine(WaitForGameInformation());
        }

        private IEnumerator WaitForGameInformation()
        {
            while (_client?.GetCurrentGameInformation() == null)
                yield return null;

            Initialize(_client.GetCurrentGameInformation());
        }

        
        public void Initialize(IGameInformation gameInformation)
        {
            _gameInformation = gameInformation;

            InitScripts();
            InitTexts();
            LoadSprites();
            InitMessagesLastCheck();
            ChangePlayerUI();
            ToggleTimeCycleUI();
            SetOnClickListeners();
        }


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            bool isActivePanel = _panelController.HideActivePanel();
            
            if(!isActivePanel) _sceneChanger.GoBack();
            
        }
        
        public void UpdateUI(IGameInformation gameInformation)
        {
            _gameInformation = gameInformation;
            PassTurn(_lastTime != gameInformation.TimePeriod.Time, _gameSettings.GameMode == GameMode.Local);
            _lastTime = gameInformation.TimePeriod.Time;
        }

        private void InitMessagesLastCheck()
        {
            var timePeriod = TimePeriod.Start();
            foreach (var player in _gameInformation.AllPlayers)
            {
                _messagesLastCheck[player.Number] = timePeriod;
            }
        }

        private void InitScripts()
        {
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
            
            _gameSettings = _gameInformation.GameSettings;
            _panelController = GetComponent<PanelController>();
            roleBookPanel.Initialize(_gameInformation.GameSettings.RolePack);
            _alphaThresholdManager = ServiceLocator.Get<AlphaThresholdManager>();
            specialRolesContainer.InitializePanel(_gameInformation);
        }
        
        private void InitTexts()
        {
            rolePackText.text = string.Format(
                TextManager.Translate("role_pack_texts.current_role_pack")
                , TextManager.Translate($"role_pack.{_gameSettings.RolePack.FormatEnum()}"));
            
            phaseCountdownUI.StartCountdown(_gameSettings.PhaseTime);
        }
        
        private void LoadSprites()
        {
            _daySprite = Resources.Load<Sprite>("Canvas/Phase/day0");
            _nightSprite = Resources.Load<Sprite>("Canvas/Phase/night0");
            _votingSprite = Resources.Load<Sprite>("Canvas/Phase/voting0");
        }

       
        private void ChangePlayerUI()
        {
            PlayerDto currentPlayer = _gameInformation.CurrentPlayer;
            string numberString = TextManager.Translate("general.player_number");
            
            numberText.text = string.Format(numberString, currentPlayer.Number);
            nameText.text = currentPlayer.Name;
            roleText.text = RoleCatalog.GetRole(currentPlayer.RoleDto.RoleId).GetName();
            
            alivePlayersLayout.RefreshAllBoxes(currentPlayer);
            ManageNotification();
            specialRolesContainer.ResetBoxes();
            specialRolesContainer.ShowPanel(currentPlayer.RoleDto, _client.GetCurrentGameInformation().TimePeriod.Time);
        }

        private void SetOnClickListeners()
        {
            GameMode gameMode = _gameSettings.GameMode;
            switch (gameMode)
            {
                case GameMode.Online:
                    passTurnButton.gameObject.SetActive(false);
                    break;
                case GameMode.Local:
                    passTurnButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        TextManager.Translate("general.pass_turn");
                    passTurnButton.onClick.RemoveAllListeners();
                    passTurnButton.onClick.AddListener(SendInfo);
                    clockText?.GetComponentInParent<Image>()?.gameObject.SetActive(false);
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

        private void SendInfo()
        {
            _client.GetCurrentClientInfo().Number = _gameInformation.CurrentPlayer.Number;
            _client.SendClientInfo();
            _client.ResetClientInfo();
        }
        
        private void ShowMessagesPanel()
        {
            _panelController.ShowPanel("MessagesPanel");
            messagesLayout.RefreshLayout(_gameInformation.CurrentPlayer, _gameInformation.Messages);
            _messagesLastCheck[_gameInformation.CurrentPlayer.Number] = (TimePeriod)_gameInformation.TimePeriod.Clone();
            ManageNotification();
        }
        
        private void ShowRoleBookPanel()
        {
            _panelController.ShowPanel("RoleBookPanel");
            roleBookPanel.SelectRole(_gameInformation.CurrentPlayer.RoleDto.RoleId);
        }

        private void ShowGraveyardPanel()
        {
            const string panel = "GraveyardPanel";
            _panelController.ShowPanel(panel);
            graveyardLayout.RefreshLayout(_gameInformation.DeadPlayers);
        }

        private void ManageNotification()
        {
            int currentPlayerNum = _gameInformation.CurrentPlayer.Number;
            TimePeriod lastCheckTimePeriod = _messagesLastCheck.GetValueOrDefault(currentPlayerNum);
            bool isChecked = lastCheckTimePeriod.GetPrevious(_gameSettings.GameMode) >= _gameInformation.LastMessagePeriod;
            messageNotificationImage.gameObject.SetActive(!isChecked);
            
        }

        private void PassTurn(bool timeChanged, bool playerChanged = false)
        {
            if (timeChanged)
            {
                ToggleTimeCycleUI();
                phaseCountdownUI.StartCountdown(_gameSettings.PhaseTime);
                
            }

            if (playerChanged)
            {
                ChangePlayerUI();
                _panelController.ShowPanel("PassTurnPanel");
                var passTurnController = _panelController.GetComponent<PassTurnPanelController>("PassTurnPanel");
                passTurnController.UpdatePanel(_gameInformation.CurrentPlayer, _gameInformation.TimePeriod.Time);
            }
            
        }

        private void ToggleTimeCycleUI()
        {
            if (_gameInformation.GameStatus == GameStatus.Ended)
            {
                _sceneChanger.LoadScene(SceneType.GameEnd);
                return;
            }
            
            TimePeriod timePeriod = _gameInformation.TimePeriod;
            Time time = timePeriod.Time;
            string timeString = TextManager.Translate($"time.{timePeriod.Time.FormatEnum()}");
            timeText.text = string.Format(timeString, timePeriod.DayCount);

            alivePlayersLayout.RefreshLayout(_gameInformation.CurrentPlayer, timePeriod.Time);
            
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