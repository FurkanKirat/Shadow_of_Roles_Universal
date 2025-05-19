using Managers;
using TMPro;
using UI;
using UnityEngine;

namespace SceneControllers
{
    public class CreditsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditsText;
        [SerializeField] private BackButton backButton;
        private SceneChanger _sceneChanger;

        private void Start()
        {
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
            creditsText.text = TextManager.Translate("credits.credits_text");
            backButton.AddListener(GoBack);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack();
            }
        }

        private void GoBack()
        {
            _sceneChanger.GoBack();
        }
    }
}