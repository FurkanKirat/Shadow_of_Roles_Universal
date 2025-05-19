using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class Psycho : RoleTemplate, IAttackAbility
    {
        public Psycho() : base(RoleId.Psycho, RoleCategory.CorrupterKilling, 
            RolePriority.None, AbilityType.OtherThanTeamMembers, WinningTeam.Corrupter)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.KnowsTeamMembers)
                .AddAttribute(RoleAttribute.HasAttackAbility)
                .AddAttribute(RoleAttribute.CanKill1V1)
                .SetAttack(1);
            
            ChanceProperty = ChancePropertyFactory.Unique(100);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IAttackAbility)this).Attack(roleOwner, choosenPlayer, gameService, CauseOfDeath.Psycho);
        }
    }
}