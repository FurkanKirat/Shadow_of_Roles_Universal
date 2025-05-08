using System;
using UnityEngine;

namespace UI
{
    public class SquarePanelScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRect;

        private void Start()
        {
            float screenHeight = Screen.height;
            float screenWidth = Screen.width;
            float targetHeight = screenHeight * 0.85f;
            float targetWidth = targetHeight * 1.5f;

            float widthLimit = screenWidth * 0.9f;
            targetWidth = Math.Max(widthLimit, targetWidth);
            
            Canvas canvas = panelRect.GetComponentInParent<Canvas>();
            float sf = canvas.scaleFactor;
            panelRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal, targetWidth / sf);
            panelRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,   targetHeight / sf);
        }
    }

}