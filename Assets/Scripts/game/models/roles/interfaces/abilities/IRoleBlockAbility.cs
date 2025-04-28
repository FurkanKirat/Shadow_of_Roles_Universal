using game.models.player;
using Game.Models.Roles.Enums;

namespace game.models.roles.interfaces.abilities
{
    public interface IRoleBlockAbility
    {
        AbilityResult RoleBlock(Player target) {

            target.Role.CanPerform = false;

            return AbilityResult.Success;
        }
    }
}