using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace game.models.Settings
{
    [System.Serializable]
    public class Language
    {
        public static readonly Language English = new Language("en_us", "English");
        public static readonly Language Turkish = new Language("tr_tr", "Türkçe");

        public string Code { get; }
        public string Text { get; }

        [JsonConstructor]
        private Language(string code, string text)
        {
            Code = code;
            Text = text;
        }
        

        public static Language[] Values()
        {
            return new[] { English, Turkish };
        }

        public static List<Language> ValuesList()
        {
            var languages = new List<Language>(Values());
            return languages;
        }
        
        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            return obj is Language otherLanguage && Code.Equals(otherLanguage.Code);
        }

        public static bool operator ==(Language lang1, Language lang2)
        {
            if (lang1 == null && lang2 == null) return true;
            if (lang1 is null || lang2 is null) return false;
            if (ReferenceEquals(lang1, lang2)) return true;
            return lang1.Equals(lang2);
        }
        
        public static bool operator !=(Language lang1, Language lang2)
        {
            return !(lang1 == lang2);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }

}


