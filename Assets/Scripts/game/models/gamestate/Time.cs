using System;

namespace game.models.gamestate
{
    public enum Time
    {
        Day = 0,
        Voting = 1,
        Night = 2
    }

    public static class TimeExtensions
    {
        public static Time Next(this Time time)
        {
            return (Time) (((int)time+1) % Enum.GetValues(typeof(Time)).Length);
        }
        
        public static Time Next(this Time current, GameMode gameMode)
        {
            var cycle = gameMode == GameMode.Online ? OnlineCycle : OfflineCycle;
            int index = Array.IndexOf(cycle, current);
            return cycle[(index + 1) % cycle.Length];
        }

        public static Time Previous(this Time time)
        {
            int size = Enum.GetValues(typeof(Time)).Length;
            return (Time) (((int)time - 1 + size) % size);
        }
        
        public static Time Previous(this Time current, GameMode gameMode)
        {
            var cycle = gameMode == GameMode.Online ? OnlineCycle : OfflineCycle;
            int index = Array.IndexOf(cycle, current);
            return cycle[(index - 1 + + cycle.Length) % cycle.Length];
        }

        private static readonly Time[] OfflineCycle = { Time.Day, Time.Voting, Time.Night };
        private static readonly Time[] OnlineCycle = { Time.Voting, Time.Night };
    }
    
}