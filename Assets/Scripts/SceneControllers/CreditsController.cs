using game.Constants;
using Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers
{
    public class CreditsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditsText;

        private void Start()
        {
            creditsText.text = TextCategory.Credits.GetTranslation("credits_text");
        }
    }
}