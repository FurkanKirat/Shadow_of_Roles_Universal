using System.Collections.Generic;
using game.models.gamestate;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public class DarkChaosRoleBuilder : IRoleBuilderStrategy
    {
        public List<IRoleHint> Build(GameSettings gameSettings)
        {
            var distributor = new RolesBuilder(gameSettings)
                .AddHint(new NoHint())
                .AddHint(new NoHint())
                .AddHint(new NoHint())
                .AddHint(new NoHint())
                .AddHint(new NoHint())
                .AddConditionalHint(settings => settings.PlayerCount >= 6, new NoHint())
                .AddConditionalHint(settings => settings.PlayerCount >= 7, new NoHint())
                .AddConditionalHint(settings => settings.PlayerCount >= 8, new NoHint())
                .AddConditionalHint(settings => settings.PlayerCount >= 9, new NoHint())
                .AddConditionalHint(settings => settings.PlayerCount >= 10, new NoHint())
                .Build();

            return distributor;
        }
    }
}