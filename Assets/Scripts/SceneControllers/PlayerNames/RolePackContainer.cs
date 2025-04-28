using System.Collections.Generic;
using game.models.gamestate;
using game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class RolePackContainer : MonoBehaviour
    {
        [SerializeField] private GameObject rolePackPrefab;
        [SerializeField] private RolesInfoContainer rolesInfoContainer;
        [SerializeField] private RolePackPanel rolePackPanel;
        private readonly List<RolePackInfo> _rolePacks = RolePackCatalog.GetAllModes(true, true);
        public void Init()
        {
            int index = 0;
            var scrollRect = rolesInfoContainer.GetComponentInParent<ScrollRect>();
            foreach (var rolePackInfo in _rolePacks)
            {
                var newBox = Instantiate(rolePackPrefab, gameObject.transform);
                var script = newBox.GetComponent<RolePackBox>();
                script.Initialize(rolePackInfo, rolesInfoContainer, rolePackPanel, scrollRect, index);
                rolePackPanel.RolePackBoxes.Add(script);
                index++;
            }
        }
    }
}