using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.roles.Templates;
using game.Services;
using Managers;
using SceneControllers.GameScene.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.RoleBook
{
    public class RoleBookPanel : MonoBehaviour
    {
        [SerializeField] private RoleBookRolesContainer roleBookRolesContainer;
        [SerializeField] private RoleBookRolePanel roleBookRolePanel;
        [SerializeField] private ScrollRect scrollRect;
        private RoleTemplate _currentRoleTemplate;
        private RolePackInfo _gameRolePackInfo;
        public List<RoleBookRoleBox> RolePackBoxes { get; } = new();
        private int _selectedRolePackIndex = 0;
        private List<RoleTemplate> _roles;

        public void Initialize(RolePack rolePack)
        {
            _roles = RoleCatalog.GetAllRoles(rolePack);
            
            roleBookRolesContainer.Init(rolePack);
            
        }

        public void SelectRole(RoleTemplate roleTemplate, int index)
        {
            if (_currentRoleTemplate != null && roleTemplate.RoleID == _currentRoleTemplate.RoleID) return;
            
           _currentRoleTemplate = roleTemplate;
           _selectedRolePackIndex = index;

           for (int i = 0; i < RolePackBoxes.Count; ++i)
           {
               var rolePackBox = RolePackBoxes[i];
               rolePackBox.GetComponentInChildren<Button>().GetComponent<Image>().color
                   = i == _selectedRolePackIndex ? Color.cyan : Color.white;
               
           }
           roleBookRolePanel.ChangeInfo(roleTemplate);
        }
        
        public void SelectRole(RoleTemplate roleTemplate)
        {
            int index = _roles.IndexOf(roleTemplate);
            SelectRole(roleTemplate, index);
            scrollRect.ScrollToIndex(index, _roles.Count);
        }
        
    }
}