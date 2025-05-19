using game.Constants;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Services;
using Managers;
using Networking.DataTransferObjects;
using SceneControllers.GameScene.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Time = game.models.gamestate.Time;

namespace SceneControllers.GameScene
{
    public class AlivePlayersBox : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI playerNameText, numberText, roleText;
        [SerializeField] private Image circleImage;
        private TextMeshProUGUI _buttonText;
        private PlayerDto _currentPlayer, _targetPlayer;
        private Time _time;
        private PlayerClick _playerClick;
        public int Index {get;set;}

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                UpdateButtonText();
            }
        }
        

        public void Init(PlayerDto currentPlayer, PlayerDto targetPlayer, Time time, PlayerClick playerClick, int index)
        {
            _targetPlayer = targetPlayer;
            _time = time;
            _playerClick = playerClick;
            Index = index;
            
            _buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            
            UpdateButtonText();
            UpdatePlayer(currentPlayer, targetPlayer);
            playerNameText.text = _targetPlayer.Name;
            numberText.text = _targetPlayer.Number.ToString();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                _playerClick?.Invoke(_currentPlayer, _targetPlayer, Index);
            });
        }

        public void UpdatePlayer(PlayerDto currentPlayer, PlayerDto targetPlayer)
        {
            _currentPlayer = currentPlayer;
            _targetPlayer = targetPlayer;
            UpdateCircleColor();
            UpdateButtonVisibility();
            UpdateRoleVisibilityAndText();
            
            IsSelected = false;
            
        }
        
        private void UpdateCircleColor()
        {
            circleImage.color = _currentPlayer.IsSamePlayer(_targetPlayer)
                ? UIConstants.Colors.Yellow 
                : UIConstants.Colors.Blue;
        }
        
        private void UpdateButtonVisibility()
        {
            var playerBtnVis = new PlayerButtonVisibility(_currentPlayer, _targetPlayer, _time);
            bool shouldShowButton = playerBtnVis.ShouldShowButton();
            SetCanvasGroupVisibility(button.GetComponent<CanvasGroup>(), shouldShowButton);
        }

        private void UpdateRoleVisibilityAndText()
        {
            var roleVisibility = new PlayerRoleVisibility(_currentPlayer, _targetPlayer);
            bool shouldShowRole = roleVisibility.ShouldShowRole();
    
            SetCanvasGroupVisibility(roleText.GetComponent<CanvasGroup>(), shouldShowRole);
            if (!shouldShowRole) return;
            RoleId targetId = _targetPlayer.RoleDto.RoleId;
            
            RoleTemplate role = RoleCatalog.GetRole(targetId);
            
            roleText.text = role.GetName();

            var roleTextColor = new RoleTextColor(role.WinningTeam);
            roleText.color = roleTextColor.GetColor();
        }

        private void SetCanvasGroupVisibility(CanvasGroup group, bool visible)
        {
            group.alpha = visible ? 1 : 0;
            group.interactable = visible;
            group.blocksRaycasts = visible;
        }
        
        public void UpdateTime(PlayerDto currentPlayer, PlayerDto targetPlayer, Time time)
        {
            _time = time;
            UpdatePlayer(currentPlayer, targetPlayer);
        }
        
        private void UpdateButtonText()
        {
            string key = IsSelected ? "unselect" : "select";
            _buttonText.text = TextManager.Translate($"general.{key}");
        }

        public bool IsPlayerAlive()
        {
            return _targetPlayer.DeathProperties.IsAlive;
        }
        
        
    }


}