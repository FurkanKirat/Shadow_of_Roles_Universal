using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.FolkRoles
{
    public class ChillGuy : RoleTemplate, INoAbility
    {
        public ChillGuy() : base(RoleId.ChillGuy, RoleCategory.FolkVillager, 
            RolePriority.None, AbilityType.NoAbility, WinningTeam.Folk)
        {
 
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility)this).PerformAbilityForNoAbilityRoles();
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((INoAbility)this).DoNothing();
        }

        public override ChanceProperty GetChanceProperty()
        {
            return ChancePropertyFactory.Unlimited(10);
        }
    }
}