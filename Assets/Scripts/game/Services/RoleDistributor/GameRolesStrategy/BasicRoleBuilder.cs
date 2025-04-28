using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public class BasicRoleBuilder : IRoleBuilderStrategy
    {
        public List<IRoleHint> Build(GameSettings gameSettings)
        {
            var distributor = new RolesBuilder(gameSettings)
                .AddHint(new RoleIdHint(RoleId.Psycho))
                .AddHint(new RoleIdHint(RoleId.SoulBinder))
                .AddHint(new RoleIdHint(RoleId.ChillGuy))
                .AddHint(new RoleIdHint(RoleId.ChillGuy))
                .AddHint(new RoleIdHint(RoleId.ChillGuy))
                .AddConditionalHint(settings => settings.PlayerCount >= 6, new RoleIdHint(RoleId.ChillGuy))
                .AddConditionalHint(settings => settings.PlayerCount >= 7, new RoleIdHint(RoleId.ChillGuy))
                .AddConditionalHint(settings => settings.PlayerCount >= 8, new RoleIdHint(RoleId.ChillGuy))
                .AddConditionalHint(settings => settings.PlayerCount >= 9, new RoleIdHint(RoleId.ChillGuy))
                .AddConditionalHint(settings => settings.PlayerCount >= 10, new RoleIdHint(RoleId.ChillGuy))
                .Build();
                
            return distributor;
        }
    }
}