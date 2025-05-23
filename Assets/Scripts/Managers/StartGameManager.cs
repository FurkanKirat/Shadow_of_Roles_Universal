﻿using System;
using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using game.models.roles;
using game.Services.GameServices;
using game.Services.RoleDistributor;
using game.Services.RoleDistributor.GameRolesStrategy;
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
            if (roles.Count != playerCount)
            {
                Debug.Log("Player count mismatch");
                return null;
            }
            for(var i=0; i < playerCount ; ++i){
                players[i].Role = new Role(roles[i]);
            }

            return gameSettings.GameMode switch
            {
                GameMode.Local => new LocalGameService(players, ruleSet),
                GameMode.Online => new OnlineGameService(players, ruleSet),
                _ => throw new NotImplementedException(),
            };
        }
        
    }
}