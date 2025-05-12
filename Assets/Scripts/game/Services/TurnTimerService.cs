using System;
using System.Collections.Generic;
using System.Timers;
using game.Services.GameServices;
using Managers;
using Networking.Interfaces;
using Networking.Server;
using UnityEngine;

namespace Game.Services
{
    public class TurnTimerService : MonoBehaviour
    {
        private BaseGameService _onlineGameService;
        private OnlineServer onlineServer;
        private Timer _timer;
        private bool _isRunning = true;

        private static readonly Queue<Action> MainThreadActions = new();

        public static void RunOnMainThread(Action action)
        {
            lock (MainThreadActions)
            {
                MainThreadActions.Enqueue(action);
            }
        }

        private void Update()
        {
            lock (MainThreadActions)
            {
                while (MainThreadActions.Count > 0)
                {
                    MainThreadActions.Dequeue().Invoke();
                }
            }
        }

        public void Initialize(OnlineGameService onlineGameService)
        {
            _onlineGameService = onlineGameService;
            onlineServer = (OnlineServer)ServiceLocator.Get<IServer>();
            SchedulePhase();
        }

        private void SchedulePhase()
        {
            if (!_isRunning) return;

            Debug.Log("Timer scheduled");
            int phaseTime = _onlineGameService.GameSettings.PhaseTime * 1000;
            _timer = new Timer(phaseTime);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isRunning) return;
            
            RunOnMainThread(() =>
            {
                Debug.Log("Timer elapsed");
                _onlineGameService.ToggleDayNightCycle();
                onlineServer.SendGameState();
                SchedulePhase();
            });
        }

        public void StopTimer()
        {
            _isRunning = false;
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
    
}
