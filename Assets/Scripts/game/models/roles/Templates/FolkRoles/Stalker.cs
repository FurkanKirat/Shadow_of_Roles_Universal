using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.FolkRoles
{
    public class Stalker : RoleTemplate, IInvestigativeAbility
    {
        public Stalker() : base(RoleId.Stalker, RoleCategory.FolkAnalyst, RolePriority.None, AbilityType.ActiveOthers, WinningTeam.Folk)
        {
            ChanceProperty = ChancePropertyFactory.Unlimited(25);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IInvestigativeAbility) this).StalkerAbility(roleOwner, choosenPlayer, gameService);
        }
        
    }
}