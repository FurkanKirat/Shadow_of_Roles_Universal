using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;
using game.Utils;

namespace game.models.roles.Templates.FolkRoles
{
    public class FolkHero : RoleTemplate
    {
        public FolkHero() : base(RoleId.FolkHero, RoleCategory.FolkProtector, RolePriority.Immune,
            AbilityType.ActiveAll, WinningTeam.Folk)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.HasImmuneAbility)
                .SetCooldown(3); // Actually 2 cooldown
            
            ChanceProperty = ChancePropertyFactory.Unique(25);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility) this).PerformAbilityForImmuneAbilityRoles(roleOwner, choosenPlayer, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            RoleProperties.Cooldown.Reset();
            
            var template = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(RoleID, "ability_message")
            };
            
            gameService.MessageService.SendPrivateMessage(template, roleOwner);
            choosenPlayer.Role.IsImmune = true;
            return AbilityResult.Success;
                
        }
    }
}