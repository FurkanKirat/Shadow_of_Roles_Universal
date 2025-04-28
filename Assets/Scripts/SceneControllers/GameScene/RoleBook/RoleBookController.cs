using Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.RoleBook
{
    public class RoleBookController : MonoBehaviour
    {
        private PanelAnimator _panelAnimator;


        private void Awake()
        {
            _panelAnimator = gameObject.GetComponent<PanelAnimator>();
        }
        
    }
}