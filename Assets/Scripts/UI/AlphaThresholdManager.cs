using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AlphaThresholdManager : MonoBehaviour
    {
        [SerializeField] private float threshold = 0.5f;


        public void SetAlphaThreshold(Image targetImage)
        {
            if (targetImage != null)
            {
                targetImage.alphaHitTestMinimumThreshold = threshold;
            }
        }
        
        public void SetAlphaThreshold(Button button)
        {
            if (button == null) return;
            
            var image = button.GetComponent<Image>();
            if (image == null) return;
            
            image.alphaHitTestMinimumThreshold = threshold;
            
        }

        public void SetAlphaThresholdForAllButtons()
        {
            var images = FindObjectsByType<Image>(FindObjectsSortMode.None);
            
            foreach (var image in images)
            {
                SetAlphaThreshold(image);
            }
        }
    }
}