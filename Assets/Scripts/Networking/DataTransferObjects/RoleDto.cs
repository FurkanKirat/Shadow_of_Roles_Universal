using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models.Roles.Enums;
using Networking.DataTransferObjects.RoleViewStrategies;

namespace Networking.DataTransferObjects
{
    [Serializable]
    public class RoleDto
    {
        public RoleId RoleId { get; set; }
        public int Money { get; set; }
        public int AbilityUsesLeft { get; set; }
        public int Cooldown { get; set; }
        public bool IsRevealed { get; set; }

        public Dictionary<ExtraData, object> ExtraData { get; set; } = new();

        public bool CanUseAbility()
        {
            return AbilityUsesLeft !=0 && Cooldown == 0;
        }
        
        public override string ToString()
        {
            string extraDataString = ExtraData != null
                ? string.Join(", ", ExtraData.Select(kvp => $"{kvp.Key}: {kvp.Value}"))
                : "null";

            return
                $"{nameof(RoleId)}: {RoleId}, " +
                $"{nameof(Money)}: {Money}, " +
                $"{nameof(AbilityUsesLeft)}: {AbilityUsesLeft}, " +
                $"{nameof(Cooldown)}: {Cooldown}, " +
                $"{nameof(IsRevealed)}: {IsRevealed}, " +
                $"{nameof(ExtraData)}: {{{extraDataString}}}";
        }

    }
}