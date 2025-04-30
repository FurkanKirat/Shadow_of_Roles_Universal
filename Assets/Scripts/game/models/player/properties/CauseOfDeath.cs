using System;

namespace game.models.player.properties
{
    [Flags]
    public enum CauseOfDeath
    {
        None = 0,
        Hanging = 1 << 0,
        Entrepreneur = 1 << 1,
        Psycho = 1 << 2,
        Assassin = 1 << 3,
        LastJoke = 1 << 4
    }
}

