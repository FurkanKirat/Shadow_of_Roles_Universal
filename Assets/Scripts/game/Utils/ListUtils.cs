using System.Collections.Generic;
using UnityEngine;

namespace game.Utils
{
    public static class ListUtils
    {
        public static void PrintList<T>(this List<T> list)
        {
            foreach (var variable in list)
            {
                Debug.Log(variable.ToString());
            }
        }
    }
}