using game.models.gamestate;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.models.roles.Templates.NeutralRoles;

namespace SceneControllers.GameScene.Helper
{
    public class PlayerButtonVisibility
    {
        private bool IsLocalPlayer { get; }
        private bool IsPlayerAlive { get; }
        private Player Self { get; } 
        private Player Target { get;}
        private Time Time { get; }


        public PlayerButtonVisibility(Player self, Player target, Time time){
            Self = self;
            Target = target;
            Time = time;

            IsLocalPlayer = self.IsSamePlayer(target);
            IsPlayerAlive = self.DeathProperties.IsAlive;
        }


        public bool ShouldShowButton()
        {
            return Time switch
            {
                Time.Day => ShouldShowButtonDuringDay(),
                Time.Voting => ShouldShowButtonDuringVoting(),
                Time.Night => ShouldShowButtonDuringNight(),
                _ => false
            };
        }


        private bool ShouldShowButtonDuringDay(){
            return false;
        }

        private bool ShouldShowButtonDuringVoting(){
            return IsPlayerAlive && !IsLocalPlayer;
        }

        private bool ShouldShowButtonDuringNight(){
            bool abilityVis = CanUseAbilityOnTarget();

            return ApplySpecialRoleRules(abilityVis);
        }

        private bool CanUseAbilityOnTarget(){

            if(!IsPlayerAlive || !Self.Role.Template.RoleProperties.CanUseAbility){
                return false;
            }

            return Self.Role.Template.AbilityType.CanUseAbility(Self, Target);

        }

        private bool ApplySpecialRoleRules(bool previous){
            bool visible = previous;
            if (Self.Role.Template.RoleID == RoleId.LoreKeeper) {
                var lorekeeper = (LoreKeeper) Self.Role.Template;
                if (lorekeeper.AlreadyChosenPlayers.Contains(Target))
                    visible = false;

            }

            else if (Self.Role.Template.RoleID == RoleId.LastJoke) {
                RoleTemplate lastJoke = Self.Role.Template;
                if(Self.DeathProperties.IsAlive) visible = false;
                else if(lastJoke.RoleProperties.AbilityUsesLeft.Current > 0 )
                    visible = lastJoke.AbilityType.CanUseAbility(Self, Target);
            }
            
            return visible;
        }
    }
}