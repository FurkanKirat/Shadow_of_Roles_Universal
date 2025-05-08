using System;
using System.Collections.Generic;
using System.Linq;
using Networking.DataTransferObjects.RoleViewStrategies;

namespace Networking.DataTransferObjects
{
    [Serializable]
    public class ClientInfoDto
    {
        public int Number {get;set;}
        public int TargetNumber {get;set;}
        public Dictionary<ExtraData, object> ExtraData { get; set; } = new();

        public override string ToString()
        {
            string extraDataString = ExtraData != null ? string.Join(", ", ExtraData.Select(kv => $"{kv.Key}: {kv.Value}")) : "No Extra Data";

            return $"{nameof(Number)}: {Number}, {nameof(TargetNumber)}: {TargetNumber}, {nameof(ExtraData)}: {extraDataString}";
        }
    }
}