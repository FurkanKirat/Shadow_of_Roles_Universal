using UnityEngine;

namespace SceneControllers.GameScene.Graveyard
{
    public class GraveyardController : MonoBehaviour
    {
        private PanelAnimator _panelAnimator;
        
        private void Awake()
        {
            _panelAnimator = gameObject.GetComponent<PanelAnimator>();
        }
    }
}