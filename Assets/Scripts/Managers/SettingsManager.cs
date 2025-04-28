using game.models.Settings;
using UnityEngine;

namespace Managers
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public class SettingsManager
    {
        private readonly FilePathManager _filePathManager = ServiceLocator.Get<FilePathManager>();
        private readonly LanguageManager _languageManager = ServiceLocator.Get<LanguageManager>();
        public UserSettings UserSettings { get; set; }

        public SettingsManager()
        {
            LoadSettings();
        }
        
        private bool LoadSettingsFile()
        {
            string settingsFilePath = _filePathManager.SettingsFilePath;

            if (!File.Exists(settingsFilePath))
            {
                Debug.Log($"Ayarlar dosyası bulunamadı: {settingsFilePath}");
                return false;
            }

            try
            {
                var json = File.ReadAllText(settingsFilePath);
                UserSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                _languageManager.LoadLanguageFile(UserSettings.Language);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log($"Ayarlar dosyası yüklenirken bir hata oluştu: {ex.Message}");
                return false;
            }
        }

        public bool LoadSettings()
        {
            bool success = LoadSettingsFile();
            
            if(!success) UserSettings = UserSettings.DefaultSettings;
            
            return success;
        }
        
        public bool SaveSettings()
        {
            string settingsFilePath = _filePathManager.SettingsFilePath;

            try
            {
                var json = JsonConvert.SerializeObject(UserSettings, Formatting.Indented);
                File.WriteAllText(settingsFilePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ayarlar dosyası kaydedilirken bir hata oluştu: {ex.Message}");
                return false;
            }
        }

        public bool ChangeLanguage(Language language)
        {
            UserSettings.Language = language;
            _languageManager.LoadLanguageFile(UserSettings.Language);
            return SaveSettings();
        }

    }

}