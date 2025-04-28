using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class PanelAnimator : MonoBehaviour
    {
        private Animator _animator;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger("Show");
            StartCoroutine(EnableInteraction());
        }

        public void Hide()
        {
            _animator.SetTrigger("Hide");
            StartCoroutine(DisableAfterAnimation());
        }

        private IEnumerator EnableInteraction()
        {
            yield return new WaitForSeconds(0.3f);
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        private IEnumerator DisableAfterAnimation()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
        }
    }
}
