using game.models.gamestate;
namespace game.Services
{
    public class TimeService
    {
        public TimePeriod TimePeriod { get;} = TimePeriod.Start();

        public void ToggleTimeCycle()
        {
            TimePeriod.Next();
        }
        
    }
}