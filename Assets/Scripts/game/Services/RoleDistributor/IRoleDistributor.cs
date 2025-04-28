using System.Collections.Generic;
using game.models.gamestate;
using game.models.roles.Templates;

namespace game.Services.RoleDistributor
{
    public interface IRoleDistributor
    {
        public List<RoleTemplate> DistributeRoles();
    }
}