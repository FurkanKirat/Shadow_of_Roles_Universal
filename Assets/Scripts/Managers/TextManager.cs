using System;
using System.Collections.Generic;
using game.Constants;
using game.Utils;
using UnityEngine;

namespace Managers
{
    public static class TextManager
    {
        public static Dictionary<string, Dictionary<string, string>> Translations { get; private set; }

        static TextManager()
        {
            Translations = new Dictionary<string, Dictionary<string, string>>();
        }
        
        public static string GetTranslation(this TextCategory category, string key)
        {
            return Translate(category, key);
            
        }
        
        private static string Translate<T>(T category, string key) where T : Enum
        {
            string format = category.FormatEnum();
            if (Translations.ContainsKey(format))
            {
                return Translations[format][key];
            }
            
            return $"Translation {format} - {key} could not found!";
            
        }

        public static string GetEnumTranslation<T>(this TextCategory category, T enumValue) where T : Enum
        {
            
            return Translate(category,enumValue.FormatEnum());
        }
        public static string GetEnumCategoryTranslation<T>(T enumCategory, string value) where T : Enum
        {
            
            return Translate(enumCategory, value);
        }
        
        public static string FormatMessage(this TextCategory category, string key, Dictionary<string, string> placeholders = null)
        {
            string template = GetTranslation(category, key);
            return StringFormatter.ReplacePlaceholders(template, placeholders);
        }
        
        public static string FormatMessage<T>(T category, string key, Dictionary<string, string> placeholders = null) where T : Enum
        {
            string template = Translate(category, key);
            return StringFormatter.ReplacePlaceholders(template, placeholders);
        }

        public static string FormatEnumCategoryMessage<T>(T enumCategory, string key, Dictionary<string, string> placeholders = null) where T : Enum
        {
            string template = GetEnumCategoryTranslation(enumCategory, key);
            return StringFormatter.ReplacePlaceholders(template, placeholders);
        }
        
        public static string FormatEnumMessage<T>(this TextCategory category, T enumKey, Dictionary<string, string> placeholders = null) where T : Enum
        { 
            string template = GetEnumTranslation(category, enumKey);
            return StringFormatter.ReplacePlaceholders(template, placeholders);
        }
        
        public static bool LoadTranslations(Dictionary<string, Dictionary<string,string>> translations)
        {
            if (translations == null || translations.Count == 0)
            {
                Debug.Log("Translations not loaded!");
                return false;
            }

            Translations = translations;
            return true;
        }
    }
}