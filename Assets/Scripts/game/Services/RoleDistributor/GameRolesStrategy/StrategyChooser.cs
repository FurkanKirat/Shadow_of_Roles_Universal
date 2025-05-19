using System;
using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public static class StrategyChooser
    {
        public static Dictionary<RoleId, int> CustomRoles { get; set; }
        public static IRoleBuilderStrategy GetStrategy(RolePack rolePack)
        {
            IRoleBuilderStrategy hints = rolePack switch
            {
                RolePack.Classic => new ClassicRoleBuilder(),
                RolePack.Complex => new ComplexRoleBuilder(),
                RolePack.Basic => new BasicRoleBuilder(),
                RolePack.DarkChaos => new DarkChaosRoleBuilder(),
                RolePack.Custom => new CustomBuilder(CustomRoles),
                _=> throw new ArgumentOutOfRangeException(nameof(rolePack), rolePack, "Unknown ruleSet")
            };
            return hints;
        }
        
    }
}