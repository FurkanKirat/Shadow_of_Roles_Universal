using System;

namespace game.Utils
{
    public static class EnumUtils
    {
        public static T GetEnumValue<T>(int value) where T : Enum
        {
            
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}