
using System;

namespace game.models.gamestate
{
    public class GameEndResult {
        private readonly bool _gameFinished;
        private readonly GameEndReason _reason;
        private readonly WinStatus _winStatus;

        // Constructor
        public GameEndResult(bool gameFinished, GameEndReason reason, WinStatus winStatus) {
            _gameFinished = gameFinished;
            _reason = reason;
            _winStatus = winStatus;
        }

        // Property definitions
        public bool GameFinished => _gameFinished;

        public GameEndReason Reason => _reason;

        public WinStatus WinStatus => _winStatus;

        protected bool Equals(GameEndResult other)
        {
            return _gameFinished == other._gameFinished && _reason == other._reason && _winStatus == other._winStatus;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GameEndResult)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_gameFinished, (int)_reason, (int)_winStatus);
        }

        public static bool operator ==(GameEndResult left, GameEndResult right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
            return left.Equals(right);
        }
        
        public static bool operator !=(GameEndResult a, GameEndResult b)
        {
            return !(a == b);
        }
    }

    // Enum definitions
    public enum GameEndReason {
        SinglePlayerRemains,
        NoPlayersAlive,
        OnlyTwoPlayersCannotKillEachOther,
        OnlyTwoCanWinTogether,
        AllSameTeam,
        NoKillsInMultipleNights,
        None
    }
    
    public enum WinStatus {
        Won,
        Lost,
        Tied,
        Unknown
    }
}