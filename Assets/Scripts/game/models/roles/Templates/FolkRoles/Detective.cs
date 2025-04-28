

using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services;
using game.Services.GameServices;

namespace game.models.roles.Templates.FolkRoles
{
    public class Detective : RoleTemplate, IInvestigativeAbility
    {
        public Detective() : base(RoleId.Detective, RoleCategory.FolkAnalyst,
            RolePriority.None, AbilityType.ActiveOthers, WinningTeam.Folk){
        }
        
        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService) {
            return ((IInvestigativeAbility) this).DetectiveAbility(roleOwner, choosenPlayer, gameService);

        }
        
        public override ChanceProperty GetChanceProperty() {
            return new ChanceProperty(25, ChanceProperty.NoMaxLimit);
        }
    }
}

