using UnityEngine;

namespace game.Constants
{
    public static class UIConstants
    {
        public static class Colors
        {
            public static readonly Color White = HexToColor("#EEEFFF");
            public static readonly Color Red = HexToColor("#FF0800");
            public static readonly Color DeadColor = HexToColor("#5E1914");
            public static readonly Color DangerColor = HexToColor("#D03D33");
            public static readonly Color Aquamarine = HexToColor("#6BCAE2");
            public static readonly Color Blue = HexToColor("#6495ED");
            public static readonly Color Yellow = HexToColor("#F5E050");
            public static readonly Color Green = HexToColor("#29AB87");

            private static Color HexToColor(string hex)
            {
                if (ColorUtility.TryParseHtmlString(hex, out var color))
                    return color;
                
                Debug.LogWarning($"Invalid hex color: {hex}");
                return Color.magenta;
            }
        }
        
    }
}