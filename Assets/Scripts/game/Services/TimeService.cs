using game.models.gamestate;
using game.Services.GameServices;
using UnityEngine;

namespace game.Services
{
    public class TimeService
    {
        private readonly BaseGameService _gameService;
        public TimePeriod TimePeriod { get;} = TimePeriod.Start();

        public TimeService(BaseGameService gameService)
        {
            _gameService = gameService;
        }

        public void ToggleTimeCycle()
        {
            Debug.Log("Before: " + TimePeriod );
            TimePeriod.Next(_gameService.GameSettings.GameMode);
            Debug.Log("After: " + TimePeriod );
        }
        
    }
}