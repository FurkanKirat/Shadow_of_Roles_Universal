using System;
using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.player;
using game.models.roles;
using game.Services.GameServices;
using game.Services.RoleDistributor;
using game.Services.RoleDistributor.GameRolesStrategy;

namespace Managers
{
    public class StartGameManager
    {
        public IDataProvider GameService { get; private set; }
        
        public void InitializeGameService(List<Player> players, GameSettings gameSettings){
            var ruleSet = gameSettings.RolePack;
            var playerCount = players.Count;
            
            var hints = StrategyChooser.GetStrategy(ruleSet);
            
            var roleDistributor = new RoleDistributor(hints.Build(gameSettings), gameSettings);
            var roles = roleDistributor.DistributeRoles();
            for(var i=0; i < playerCount ; ++i){
                players[i].Role = new Role(roles[i]);
            }
            GameService = new SingleDeviceGameService(players, ruleSet);

        }
        
    }
}