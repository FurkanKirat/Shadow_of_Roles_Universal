using game.models.player;
using game.models.roles.properties;

namespace SceneControllers.GameScene.Helper
{
    public class PlayerRoleVisibility
    {
        private Player _self;
        private Player _target;

        public PlayerRoleVisibility(Player self, Player target)
        {
            _self = self;
            _target = target;
        }

        public bool ShouldShowRole()
        {
            bool isRevealed = _target.Role.IsRevealed;
            bool sameTeamKnowsMembers = SameTeamAndShouldShowRole();

            bool isPlayerCurrentPlayer = _self.IsSamePlayer(_target);
            
            return isRevealed || sameTeamKnowsMembers || isPlayerCurrentPlayer;
        }

        private bool SameTeamAndShouldShowRole()
        {
            bool knowsTeamMembers = _self.Role.Template.RoleProperties.HasAttribute(RoleAttribute.KnowsTeamMembers);
            bool sameTeam = _self.Role.Template.WinningTeam == _target.Role.Template.WinningTeam;
            
            return knowsTeamMembers && sameTeam;
        }
    }
}