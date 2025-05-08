using System;
using Managers;

namespace game.models.gamestate
{
    public class TimePeriod : IEquatable<TimePeriod>, IComparable<TimePeriod>, ICloneable
    {
        public Time Time {get; set;}
        public int DayCount {get; set;}

        // Constructor
        private TimePeriod(Time time, int dayCount)
        {
            Time = time;
            DayCount = dayCount;
        }

        public static TimePeriod Default()
        {
            return new TimePeriod(Time.Day,-1);
        }

        public static TimePeriod Start()
        {
            return new TimePeriod(Time.Night,1);
        }

        public object Clone()
        {
            return new TimePeriod(Time, DayCount);
        }

        public void Next()
        {
            if(Time == Time.Night) DayCount++;
            Time = Time.Next();
        }

        public TimePeriod GetPrevious()
        {
            var timePeriod = new TimePeriod(Time, DayCount);
            if(Time == Time.Day) timePeriod.DayCount--;
            timePeriod.Time = timePeriod.Time.Previous();
            return timePeriod;
        }

        public string GetAsFormattedString()
        {
            string timeStr = Time != Time.Night ?
                TextManager.Translate("time.day") : TextManager.Translate("time.night");
            return string.Format(timeStr, DayCount);
        }
        

        // Equals method for comparing two TimePeriod objects
        public bool Equals(TimePeriod other)
        {
            if (other == null) return false;
            return DayCount == other.DayCount && Time == other.Time;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TimePeriod);
        }

        // Override GetHashCode to match custom equality logic
        public override int GetHashCode()
        {
            return HashCode.Combine(Time, DayCount);
        }

        public static bool operator ==(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            if (ReferenceEquals(timePeriod1, timePeriod2)) return true;
            if (ReferenceEquals(timePeriod1, null) || ReferenceEquals(timePeriod2, null)) return false;
            return timePeriod1.Equals(timePeriod2);
        }
        
        public static bool operator !=(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            return !(timePeriod1 == timePeriod2);
        }

        public static bool operator >(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            if (timePeriod1 == null || timePeriod2 == null)
            {
                throw new ArgumentNullException("Null values cannot be compared.");
            }
            
            return timePeriod1.CompareTo(timePeriod2) > 0;
        }
        
        public static bool operator <(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            if (timePeriod1 == null || timePeriod2 == null)
            {
                throw new ArgumentNullException("Null values cannot be compared.");
            }

            return timePeriod1.CompareTo(timePeriod2) < 0;
        }
        
        public static bool operator <=(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            if (timePeriod1 == null || timePeriod2 == null)
            {
                throw new ArgumentNullException("Null values cannot be compared.");
            }

            return timePeriod1.CompareTo(timePeriod2) <= 0;
        }
        
        public static bool operator >=(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            if (timePeriod1 == null || timePeriod2 == null)
            {
                throw new ArgumentNullException("Null values cannot be compared.");
            }

            return timePeriod1.CompareTo(timePeriod2) >= 0;
        }

        

        // Method to increment dayCount
        public void IncrementDayCount()
        {
            DayCount++;
        }
        
        // Method to check if this TimePeriod is after another
        public bool IsAfter(TimePeriod other)
        {
            if (DayCount > other.DayCount)
            {
                return true;
            }
            if (DayCount == other.DayCount)
            {
                return Time.CompareTo(other.Time) > 0;
            }
            return false;
        }

        // Method to subtract dayCount of another TimePeriod from this one
        public int Subtract(TimePeriod timePeriod)
        {
            return DayCount - timePeriod.DayCount;
        }

        // Override ToString method to provide string representation of TimePeriod
        public override string ToString()
        {
            return $"TimePeriod{{time={Time}, dayCount={DayCount}}}";
        }

        // IComparable implementation to compare TimePeriod instances
        public int CompareTo(TimePeriod other)
        {
            if (other == null) return 1;
            if (DayCount != other.DayCount)
            {
                return DayCount.CompareTo(other.DayCount);
            }
            return Time.CompareTo(other.Time);
        }
    }
}
