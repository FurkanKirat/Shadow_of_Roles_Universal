using System;
using System.Collections.Generic;

namespace Game.Models.Roles.Enums
{
    public enum WinningTeam
    {
        Unknown = 0,
        Folk = -2,
        Corrupter = -1,

        Assassin = 1,
        LoreKeeper = 2,
        Clown = 3,
    }

    public static class WinningTeamExtensions
    {
        // Mapping of WinningTeam to forbidden alliances
        private static readonly Dictionary<WinningTeam, HashSet<WinningTeam>> CannotWinWithMap = new Dictionary<WinningTeam, HashSet<WinningTeam>>()
        {
            { WinningTeam.Assassin, new HashSet<WinningTeam> { WinningTeam.Folk, WinningTeam.Corrupter } },
            { WinningTeam.Corrupter, new HashSet<WinningTeam> { WinningTeam.Assassin, WinningTeam.Folk } },
            { WinningTeam.Folk, new HashSet<WinningTeam> { WinningTeam.Corrupter, WinningTeam.Assassin } }
        };
        
        // Get the Team associated with the WinningTeam
        public static Team GetTeam(this WinningTeam team)
        {
            return team switch
            {
                WinningTeam.Folk => Team.Folk,
                WinningTeam.Corrupter => Team.Corrupter,
                WinningTeam.Assassin => Team.Neutral,
                WinningTeam.LoreKeeper => Team.Neutral,
                WinningTeam.Clown => Team.Neutral,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // Check if the WinningTeam can win with another WinningTeam
        public static bool CanWinWith(this WinningTeam team, WinningTeam other)
        {
            return !CannotWinWithMap.ContainsKey(team) || !CannotWinWithMap[team].Contains(other);
        }
    }
}
