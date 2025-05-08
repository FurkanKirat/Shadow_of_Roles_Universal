using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using Networking.DataTransferObjects;

namespace game.models
{
    public interface IGameInformation
    {
        Dictionary<TimePeriod, List<Message>> Messages { get; }
        TimePeriod LastMessagePeriod { get; }
        List<PlayerDto> AlivePlayers { get; }
        List<PlayerDto> DeadPlayers { get; }
        List<PlayerDto> AllPlayers { get; }
        TimePeriod TimePeriod { get; }
        PlayerDto CurrentPlayer { get; }
        GameSettings GameSettings { get; }
        GameStatus GameStatus { get; }
        WinningTeam WinnerTeam { get; }
    }
}