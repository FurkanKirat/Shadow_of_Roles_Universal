using System.Collections.Generic;
using game.models.gamestate;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public interface IRoleBuilderStrategy
    {
        List<IRoleHint> Build(GameSettings gameSettings);
    }
}
