using System;
using System.Linq;

namespace game.models.gamestate
{
    public enum Time
    {
        None = -1,
        Day = 0,
        Voting = 1,
        Night = 2
    }

    public static class TimeExtensions
    {
        public static Time Next(this Time current, GameMode gameMode)
        {
            var cycle = gameMode == GameMode.Online ? OnlineCycle : OfflineCycle;
            int index = Array.IndexOf(cycle, current);
            return cycle[(index + 1) % cycle.Length];
        }
        
        public static Time Previous(this Time current, GameMode gameMode)
        {
            var cycle = gameMode == GameMode.Online ? OnlineCycle : OfflineCycle;
            int index = Array.IndexOf(cycle, current);
            return cycle[(index - 1 + cycle.Length) % cycle.Length];
        }

        public static Time GetFirst(GameMode gameMode)
        {
            var cycle = gameMode == GameMode.Online ? OnlineCycle : OfflineCycle;
            return cycle.First();
        }

        public static Time GetLast(GameMode gameMode)
        {
            var cycle = gameMode == GameMode.Online ? OnlineCycle : OfflineCycle;
            return cycle.Last();
        }

        private static readonly Time[] OfflineCycle = { Time.Day, Time.Voting, Time.Night };
        private static readonly Time[] OnlineCycle = { Time.Voting, Time.Night };
    }
    
}