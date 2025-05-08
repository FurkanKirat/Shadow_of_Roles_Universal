using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private float alphaThreshold;
        private void Awake()
        {
            var image = GetComponent<Image>();
            if (image != null)
                image.alphaHitTestMinimumThreshold = alphaThreshold;
            var button = GetComponent<Button>();
            button.onClick.AddListener(Close);
        }

        private void Close()
        {
            if(targetObject != null) targetObject.SetActive(false);
            else gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

}
