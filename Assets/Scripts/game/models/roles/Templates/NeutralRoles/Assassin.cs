using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.NeutralRoles
{
    public class Assassin : RoleTemplate, IAttackAbility
    {
        public Assassin() : base(RoleId.Assassin, RoleCategory.NeutralKilling, 
            RolePriority.None, AbilityType.ActiveOthers, WinningTeam.Assassin)
        {
            RoleProperties
                .SetAttack(1)
                .SetDefence(1)
                .AddAttribute(RoleAttribute.HasAttackAbility)
                .AddAttribute(RoleAttribute.WinsAlone)
                .AddAttribute(RoleAttribute.MustBeLastStanding)
                .AddAttribute(RoleAttribute.MustSurviveUntilEnd)
                .AddAttribute(RoleAttribute.RoleBlockImmune)
                .AddAttribute(RoleAttribute.CanKill1V1);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility) this).PerformAbilityForBlockImmuneRoles(roleOwner, choosenPlayer, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IAttackAbility)this).Attack(roleOwner, choosenPlayer, gameService, CauseOfDeath.Assassin);
        }

        public override ChanceProperty GetChanceProperty()
        {
            return ChancePropertyFactory.Unique(40);
        }
    }
}