using System.Collections.Generic;
using SceneControllers.GameScene.Graveyard;
using SceneControllers.GameScene.Messages;
using SceneControllers.GameScene.RoleBook;
using UnityEngine;

namespace SceneControllers.GameScene
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private PassTurnPanelController passTurnPanelController;
        [SerializeField] private MessagesController messagesController;
        [SerializeField] private GraveyardController graveyardController;
        [SerializeField] private RoleBookController roleBookController;
        [SerializeField] private PanelAnimator announcementsAnimator;
        private readonly Dictionary<string, PanelAnimator> _panelAnimators = new();
        private readonly HashSet<string> activePanels = new();
        
        private void Awake()
        {
            _panelAnimators["PassTurnPanel"] = GetPanelAnimator(passTurnPanelController);
            _panelAnimators["AnnouncementsPanel"] = announcementsAnimator;
            _panelAnimators["MessagesPanel"] = GetPanelAnimator(messagesController);
            _panelAnimators["GraveyardPanel"] = GetPanelAnimator(graveyardController);
            _panelAnimators["RoleBookPanel"] = GetPanelAnimator(roleBookController);
        }
        
        public bool HideActivePanel()
        {
            foreach (var panel in activePanels)
            {
                _panelAnimators[panel].Hide();
            }
            return activePanels.Count != 0;
        }

        public void ShowPanel(string panelName, bool hideActivePanel = true)
        {
            if(hideActivePanel) HideActivePanel();
            activePanels.Add(panelName);
            _panelAnimators[panelName].Show();
        }

        public T GetComponent<T>(string panelName) where T : Component
        {
            return _panelAnimators[panelName].GetComponent<T>();
        }
        
        private PanelAnimator GetPanelAnimator<T>(T component) where T : Component
        {
            return component.gameObject.GetComponentInChildren<PanelAnimator>(true);
        }
        
    }
}