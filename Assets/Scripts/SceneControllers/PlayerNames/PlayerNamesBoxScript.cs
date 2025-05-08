using Managers;
using TMPro;
using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;

namespace SceneControllers.PlayerNames
{
    public class PlayerNamesBoxScript : MonoBehaviour
    {
        private TMP_InputField _playerNamesText;
        private Toggle _isAIToggle;
        private void Awake()
        {
            _playerNamesText = gameObject.GetComponentInChildren<TMP_InputField>();
            _isAIToggle = gameObject.GetComponentInChildren<Toggle>();
            
            _isAIToggle.GetComponentInChildren<TextMeshProUGUI>().text 
                = TextManager.Translate("general.ai");
        }

        public void SetPlayerName(string playerName)
        {
            _playerNamesText.text = playerName;
        }
        
        public string GetPlayerName()
        {
            return _playerNamesText.text;
        }

        public void SetIsAI(bool isAI)
        {
            _isAIToggle.isOn = isAI;
        }

        public bool GetIsAI()
        {
            return _isAIToggle.isOn;
        }


    }

}
