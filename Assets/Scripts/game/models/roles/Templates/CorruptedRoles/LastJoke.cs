using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class LastJoke : RoleTemplate, IAttackAbility
    {
        public LastJoke() : base(RoleId.LastJoke, RoleCategory.CorrupterSupport, 
            RolePriority.LastJoke, AbilityType.OtherThanTeamMembers, WinningTeam.Corrupter)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.KnowsTeamMembers)
                .AddAttribute(RoleAttribute.HasPostDeathEffect)
                .AddAttribute(RoleAttribute.RoleBlockImmune)
                .AddAttribute(RoleAttribute.HasAttackAbility)
                .SetAbilityUsesLeft(1)
                .SetAttack(3);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ExecuteAbility(roleOwner,choosenPlayer, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            if(CanUseAbility() && !roleOwner.DeathProperties.IsAlive){

                RoleProperties.AbilityUsesLeft.DecrementCurrent();

                if(choosenPlayer==null){
                    return AbilityResult.NoOneSelected;
                }
                return ((IAttackAbility) this).Attack(roleOwner,choosenPlayer, gameService, CauseOfDeath.LastJoke);
            }

            return AbilityResult.AbilityNotReady;
        }
        
        public bool CanUseAbility() {
            return RoleProperties.AbilityUsesLeft.Current > 0;
        }

        public override ChanceProperty GetChanceProperty()
        {
            return new ChanceProperty(15, ChanceProperty.Unique);
        }
    }
}