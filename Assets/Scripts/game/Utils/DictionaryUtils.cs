using System;
using System.Collections.Generic;

namespace game.Utils
{
    public static class DictionaryUtils
    {
        public static void ComputeIfAbsent<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, Func<List<TValue>> valueFactory)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = valueFactory();
            }
        }
    }
}