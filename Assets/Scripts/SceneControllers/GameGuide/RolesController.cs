using System;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services;
using game.Utils;
using SceneControllers.GameScene.RoleBook;
using UnityEngine;

namespace SceneControllers.GameGuide
{
    public class RolesController : MonoBehaviour
    {
        private RoleBookPanel _rolesBookPanel;

        private void Start()
        {
            _rolesBookPanel = GetComponentInChildren<RoleBookPanel>();
            _rolesBookPanel.Initialize(RolePack.All);
            var firstId = EnumUtils.GetEnumValue<RoleId>(1);
            var firstRole = RoleCatalog.GetRole(firstId);
            _rolesBookPanel.SelectRole(firstRole);
        }
    }
}