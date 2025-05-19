using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class DarkRevealer : RoleTemplate, IInvestigativeAbility
    {
        public DarkRevealer() : base(RoleId.DarkRevealer, RoleCategory.CorrupterAnalyst, RolePriority.None, 
            AbilityType.OtherThanTeamMembers, WinningTeam.Corrupter)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.KnowsTeamMembers);
            
            ChanceProperty = ChancePropertyFactory.Unlimited(30);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IInvestigativeAbility) this).DetectiveAbility(roleOwner, choosenPlayer, gameService);
        }
    }
}