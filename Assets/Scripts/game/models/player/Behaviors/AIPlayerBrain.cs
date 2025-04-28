using System.Collections.Generic;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces;
using game.models.roles.properties;
using game.models.roles.Templates;
using game.Utils;

namespace game.models.player.Behaviors
{
    public class AIPlayerBrain : IPlayerBrain
    {
        public void ChooseNightPlayer(Player self, List<Player> players)
        {
            List<Player> choosablePlayers = new List<Player>(players);
            choosablePlayers = self.Role.Template.FilterChoosablePlayers(self, choosablePlayers);
            
            ChooseRoleSpecificValues(self,choosablePlayers);
            
            if(choosablePlayers.Count == 0){
                return;
            }

            self.Role.ChosenPlayer = choosablePlayers.GetRandomElement();
        }

        public void ChooseVotingPlayer(Player self, List<Player> players)
        {
            List<Player> choosablePlayers = new List<Player>(players);
            choosablePlayers.Remove(self);
            
            var selfRole = self.Role.Template;
            var team = selfRole.WinningTeam;
            
            bool knowsTeamMembers = selfRole.RoleProperties.HasAttribute(RoleAttribute.KnowsTeamMembers);
            bool winsAlone = selfRole.RoleProperties.HasAttribute(RoleAttribute.WinsAlone);
            
            if(knowsTeamMembers){
                choosablePlayers.RemoveAll(player => player.Role.Template.WinningTeam == team);
            }
            else if (!winsAlone) {
                
                foreach (var player in players) {
                    
                    if(!player.Role.IsRevealed) continue;
                    
                    var playerTeam = player.Role.Template.WinningTeam;
                    
                    if (!playerTeam.CanWinWith(team)) {
                        self.Role.ChosenPlayer = player;
                        return;
                    } 
                    
                    if (playerTeam == team)
                        choosablePlayers.Remove(player);
                    
                }
            }

            if(choosablePlayers.Count == 0){
                return;
            }
            self.Role.ChosenPlayer = choosablePlayers.GetRandomElement();
        }
        

        private void ChooseRoleSpecificValues(Player self,List<Player> choosablePlayers) {
            var roleAIBehavior = self.Role.Template as IRoleAIBehavior;
            roleAIBehavior?.ChooseRoleSpecificValues(choosablePlayers);
        }
    }
}