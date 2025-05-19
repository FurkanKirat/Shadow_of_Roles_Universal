using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces;
using game.models.roles.properties;
using game.models.roles.Templates.FolkRoles;
using game.models.roles.Templates.NeutralRoles;
using game.Services;
using game.Services.GameServices;
using game.Utils;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class Disguiser : RoleTemplate, IPriorityChangingRole
    {
        private RoleTemplate _currentRole;
        public Disguiser() : base(RoleId.Disguiser, RoleCategory.CorrupterSupport,
            RolePriority.None, AbilityType.ActiveAll, WinningTeam.Corrupter)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.KnowsTeamMembers)
                .AddAttribute(RoleAttribute.HasDisguiseAbility)
                .SetAttack(1);
            
            ChanceProperty = ChancePropertyFactory.Unlimited(15);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            roleOwner.Role.Template.RoleProperties.Attack.Current = _currentRole.RoleProperties.Attack.Default;
            return _currentRole.ExecuteAbility(roleOwner, choosenPlayer, gameService);
        }

        private void SetRandomRole(RolePack rolePack){
            var possibleRoles = new List<RoleTemplate>(RoleCatalog.GetAllRoles(rolePack));
            possibleRoles.Remove(new Disguiser());
            possibleRoles.Remove(new Entrepreneur());
            possibleRoles.Remove(new ChillGuy());
            possibleRoles.Remove(new LoreKeeper());
            possibleRoles.Remove(new Clown());
            possibleRoles.Remove(new LastJoke());

            _currentRole = possibleRoles.GetRandomElement().Copy();

            RolePriority = _currentRole.RolePriority;

            if (_currentRole.RoleProperties.HasAttribute(RoleAttribute.RoleBlockImmune)) {
                RoleProperties.AddAttribute(RoleAttribute.RoleBlockImmune);
            } else {
                RoleProperties.RemoveAttribute(RoleAttribute.RoleBlockImmune);
            }

        }
        
        public void ChangePriority(RolePack rolePack)
        {
            SetRandomRole(rolePack);
        }
    }
}