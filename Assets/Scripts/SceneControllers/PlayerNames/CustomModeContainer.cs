using System.Collections.Generic;
using Game.Models.Roles.Enums;
using game.Services;
using TMPro;
using UnityEngine;

namespace SceneControllers.PlayerNames
{
    public class CustomModeContainer : MonoBehaviour
    {
        [SerializeField] private GameObject rolePrefab;
        [SerializeField] private Transform roleContainer;
        [SerializeField] private TextMeshProUGUI playerCountText;
        private readonly List<CustomRoleBox> customRoleBoxes = new();

        public void Start()
        {
            var roles = RoleCatalog.GetAllRoles();

            foreach (var role in roles)
            {
                var roleBox = Instantiate(rolePrefab, roleContainer);
                var customRoleBox = roleBox.GetComponent<CustomRoleBox>();
                customRoleBox.Initialize(role.RoleID);
                customRoleBoxes.Add(customRoleBox);
            }
        }

        public Dictionary<RoleId, int> StartGame()
        {
            var roleDict = new Dictionary<RoleId, int>();
            
            foreach (var customRoleBox in customRoleBoxes)
            {
                roleDict.Add(customRoleBox.RoleId, customRoleBox.Count);
            }

            return roleDict;
        }

        public void UpdatePlayerCount(int playerCount)
        {
            playerCountText.text = playerCount.ToString();
        }
    }
}