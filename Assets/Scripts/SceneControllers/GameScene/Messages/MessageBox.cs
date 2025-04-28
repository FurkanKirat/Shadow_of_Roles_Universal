using game.Constants;
using game.models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.Messages
{
    public class MessageBox : MonoBehaviour
    {
        private TextMeshProUGUI _messageText;
        private Message _message;

        public void Init(Message message)
        {
            _message = message;
            
            _messageText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _messageText.text = _message.Text;

            var image = gameObject.GetComponentInChildren<Image>();
            if (message.IsPublic)
            {
                image.color = UIConstants.PublicColor;
                _messageText.color = UIConstants.PrivateColor;
            }
            else
            {
                image.color = UIConstants.PrivateColor;
                _messageText.color = UIConstants.PublicColor;
            }
        }
    }
}