using game.models.gamestate;
using game.models.player;

namespace game.models
{
    public class Message
    {
        
        public TimePeriod TimePeriod { get; }

        public string Text { get; }

        public Player Receiver { get; }

        public bool IsPublic { get; }
        public Message(TimePeriod timePeriod, string text, Player receiver, bool isPublic){
            TimePeriod = timePeriod;
            Text = text;
            Receiver = receiver;
            IsPublic = isPublic;
        }
        

        
    }
}