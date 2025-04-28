using game.models.gamestate;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.Messages
{
    public class TimePeriodBox : MonoBehaviour
    {
        private TimePeriod _timePeriod;
        private TextMeshProUGUI _textMesh;
        public void Init(TimePeriod timePeriod)
        {
            _timePeriod = timePeriod;
            _textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _textMesh.text = _timePeriod.GetAsFormattedString();
        }
    }
}