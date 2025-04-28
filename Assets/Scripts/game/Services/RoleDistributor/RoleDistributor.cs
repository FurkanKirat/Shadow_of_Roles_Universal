using System.Collections.Generic;
using game.models.gamestate;
using game.models.roles.Templates;
using game.Services.RoleDistributor.Hints;
using game.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace game.Services.RoleDistributor
{
    public class RoleDistributor : IRoleDistributor
    {
        private readonly List<IRoleHint> _hints;
        private readonly GameSettings _gameSettings;

        public RoleDistributor(List<IRoleHint> hints, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _hints = hints;
        }
        
        public List<RoleTemplate> DistributeRoles()
        {
            var roles = new Dictionary<RoleTemplate, int>();
            var roleList = new List<RoleTemplate>();
            foreach (var hint in _hints)
            {
                var role = hint.SelectRole(roles, _gameSettings.RolePack);
                roles[role] = roles.GetValueOrDefault(role) + 1;
            }

            foreach (var (role, count) in roles)
            {
                for (int i = 0; i < count; i++)
                {
                    roleList.Add(role);
                }
            }

            roleList.Shuffle();
            return roleList;
            
        }
        
    }
}