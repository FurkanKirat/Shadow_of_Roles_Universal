using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public class ComplexRoleBuilder : IRoleBuilderStrategy
    {
        public List<IRoleHint> Build(GameSettings gameSettings)
        {
            
            var corrupterCategories = new[] { RoleCategory.CorrupterSupport, RoleCategory.CorrupterAnalyst };
            var builder = new RolesBuilder(gameSettings)
                .AddHint(new CategoryHint(RoleCategory.CorrupterKilling))
                .AddHint(new CategoryHint(RoleCategory.FolkAnalyst))
                .AddHint(new MultipleCategoryHint(new[] { RoleCategory.FolkSupport, RoleCategory.FolkProtector }))
                .AddHint(new TeamHint(Team.Folk))
                .AddHint(new TeamHint(Team.Neutral))
                .AddConditionalHint(settings => settings.PlayerCount >= 6, new TeamHint(Team.Folk))
                
                .AddConditionalHint(settings => settings.PlayerCount >= 7, 
                    new SameChanceHint(new List<IRoleHint>{new TeamHint(Team.Neutral),new MultipleCategoryHint(corrupterCategories)}))
                
                .AddConditionalHint(settings => settings.PlayerCount >= 8, 
                    new SameChanceHint(new List<IRoleHint>{new TeamHint(Team.Neutral),new MultipleCategoryHint(corrupterCategories)}))
                
                .AddConditionalHint(settings => settings.PlayerCount >= 9, new TeamHint(Team.Folk))
                .AddConditionalHint(settings => settings.PlayerCount >= 10, 
                    new SameChanceHint(new List<IRoleHint>{new TeamHint(Team.Neutral),new MultipleCategoryHint(corrupterCategories)}))
                .Build();
            return builder;
        }
    }
}
