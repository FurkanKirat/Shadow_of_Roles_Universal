using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using Game.Services;

namespace game.Services.GameServices
{
    public class OnlineGameService : BaseGameService
    {
        public TurnTimerService TurnTimerService { get; }

        public OnlineGameService(List<Player> players, TurnTimerService.IOnTimeChangeListener onTimeChangeListener, RolePack rolePack)
        : base(players, new TimeService(), new GameSettings(GameMode.Online, rolePack, players.Count))
        {
            TurnTimerService = new TurnTimerService(this, onTimeChangeListener);
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