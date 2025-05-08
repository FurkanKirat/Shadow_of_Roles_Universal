using System.Timers;
using game.models.gamestate;
using game.Services.GameServices;

namespace Game.Services
{
    public class TurnTimerService
    {
        private Timer _timer;
        private readonly OnlineGameService _onlineDeviceGameService;
        private bool _isRunning = true;
        private readonly IOnTimeChangeListener _onTimeChangeListener;

        public TurnTimerService(OnlineGameService onlineDeviceGameService, IOnTimeChangeListener onTimeChangeListener)
        {
            _onlineDeviceGameService = onlineDeviceGameService;
            _onTimeChangeListener = onTimeChangeListener;
            SchedulePhase();
        }

        private void SchedulePhase()
        {
            if (!_isRunning) return;

            int delay = GetCurrentPhaseDuration();

            _timer = new Timer(delay);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isRunning) return;

            _onlineDeviceGameService.ToggleDayNightCycle();

            _onTimeChangeListener?.OnTimeChange();

            UpdatePhase();
        }

        private int GetCurrentPhaseDuration()
        {
            const int delay = 500;
            return _onlineDeviceGameService.TimeService.TimePeriod.Time switch
            {
                Time.Day => TimeManager.DayTime + delay,
                Time.Voting => TimeManager.VotingTime + delay,
                Time.Night => TimeManager.NightTime + delay,
                _ => 20000 + delay
            };
        }

        private void UpdatePhase()
        {
            SchedulePhase();
        }

        public void StopTimer()
        {
            _isRunning = false;
            _timer.Stop();
            _timer.Dispose();
        }

        public interface IOnTimeChangeListener
        {
            void OnTimeChange();
        }
    }
}
