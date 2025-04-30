using UnityEngine;

namespace SceneControllers.GameScene.Messages
{
    public class MessagesController : MonoBehaviour
    {
        private PanelAnimator _panelAnimator;


        private void Awake()
        {
            _panelAnimator = gameObject.GetComponent<PanelAnimator>();
        }
        
    }
}