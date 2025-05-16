using System.Collections.Generic;
using game.models.gamestate;
using game.Utils;
using Managers;

namespace game.models
{
    public class Message
    {
        public MessageTemplate Template { get; }
        public TimePeriod TimePeriod { get; }
        public int ReceiverNumber { get; }
        public bool IsPublic { get; }
        
        public Message(MessageTemplate messageTemplate, TimePeriod timePeriod, int receiverNumber, bool isPublic){
            Template = messageTemplate;
            TimePeriod = timePeriod;
            ReceiverNumber = receiverNumber;
            IsPublic = isPublic;
        }

        public string GetText()
        {
            string text = TextManager.Translate(Template.MessageKey);
            var translatedPlaceHolders = new Dictionary<string, string>();
            if (Template.PlaceHolders != null)
            {
                foreach (var placeholder in Template.PlaceHolders)
                {
                    string value = placeholder.Value;
                
                    if (placeholder.Key.EndsWith("Id"))
                    {
                        value = TextManager.Translate($"{placeholder.Value}.name");
                    }
                    translatedPlaceHolders[placeholder.Key] = value;
                }
            }
            
            return StringFormatter.ReplacePlaceholders(text, translatedPlaceHolders);
        }
        
    }
}