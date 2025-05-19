
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class BackButton : MonoBehaviour
    {
        private Button _button;
        public void AddListener(UnityAction callback)
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(callback);
        }
    }
}