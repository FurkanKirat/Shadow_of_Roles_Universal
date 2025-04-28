using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public class ClassicRoleBuilder : IRoleBuilderStrategy
    {
        public List<IRoleHint> Build(GameSettings gameSettings)
        {
            var distributor = new RolesBuilder(gameSettings)
                .AddHint(new RoleIdHint(RoleId.Psycho))
                .AddHint(new RoleIdHint(RoleId.SoulBinder))
                .AddHint(new CategoryHint(RoleCategory.FolkAnalyst))
                .AddHint(new RoleIdHint(RoleId.Clown))
                .AddHint(new MultipleCategoryHint(new[]
                    { RoleCategory.FolkVillager, RoleCategory.FolkSupport, RoleCategory.FolkAnalyst }))
                .AddConditionalHint(settings => settings.PlayerCount >= 6, new RoleIdHint(RoleId.ChillGuy))
                .AddConditionalHint(settings => settings.PlayerCount >= 7, new TeamHint(Team.Corrupter))
                .AddConditionalHint(settings => settings.PlayerCount >= 8, new TeamHint(Team.Folk))
                .AddConditionalHint(settings => settings.PlayerCount >= 9, new RoleIdHint(RoleId.Assassin))
                .AddConditionalHint(settings => settings.PlayerCount >= 10, new TeamHint(Team.Folk))
                .Build();

            return distributor;
        }
    }
}