using System;
using game.models.gamestate;
using game.models.player.Behaviors;
using game.models.player.properties;
using game.models.roles;
using UnityEngine;

namespace game.models.player
{
    public class Player : IEquatable<Player>
    {
        public int Number { get; }
        public string Name { get; }
        public DeathProperties DeathProperties { get; }
        public Role Role { get; set; }
        public WinStatus WinStatus { get; set; }
        public PlayerType PlayerType { get; set; }
        public IPlayerBrain Brain { get; private set; }

        private Player(int number, string name, PlayerType playerType, IPlayerBrain brain)
        {
            
            Number = number;
            Name = name;
            PlayerType = playerType;
            Brain = brain;
            
            DeathProperties = new DeathProperties();
            WinStatus = WinStatus.Lost;
        }

        public static class PlayerFactory
        {
            public static Player CreatePlayer(int number, string name, PlayerType type)
            {
                IPlayerBrain brain = type switch
                {
                    PlayerType.AI => new AIPlayerBrain(),
                    PlayerType.Human => new HumanPlayerBrain(),
                    _ => throw new ArgumentException("Unknown type")
                };
    
                return new Player(number, name, type, brain);
            }
        }
        
        public void KillPlayer(TimePeriod timePeriod, CauseOfDeath causeOfDeath )
        {
            DeathProperties.IsAlive = false;
            DeathProperties.DeathTimePeriod = timePeriod.GetPrevious();
            DeathProperties.AddCauseOfDeath(causeOfDeath);
        }
        
        public string GetNameAndNumber()
        {
            return $"({Number}) {Name}";
        }

        public bool Equals(Player other)
        {
            if (other == null) return false;
            return Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
        
        public static bool operator ==(Player player1, Player player2)
        {
            if (ReferenceEquals(player1, player2)) return true;
            if (player1 is null || player2 is null) return false;
            return player1.Equals(player2);
        }
        
        public static bool operator !=(Player player1, Player player2) => !(player1 == player2);

        public string GetNameAndRole()
        {
            return $"{GetNameAndNumber()} ({Role.Template.GetName()})";
        }

        public void SetWinStatus(WinStatus winStatus)
        {
            WinStatus = winStatus;
        }
        

        public bool IsSamePlayer(Player player)
        {
            return player.Number == Number;
        }

        public bool IsSamePlayer(int number)
        {
            return Number == number;
        }

        public override string ToString()
        {
            return $"Player{{number={Number}, name='{Name}', deathProperties={DeathProperties}, role={Role}, winStatus={WinStatus}, PlayerType={PlayerType}}}";
        }
    }
}
