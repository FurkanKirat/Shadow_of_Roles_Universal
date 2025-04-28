using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;

namespace game.Services
{
    public class MultDeviceTimeService : BaseTimeService
    {
        public override void ToggleTimeCycle()
        {
            switch (TimePeriod.Time)
            {

                case Time.Voting:
                    TimePeriod.Time = Time.Night;
                    break;

                case Time.Night:
                    TimePeriod.Time = Time.Voting;
                    TimePeriod.IncrementDayCount();
                    break;

            }

        }

    }
}