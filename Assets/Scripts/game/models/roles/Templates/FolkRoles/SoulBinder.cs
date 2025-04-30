using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.FolkRoles
{
    public class SoulBinder : RoleTemplate, IProtectiveAbility
    {
        public SoulBinder() : base(RoleId.SoulBinder, RoleCategory.FolkProtector,
            RolePriority.Heal, AbilityType.ActiveOthers, WinningTeam.Folk)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.HasHealingAbility);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IProtectiveAbility) this).Heal(roleOwner, choosenPlayer, gameService);
        }

        public override ChanceProperty GetChanceProperty()
        {
            return new ChanceProperty(20, ChanceProperty.NoMaxLimit);;
        }
    }
}