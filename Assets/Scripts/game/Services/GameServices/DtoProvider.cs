using System.Linq;
using game.models.gamestate;
using Networking.DataTransferObjects;

namespace game.Services.GameServices
{
    public class DtoProvider 
    {
        private readonly BaseGameService _gameService;

        public DtoProvider(BaseGameService gameService)
        {
            _gameService = gameService;
        }

        public GameDto GetGameInformationFor(int playerNumber)
        {
            var currentPlayer = _gameService.GetPlayer(playerNumber);
            var playerCount = _gameService.AllPlayers.Count;

            var dto = new GameDto
            {
                CurrentPlayer = new PlayerDto(currentPlayer, RoleInfoLevel.SelfInfo, playerCount),
                AlivePlayers = _gameService.AlivePlayers.Select(pn =>
                {
                    var p = _gameService.GetPlayer(pn);
                    var level = pn == playerNumber ? RoleInfoLevel.SelfInfo : RoleInfoLevel.OthersInfo;
                    return new PlayerDto(p, level, playerCount);
                }).ToList(),
                DeadPlayers = _gameService.GetDeadPlayers().Select(p => new PlayerDto(p, RoleInfoLevel.RoleRevealedInfo, playerCount)).ToList(),
                AllPlayers = _gameService.AllPlayers.Values.Select(p =>
                    new PlayerDto(p, _gameService.FinishGameService.IsGameFinished ? RoleInfoLevel.RoleRevealedInfo : RoleInfoLevel.OthersInfo, playerCount)
                ).ToList(),
                TimePeriod = _gameService.TimeService.TimePeriod,
                GameSettings = _gameService.GameSettings,
                GameStatus = _gameService.FinishGameService.IsGameFinished ? GameStatus.Ended : GameStatus.Continues,
                WinnerTeam = _gameService.FinishGameService.GetHighestPriorityWinningTeam(),
                Messages = _gameService.MessageService.GetPlayerMessages(currentPlayer),
                LastMessagePeriod = _gameService.MessageService.GetLastMessagePeriod(currentPlayer)
            };

            return dto;
        }
    }

}
