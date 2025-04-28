using System.Collections.Generic;
using game.models.player;

namespace game.models.roles.interfaces
{
    public interface IRoleAIBehavior {

        void ChooseRoleSpecificValues(List<Player> choosablePlayers);
    }
}