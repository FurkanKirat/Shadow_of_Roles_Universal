using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.Templates.NeutralRoles
{
    public class Clown : RoleTemplate, INoAbility
    {
        public Clown() : base(RoleId.Clown, RoleCategory.NeutralChaos, 
            RolePriority.None, AbilityType.NoAbility, WinningTeam.Clown)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.CanWinWithAnyTeam)
                .AddAttribute(RoleAttribute.HasOtherWinCondition)
                .AddAttribute(RoleAttribute.WinsAlone)
                .AddAttribute(RoleAttribute.MustDieToWin);
            
            ChanceProperty = ChancePropertyFactory.Unique(30);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility)this).PerformAbilityForNoAbilityRoles();
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((INoAbility)this).DoNothing();
        }
    }
}