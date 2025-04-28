using game.models.gamestate;

namespace game.models.roles.interfaces
{
    public interface IPriorityChangingRole {

        void ChangePriority(RolePack rolePack);
    }

}