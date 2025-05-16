using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class Interrupter : RoleTemplate, IRoleBlockAbility
    {
        public Interrupter() : base(RoleId.Interrupter, RoleCategory.CorrupterSupport, 
            RolePriority.RoleBlock, AbilityType.OtherThanTeamMembers, WinningTeam.Corrupter)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.KnowsTeamMembers)
                .AddAttribute(RoleAttribute.CanRoleBlock)
                .AddAttribute(RoleAttribute.RoleBlockImmune);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility) this).PerformAbilityForBlockImmuneRoles(roleOwner, choosenPlayer, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IRoleBlockAbility) this).RoleBlock(choosenPlayer);
        }

        public override ChanceProperty GetChanceProperty()
        {
            return ChancePropertyFactory.Unlimited(30);
        }
    }
}