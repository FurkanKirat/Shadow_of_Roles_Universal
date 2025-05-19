using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
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
            
            ChanceProperty = ChancePropertyFactory.Unlimited(25);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return((IPerformAbility) this).PerformAbilityForBlockImmuneRoles(roleOwner,choosenPlayer,gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IRoleBlockAbility) this).RoleBlock(choosenPlayer);
        }
    }
}