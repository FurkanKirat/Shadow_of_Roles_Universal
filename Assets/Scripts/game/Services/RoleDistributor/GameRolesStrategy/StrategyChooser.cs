using System;
using game.models.gamestate;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public static class StrategyChooser
    {
        public static IRoleBuilderStrategy GetStrategy(RolePack rolePack)
        {
            IRoleBuilderStrategy hints = rolePack switch
            {
                RolePack.Classic => new ClassicRoleBuilder(),
                RolePack.Complex => new ComplexRoleBuilder(),
                RolePack.Basic => new BasicRoleBuilder(),
                RolePack.DarkChaos => new DarkChaosRoleBuilder(),
                _=> throw new ArgumentOutOfRangeException(nameof(rolePack), rolePack, "Unknown ruleSet")
            };
            return hints;
        }
    }
}