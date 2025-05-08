using Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers
{
    public class CreditsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditsText;
        private SceneChanger _sceneChanger;

        private void Start()
        {
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
            creditsText.text = TextManager.Translate("credits.credits_text");
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _sceneChanger.GoBack();
            }
        }
    }
}