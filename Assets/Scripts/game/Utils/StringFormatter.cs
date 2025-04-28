using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace game.Utils
{
    public static class StringFormatter
    {
        public static string FormatEnum<T>(this T enumName) where T: Enum
        {
            string enumString = enumName.ToString();
            if (string.IsNullOrWhiteSpace(enumString))
                return string.Empty;

            var builder = new StringBuilder();
            for (int i = 0; i < enumString.Length; i++)
            {
                char c = enumString[i];
                if (char.IsUpper(c) && i > 0)
                    builder.Append('_');

                builder.Append(char.ToLower(c, new CultureInfo("en-US")));
            }
            return builder.ToString();
        }
        

        public static string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            if (string.IsNullOrEmpty(template) || placeholders == null)
                return template;

            foreach (var pair in placeholders)
            {
                template = template.Replace("{" + pair.Key + "}", pair.Value);
            }

            return template;
        }

        public static string Join<T>(this List<T> list, string separator, Func<T, string> formatter)
        {
            return Join(list.ToArray(), separator, formatter);
        }
        
        public static string Join<T>(this T[] arr, string separator, Func<T, string> formatter)
        {
            var sb = new StringBuilder();
            for (int i=0 ; i < arr.Length ; i++)
            {
                sb.Append(formatter(arr[i]));
                if(i == arr.Length - 1) continue;
                sb.Append(separator);
            }
            return sb.ToString();
        }
    }
}
