using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class DarkSeer : RoleTemplate, IInvestigativeAbility
    {
        public DarkSeer() : base(RoleId.DarkSeer, RoleCategory.CorrupterAnalyst,
            RolePriority.None, AbilityType.Passive, WinningTeam.Corrupter)
        {
            RoleProperties.AddAttribute(RoleAttribute.KnowsTeamMembers);
            
            ChanceProperty = ChancePropertyFactory.Unlimited(10);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility) this).PerformAbilityForPassiveRoles(roleOwner, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IInvestigativeAbility) this).DarkSeerAbility(roleOwner, gameService);
        }
        
    }
}