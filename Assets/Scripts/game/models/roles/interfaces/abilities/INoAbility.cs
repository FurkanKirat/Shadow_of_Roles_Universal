using Game.Models.Roles.Enums;

namespace game.models.roles.interfaces.abilities
{
    public interface INoAbility : IRoleAbility {
    
        AbilityResult DoNothing(){
            return AbilityResult.NoAbilityExists;
        }
    }


}
