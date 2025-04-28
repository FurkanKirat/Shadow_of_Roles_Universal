using game.models.gamestate;
namespace game.Services
{
    public class BaseTimeService
    {
        public TimePeriod TimePeriod { get;} = TimePeriod.Start();

        public virtual void ToggleTimeCycle()
        {
            TimePeriod.Next();
        }
        
    }
}