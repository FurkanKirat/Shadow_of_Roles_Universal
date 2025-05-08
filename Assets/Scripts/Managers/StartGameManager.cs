using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.player;
using game.models.roles;
using game.Services.GameServices;
using game.Services.RoleDistributor;
using game.Services.RoleDistributor.GameRolesStrategy;
using game.Utils;
using UnityEngine;

namespace Managers
{
    public static class StartGameManager
    {
        
        public static BaseGameService InitializeGameService(List<Player> players, GameSettings gameSettings){
            var ruleSet = gameSettings.RolePack;
            var playerCount = players.Count;
            
            var hints = StrategyChooser.GetStrategy(ruleSet);
            var newSettings = new GameSettings(gameSettings.GameMode, gameSettings.RolePack, playerCount);
            var roleDistributor = new RoleDistributor(hints.Build(gameSettings), newSettings);
            
            var roles = roleDistributor.DistributeRoles();
            for(var i=0; i < playerCount ; ++i){
                players[i].Role = new Role(roles[i]);
            }
            
            var gameService = new LocalGameService(players, ruleSet);
            
            return gameService;
        }
        
    }
}