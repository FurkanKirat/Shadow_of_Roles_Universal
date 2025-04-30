namespace game.models.roles.properties
{
    [System.Serializable]
    public class Stat
    {
        public int Default { get; set; }
        public int Current { get; set; }

        public Stat(int _default)
        {
            Default = _default;
            Current = _default;
        }

        public Stat(Stat stat)
        {
            Current = stat.Current;
            Default = stat.Default;
        }

        public void Reset()
        {
            Current = Default;
        }

        public void IncrementCurrent()
        {
            Current++;
        }
        
        public void IncrementCurrent(int value)
        {
            Current += value;
        }

        public void DecrementCurrent()
        {
            Current--;
        }
        
        public void DecrementCurrent(int value)
        {
            Current -= value;
        }
    }
}