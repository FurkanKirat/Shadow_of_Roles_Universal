
using System.Collections.Generic;
using game.models.DataTransferObjects;
using game.models.gamestate;
using game.models.player;
using Game.Models.Roles.Enums;
using Game.Services;

namespace game.Services.GameServices
{
    public class MultiDeviceGameService : BaseGameService
    {
        public TurnTimerService TurnTimerService {get;}

        public MultiDeviceGameService(List<Player> players, TurnTimerService.IOnTimeChangeListener onTimeChangeListener, RolePack rolePack)
        : base(players, new BaseTimeService(), new GameSettings(GameMode.Online, rolePack, players.Count))
        {
            TurnTimerService = new TurnTimerService(this, onTimeChangeListener);
        }

        private readonly object _lock = new object();

        public void UpdateAllPlayers(PlayerDto playerDto)
        {
            lock (_lock)
            {
                Player player = FindPlayer(playerDto.SenderPlayerNumber);

                if (player != null)
                {
                    player.Role.ChosenPlayer = FindPlayer(playerDto.ChosenPlayerNumber);
                    player.Role.Template = playerDto.SenderRole;
                }

                UpdateAlivePlayers();
            }
        }


        public Player FindPlayer(int number)
        {
            foreach (var player in AllPlayers)
            {
                if (number == player.Number)
                {
                    return player;
                }
            }
            return null;
        }
        
        
    }
}