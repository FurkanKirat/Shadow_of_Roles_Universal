using System;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Networking.DataTransferObjects.RoleViewStrategies;
using Newtonsoft.Json;

namespace Networking.DataTransferObjects
{
    [Serializable]
    public class PlayerDto : IPlayer
    {
        public string Name { get; }
        public int Number { get; }
        public DeathProperties DeathProperties { get; }
        public RoleDto RoleDto { get; set; }
        public WinStatus WinStatus { get; }
        
        public PlayerDto(Player player, RoleInfoLevel roleInfoLevel, int playerCount)
        {
            Name = player.Name;
            Number = player.Number;
            DeathProperties = player.DeathProperties;
            RoleDto = RoleDataDtoBuilder.Build(player.Role, roleInfoLevel, playerCount );
            WinStatus = player.WinStatus;
        }
        
        [JsonConstructor]
        public PlayerDto(string name, int number, DeathProperties deathProperties, RoleDto roleDto, WinStatus winStatus)
        {
            Name = name;
            Number = number;
            DeathProperties = deathProperties;
            RoleDto = roleDto;
            WinStatus = winStatus;
        }
        
        public override string ToString()
        {
            return
                $"{nameof(Name)}: {Name}, {nameof(Number)}: {Number}, {nameof(DeathProperties)}: {DeathProperties}, {nameof(RoleDto)}: {RoleDto}, {nameof(WinStatus)}: {WinStatus}";
        }
    }
}