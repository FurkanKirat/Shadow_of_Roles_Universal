using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services;
using game.Services.GameServices;

namespace game.models.roles.Templates.FolkRoles
{
    public class SealMaster : RoleTemplate, IRoleBlockAbility
    {
        public SealMaster() : base(RoleId.SealMaster, RoleCategory.FolkSupport, 
            RolePriority.RoleBlock, AbilityType.ActiveOthers, WinningTeam.Folk)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.CanRoleBlock)
                .AddAttribute(RoleAttribute.RoleBlockImmune);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return((IPerformAbility) this).PerformAbilityForBlockImmuneRoles(roleOwner,choosenPlayer,gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IRoleBlockAbility) this).RoleBlock(choosenPlayer);
        }

        public override ChanceProperty GetChanceProperty()
        {
            return new ChanceProperty(25, ChanceProperty.NoMaxLimit);
        }
    }
}