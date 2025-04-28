using game.Constants;
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
            gameRulesText.text = TextCategory.RoleBook.GetTranslation("rules_text");
        }
    }
}