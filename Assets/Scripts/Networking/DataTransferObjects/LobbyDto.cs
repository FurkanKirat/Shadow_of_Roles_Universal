using System;
using System.Collections.Generic;
using System.Linq;
using game.models.player;

namespace Networking.DataTransferObjects
{
    [Serializable]
    public class LobbyDto
    {
        public List<LobbyPlayer> Players { get; }
        public int Id { get; }

        // Constructor
        public LobbyDto(List<LobbyPlayer> players, int id)
        {
            Players = players ?? throw new ArgumentNullException(nameof(players));
            Id = id;
        }

        // Method to get player by id
        public LobbyPlayer GetPlayer() => Players.FirstOrDefault(player => player.Id == Id);
    }
}