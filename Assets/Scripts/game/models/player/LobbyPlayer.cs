using System;
using game.models.player.properties;

namespace game.models.player
{
    public class LobbyPlayer : IEquatable<LobbyPlayer>
    {
        public string Name { get; }
        public bool IsHost { get; }
        public bool IsAI { get; }
        public int Id { get; }
        public LobbyPlayerStatus LobbyPlayerStatus { get; set; }

        public LobbyPlayer(string name, bool isHost, bool isAI, int id, LobbyPlayerStatus lobbyPlayerStatus)
        {
            Name = name;
            IsHost = isHost;
            IsAI = isAI;
            Id = id;
            LobbyPlayerStatus = lobbyPlayerStatus;
        }
        
        public bool Equals(LobbyPlayer other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as LobbyPlayer);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
        
        public static bool operator ==(LobbyPlayer player1, LobbyPlayer player2)
        {
            if (ReferenceEquals(player1, player2)) return true;
            if (player1 is null || player2 is null) return false;
            return player1.Equals(player2);
        }
        
        public static bool operator !=(LobbyPlayer player1, LobbyPlayer player2)
        {
            return !(player1 == player2);
        }
    }
}