using game.models.gamestate;
using game.models.player;

namespace game.models
{
    public class Message
    {
        
        public TimePeriod TimePeriod { get; }

        public string Text { get; }

        public int ReceiverNumber { get; }

        public bool IsPublic { get; }
        public Message(TimePeriod timePeriod, string text, int receiverNumber, bool isPublic){
            TimePeriod = timePeriod;
            Text = text;
            ReceiverNumber = receiverNumber;
            IsPublic = isPublic;
        }
        

        
    }
}