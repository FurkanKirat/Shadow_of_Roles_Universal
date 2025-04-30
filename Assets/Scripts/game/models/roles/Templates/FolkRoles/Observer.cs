using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.FolkRoles
{
    public class Observer : RoleTemplate, IInvestigativeAbility
    {
        public Observer(): base(RoleId.Observer, RoleCategory.FolkAnalyst, RolePriority.None,
            AbilityType.ActiveOthers, WinningTeam.Folk)
        {
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IInvestigativeAbility) this).ObserverAbility(roleOwner, choosenPlayer, gameService);
        }

        public override ChanceProperty GetChanceProperty()
        {
            return new ChanceProperty(20, ChanceProperty.NoMaxLimit);
        }
    }
}