using System;
using System.Collections.Generic;
using game.models.Settings;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class LanguageManager
    {
        public bool LoadLanguageFile(Language language)
        {
            
            try
            {
                string resourcePath = $"Strings/{language.Code}";
                TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);

                if (jsonFile == null)
                {
                    Debug.LogError($"Language file not found in Resources at: {resourcePath}");
                    return false;
                }

                var translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonFile.text);
                return TextManager.LoadTranslations(translations);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading language file: " + ex.Message);
                return false;
            }
            
        }
        
    }
}