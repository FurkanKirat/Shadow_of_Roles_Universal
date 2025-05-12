using System;
using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using UnityEngine;
using Time = game.models.gamestate.Time;

namespace game.Services.GameServices
{
    public class OnlineGameService : BaseGameService
    {

        public OnlineGameService(List<Player> players,  RolePack rolePack)
        : base(players, new GameSettings(GameMode.Online, rolePack, players.Count))
        {
            
        }
        
        public override void ToggleDayNightCycle()
        {
            Debug.Log("Toggle day night cycle");
            TimeService.ToggleTimeCycle();
            Time time = TimeService.TimePeriod.Time;
            switch (time)
            {
                case Time.Day:
                    // No day phase in online mode
                    break;
                case Time.Night:
                    VotingService.ExecuteMaxVoted();
                    break;
                case Time.Voting:
                    AbilityService.PerformAllAbilities();
                    break;
                default:
                    throw new InvalidOperationException("Unknown time phase.");
            }
            
            FinishGameService.FinishGameIfNeeded();
        }

        private readonly object _lock = new object();

        // public void UpdateAllPlayers(PlayerDto playerDto)
        // {
        //     lock (_lock)
        //     {
        //         Player player = GetPlayer(playerDto.SenderPlayerNumber);
        //
        //         if (player != null)
        //         {
        //             player.Role.ChosenPlayer = GetPlayer(playerDto.ChosenPlayerNumber).Number;
        //             player.Role.Template = playerDto.SenderRole;
        //         }
        //
        //         UpdateAlivePlayers();
        //     }
        // }
        
    }
}