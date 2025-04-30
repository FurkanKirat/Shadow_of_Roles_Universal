using game.Constants;
using game.models.player;
using game.Utils;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Time = game.models.gamestate.Time;

namespace SceneControllers.GameScene
{
    public class PassTurnPanelController : MonoBehaviour
    {
        [SerializeField] private Button passButton;
        [SerializeField] private TextMeshProUGUI turnText;
        private PanelAnimator _panelAnimator;
        private string _textTemplate;
        private Player _player;
        private Time _time;
        private void Awake()
        {
            _textTemplate = TextCategory.Alerts.GetTranslation("pass_turn_message");
            
            string turnBtnText = TextCategory.General.GetTranslation("pass_turn");
            passButton.GetComponentInChildren<TextMeshProUGUI>().text = turnBtnText;
            
            _panelAnimator = gameObject.GetComponent<PanelAnimator>();
            passButton.onClick.AddListener(OnPassClicked);
        }

        private void OnPassClicked()
        {
            _panelAnimator.Hide();
            
        }

        public void UpdatePanel(Player player, Time time)
        {
            if (player == _player && _time == time) return;
            _player = player;
            _time = time;
            Image image = gameObject.GetComponentInChildren<Image>();
            int rand = RandomUtils.GetRandomNumber(0, 3);
            string timeStr = time.FormatEnum();
            string path = "Canvas/Lobby/" + timeStr + rand;
            image.sprite = Resources.Load<Sprite>(path);
            turnText.text = string.Format(_textTemplate, player.GetNameAndNumber());
        }
        
    }
}