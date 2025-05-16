using System;
using game.models.gamestate;

namespace Networking.DataTransferObjects
{
    [Serializable]
    public class MessageDto
    {
        public TimePeriod TimePeriod { get; }
        public string Text { get; }
        public int ReceiverNumber { get; }
        public bool IsPublic { get; }
    }
}