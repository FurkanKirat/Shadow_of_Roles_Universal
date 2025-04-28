using System.Collections.Generic;
using SceneControllers.GameScene.Graveyard;
using SceneControllers.GameScene.Messages;
using SceneControllers.GameScene.RoleBook;
using Scripts;
using UnityEngine;

namespace SceneControllers.GameScene
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private PassTurnPanelController passTurnPanelController;
        [SerializeField] private MessagesController messagesController;
        [SerializeField] private GraveyardController graveyardController;
        [SerializeField] private RoleBookController roleBookController;
        
        private readonly Dictionary<string, PanelAnimator> _panelAnimators = new();

        private void Awake()
        {
            _panelAnimators["PassTurnPanel"] = GetPanelAnimator(passTurnPanelController);
            //_panelAnimators["AnnouncementsPanel"] = announcementsPanel.GetComponentInChildren<PanelAnimator>();
            _panelAnimators["MessagesPanel"] = GetPanelAnimator(messagesController);
            _panelAnimators["GraveyardPanel"] = GetPanelAnimator(graveyardController);
            _panelAnimators["RoleBookPanel"] = GetPanelAnimator(roleBookController);
        }
        
        public void HidePanel(string panelName)
        {
            _panelAnimators[panelName].Hide();
        }

        public void ShowPanel(string panelName)
        {
            _panelAnimators[panelName].Show();
        }

        public T GetComponent<T>(string panelName) where T : Component
        {
            return _panelAnimators[panelName].GetComponent<T>();
        }

        private PanelAnimator GetPanelAnimator<T>(T component) where T : Component
        {
            return component.gameObject.GetComponentInChildren<PanelAnimator>();
        }
        
    }
}