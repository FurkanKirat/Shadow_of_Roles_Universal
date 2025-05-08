using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.models.roles.Templates;
using game.Services;
using Networking.DataTransferObjects;

namespace SceneControllers.GameScene.Helper
{
    public class PlayerRoleVisibility
    {
        private readonly PlayerDto _self, _target;

        public PlayerRoleVisibility(PlayerDto self, PlayerDto target)
        {
            _self = self;
            _target = target;
        }

        public bool ShouldShowRole()
        {
            if(_target.RoleDto.RoleId == RoleId.None) return false;
            
            bool sameTeamKnowsMembers = SameTeamAndShouldShowRole();

            bool isPlayerCurrentPlayer = _self.IsSamePlayer(_target);

            bool targetRevealed = _target.RoleDto.IsRevealed;
            return targetRevealed || sameTeamKnowsMembers || isPlayerCurrentPlayer;
        }

        private bool SameTeamAndShouldShowRole()
        {
            RoleTemplate selfRole = RoleCatalog.GetRole(_self.RoleDto.RoleId);
            RoleTemplate targetRole = RoleCatalog.GetRole(_target.RoleDto.RoleId);
            
            bool knowsTeamMembers = selfRole.RoleProperties.HasAttribute(RoleAttribute.KnowsTeamMembers);
            bool sameTeam = selfRole.WinningTeam == targetRole.WinningTeam;
            
            return knowsTeamMembers && sameTeam;
        }
    }
}