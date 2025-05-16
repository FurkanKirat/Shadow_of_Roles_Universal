using System.Collections.Generic;

namespace game.models
{
    public class MessageTemplate
    {
        public string MessageKey { get; set; }
        public Dictionary<string, string> PlaceHolders { get; set; }

        public MessageTemplate(){}
    }
}