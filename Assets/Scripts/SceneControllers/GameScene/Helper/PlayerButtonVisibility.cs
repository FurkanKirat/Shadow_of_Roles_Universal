using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Services;
using Networking.DataTransferObjects;
using Networking.DataTransferObjects.RoleViewStrategies;

namespace SceneControllers.GameScene.Helper
{
    public class PlayerButtonVisibility
    {
        private readonly bool _isPlayerAlive, _isLocalPlayer;
        private readonly PlayerDto _target, _self;
        private readonly Time _time;
        private readonly RoleTemplate _selfRole;
        
        public PlayerButtonVisibility(PlayerDto self, PlayerDto target, Time time){
            _self = self;
            _target = target;
            _time = time;

            _isLocalPlayer = self.IsSamePlayer(target);
            _isPlayerAlive = self.DeathProperties.IsAlive;

            _selfRole = RoleCatalog.GetRole(self.RoleDto.RoleId);
        }
        
        public bool ShouldShowButton()
        {
            return _time switch
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
            return _isPlayerAlive && !_isLocalPlayer;
        }

        private bool ShouldShowButtonDuringNight(){
            bool abilityVis = CanUseAbilityOnTarget();

            return ApplySpecialRoleRules(abilityVis);
        }

        private bool CanUseAbilityOnTarget(){

            if(!_isPlayerAlive || !_self.RoleDto.CanUseAbility()){
                return false;
            }

            return _selfRole.AbilityType.CanUseAbility(_self, _target);

        }

        private bool ApplySpecialRoleRules(bool previous){
            bool visible = previous;
            if (_selfRole.RoleID == RoleId.LoreKeeper) {
                var alreadyChosenPlayers = (HashSet<int>)_self.RoleDto.ExtraData[ExtraData.LoreKeeperAlreadyChosenPlayers];
                if (alreadyChosenPlayers.Contains(_target.Number))
                    visible = false;

            }

            else if (_selfRole.RoleID == RoleId.LastJoke) {
                if(_self.DeathProperties.IsAlive) visible = false;
                else if(_self.RoleDto.AbilityUsesLeft > 0 )
                    visible = RoleCatalog.GetRole(RoleId.LastJoke).AbilityType.CanUseAbility(_self, _target);
            }
            
            return visible;
        }
    }
}