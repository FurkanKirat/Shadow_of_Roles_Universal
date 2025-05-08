using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.Helper
{
    public static class ScrollHelper
    {
        public static void ScrollToTop(this ScrollRect scrollRect)
        {
            scrollRect.ScrollToNormalizedPosition(1f);
        }

        public static void ScrollToBottom(this ScrollRect scrollRect)
        {
            scrollRect.ScrollToNormalizedPosition(0f);
        }

        public static void ScrollToIndex(this ScrollRect scrollRect, int index, int size)
        {
            if (index <= 1)
            {
                scrollRect.ScrollToTop();
                return;
            }
            float normalizedPosition = 1f - (float)index / (size - 1);
            scrollRect.ScrollToNormalizedPosition(normalizedPosition);
        }
        
        private static void ScrollToNormalizedPosition(this ScrollRect scrollRect, float position)
        {
            scrollRect.StartCoroutine(ScrollCoroutine(scrollRect, position));
        }

        private static IEnumerator ScrollCoroutine(ScrollRect scrollRect, float position)
        {
            yield return null;
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = position;
        }
        
        private static Coroutine StartCoroutine(this ScrollRect scrollRect, IEnumerator routine)
        {
            return CoroutineStarter.Instance.StartCoroutine(routine);
        }

        private class CoroutineStarter : MonoBehaviour
        {
            private static CoroutineStarter _instance;
            public static CoroutineStarter Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        var obj = new GameObject("ScrollHelperCoroutineStarter");
                        DontDestroyOnLoad(obj);
                        _instance = obj.AddComponent<CoroutineStarter>();
                    }
                    return _instance;
                }
            }
        }
    }
}