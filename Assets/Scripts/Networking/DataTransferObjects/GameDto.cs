using System;
using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using Game.Models.Roles.Enums;

namespace Networking.DataTransferObjects
{
    [Serializable]
    public class GameDto : IGameInformation
    {
        public PlayerDto CurrentPlayer {get; set;}
        public List<PlayerDto> AlivePlayers {get; set;}
        public List<PlayerDto> DeadPlayers {get; set;}
        public List<PlayerDto> AllPlayers {get; set;}
        public TimePeriod TimePeriod {get; set;}
        public GameSettings GameSettings {get; set;}
        public GameStatus GameStatus {get; set;}
        public WinningTeam WinnerTeam {get; set;}
        public List<Message> Messages {get; set;}
        public TimePeriod LastMessagePeriod {get; set;}
        
    }
}