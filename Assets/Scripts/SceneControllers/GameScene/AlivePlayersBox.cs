
using game.Constants;
using game.models.player;
using Managers;
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
        [SerializeField] private TextMeshProUGUI playerNameText, numberText;
        [SerializeField] private Image circleImage;
        private TextMeshProUGUI _buttonText;
        private Player _currentPlayer, _targetPlayer;
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
        

        public void Init(Player currentPlayer, Player targetPlayer, Time time, PlayerClick playerClick, int index)
        {
            _targetPlayer = targetPlayer;
            _time = time;
            _playerClick = playerClick;
            Index = index;
            
            _buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            
            UpdateButtonText();
            UpdatePlayer(currentPlayer);
            playerNameText.text = _targetPlayer.Name;
            numberText.text = _targetPlayer.Number.ToString();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                _playerClick?.Invoke(_currentPlayer, _targetPlayer, Index);
            });
        }

        public void UpdatePlayer(Player currentPlayer)
        {
            _currentPlayer = currentPlayer;
            circleImage.color = currentPlayer.IsSamePlayer(_targetPlayer) 
                ? UIConstants.CircleColorCurrent : UIConstants.CircleColorDefault;
            var playerBtnVis = new PlayerButtonVisibility(_currentPlayer, _targetPlayer, _time);
            bool shouldShowButton = playerBtnVis.ShouldShowButton();
            
            var cg = button.GetComponent<CanvasGroup>();
            
            cg.alpha = shouldShowButton ? 1 : 0;
            cg.interactable = shouldShowButton;
            cg.blocksRaycasts = shouldShowButton;
            
            IsSelected = false;
            
        }
        
        public void UpdateTime(Player currentPlayer, Time time)
        {
            _time = time;
            UpdatePlayer(currentPlayer);
        }
        
        private void UpdateButtonText()
        {
            string key = IsSelected ? "unselect" : "select";
            _buttonText.text = TextCategory.General.GetTranslation(key);
        }

        public bool IsPlayerAlive()
        {
            return _targetPlayer.DeathProperties.IsAlive;
        }
        
        
    }


}