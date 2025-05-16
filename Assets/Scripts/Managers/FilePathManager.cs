using System.IO;
using UnityEngine;

namespace Managers
{
    public class FilePathManager
    {
        private readonly string _persistentDirectory = Application.persistentDataPath;
        public string SettingsFilePath { get; private set; }
        public string LogFilePath { get; private set; }
        public string MetaDataFilePath { get; private set; }

        public FilePathManager()
        {
            InitializeFilePaths();
        }

        private void InitializeFilePaths()
        {
            SettingsFilePath = Path.Combine(_persistentDirectory, "settings.json");
            MetaDataFilePath = "Assets/Resources/meta-data.json";
        }

        public string GetFullPath(string relativePath)
        {
            return Path.Combine(_persistentDirectory, relativePath);
        }
    }

}