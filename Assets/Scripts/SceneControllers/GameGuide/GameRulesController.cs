using Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameGuide
{
    public class GameRulesController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameRulesText;


        public void Start()
        {
            gameRulesText.text = TextManager.Translate("role_book.rules_text");
        }
    }
}