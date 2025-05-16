using System;
using System.Collections.Generic;
using game.Utils;
using UnityEngine;

namespace Managers
{
    public static class TextManager
    {
        private static Dictionary<string, string> Translations { get; set; }

        static TextManager()
        {
            Translations = new Dictionary<string, string>();
        }
        
        public static string Translate(string key)
        {
            if (Translations.TryGetValue(key, out string translation))
            {
                return translation;
            }
            
            return $"Translation {key} could not found!";
            
        }

        public static string TranslateEnum<T>(T enumName, string key) where T : Enum
        {
            return Translations[$"{enumName.FormatEnum()}.{key}"];
        }
        public static bool LoadTranslations(Dictionary<string, string> translations)
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