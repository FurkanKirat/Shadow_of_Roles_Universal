using System.Collections.Generic;
using game.models.gamestate;
using game.models.roles.Templates;
using game.Services;
using game.Utils;
using UnityEngine;

namespace SceneControllers.GameScene.RoleBook
{
    public class RoleBookRolesContainer : MonoBehaviour
    {
        [SerializeField] private GameObject rolePackPrefab;
        [SerializeField] private RoleBookPanel rolePackPanel;
        private List<RoleTemplate> _roles;
        
        public void Init(RolePack rolePack)
        {
            _roles = RoleCatalog.GetAllRoles(rolePack);
            int index = 0;
            
            foreach (var role in _roles)
            {
                var newBox = Instantiate(rolePackPrefab, gameObject.transform);
                var script = newBox.GetComponent<RoleBookRoleBox>();
                script.Initialize(role, rolePackPanel, index);
                rolePackPanel.RolePackBoxes.Add(script);
                ++index;
            }
        }
    }
}