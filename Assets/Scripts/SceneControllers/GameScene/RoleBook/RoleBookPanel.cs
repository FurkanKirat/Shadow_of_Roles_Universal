using System.Collections.Generic;
using game.Constants;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Services;
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

        public void SelectRole(RoleId roleId, int index)
        {
            if (_currentRoleTemplate != null && roleId == _currentRoleTemplate.RoleID) return;
            
           _currentRoleTemplate = RoleCatalog.GetRole(roleId);
           _selectedRolePackIndex = index;

           for (int i = 0; i < RolePackBoxes.Count; ++i)
           {
               var rolePackBox = RolePackBoxes[i];
               rolePackBox.GetComponentInChildren<Button>().GetComponent<Image>().color
                   = i == _selectedRolePackIndex ? UIConstants.Colors.Aquamarine : UIConstants.Colors.White;
               
           }
           roleBookRolePanel.ChangeInfo(_currentRoleTemplate);
        }
        
        public void SelectRole(RoleId roleId)
        {
            int index = _roles.IndexOf(RoleCatalog.GetRole(roleId));
            SelectRole(roleId, index);
            scrollRect.ScrollToIndex(index, _roles.Count);
        }
        
    }
}