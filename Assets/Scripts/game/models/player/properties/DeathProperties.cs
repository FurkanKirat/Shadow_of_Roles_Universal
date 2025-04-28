
using System.Collections.Generic;
using System.Linq;
using game.Constants;
using game.models.gamestate;
using game.Utils;
using Managers;

namespace game.models.player.properties
{
    public class DeathProperties
    {

        public List<CauseOfDeath> CausesOfDeath { get; } = new List<CauseOfDeath>();
        public TimePeriod DeathTimePeriod { get; set; } = TimePeriod.Default();
        public bool IsAlive { get; set; } = true;

        public void AddCauseOfDeath(CauseOfDeath causeOfDeath)
        {
            CausesOfDeath.Add(causeOfDeath);
        }

        public string GetDeathTimeAndDayCount()
        {
            string deathTimeStr = TextCategory.Time.GetEnumTranslation(DeathTimePeriod.Time);
            return string.Format(deathTimeStr, DeathTimePeriod.DayCount);
        }

        public string GetCausesOfDeathAsString()
        {
            return string.Join(", ", CausesOfDeath.Select(death => TextCategory.CauseOfDeath.GetTranslation(death.FormatEnum())));
        }
        

        public override string ToString()
        {
            return
                $"{nameof(CausesOfDeath)}: {CausesOfDeath}, {nameof(DeathTimePeriod)}: {DeathTimePeriod}, {nameof(IsAlive)}: {IsAlive}";
        }
    }
}