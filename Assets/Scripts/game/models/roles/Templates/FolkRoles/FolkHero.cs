using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services;
using game.Services.GameServices;
using Managers;

namespace game.models.roles.Templates.FolkRoles
{
    public class FolkHero : RoleTemplate
    {
        public FolkHero() : base(RoleId.FolkHero, RoleCategory.FolkProtector, RolePriority.Immune,
            AbilityType.ActiveAll, WinningTeam.Folk)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.HasImmuneAbility)
                .SetAbilityUsesLeft(2);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility) this).PerformAbilityForImmuneAbilityRoles(roleOwner, choosenPlayer, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            if(RoleProperties.AbilityUsesLeft > 0){
                SendAbilityMessage(TextManager.GetEnumCategoryTranslation(RoleID, "ability_message"),
                    roleOwner, gameService.MessageService);
                choosenPlayer.Role.IsImmune = true;
                RoleProperties.DecrementAbilityUsesLeft();
                return AbilityResult.Success;
            }
            return AbilityResult.AbilityNotReady;
        }

        public override ChanceProperty GetChanceProperty()
        {
            return new ChanceProperty(25, ChanceProperty.Unique);
        }
    }
}