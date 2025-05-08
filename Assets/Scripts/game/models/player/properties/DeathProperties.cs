using System;
using System.Linq;
using game.models.gamestate;
using game.Utils;
using Managers;

namespace game.models.player.properties
{
    public class DeathProperties
    {

        public CauseOfDeath CausesOfDeath { get; private set; }
        public TimePeriod DeathTimePeriod { get; set; } = TimePeriod.Default();
        public bool IsAlive { get; set; } = true;

        public void AddCauseOfDeath(CauseOfDeath causeOfDeath)
        {
            CausesOfDeath |= causeOfDeath;
        }
        
        public bool HasCause(CauseOfDeath cause)
        {
            return (CausesOfDeath & cause) != 0;
        }

        public string GetDeathTimeAndDayCount()
        {
            string deathTimeStr = TextManager.Translate($"time.{DeathTimePeriod.Time.FormatEnum()}");
            return string.Format(deathTimeStr, DeathTimePeriod.DayCount);
        }

        public string GetCausesOfDeathAsString()
        {
            if (CausesOfDeath == CauseOfDeath.None) return "";
            var causeList = Enum.GetValues(typeof(CauseOfDeath))
                .Cast<CauseOfDeath>()
                .Where(x => x != CauseOfDeath.None && HasCause(x))
                .Select(cause => TextManager.Translate($"cause_of_death.{cause.FormatEnum()}"));
            return string.Join(", ", causeList);
        }
        

        public override string ToString()
        {
            return
                $"{nameof(CausesOfDeath)}: {CausesOfDeath}, {nameof(DeathTimePeriod)}: {DeathTimePeriod}, {nameof(IsAlive)}: {IsAlive}";
        }
    }
}