using System.IO;
using UnityEngine;

namespace Managers
{
    public class FilePathManager
    {
        private readonly string _baseDirectory = Application.persistentDataPath;
        public string SettingsFilePath { get; private set; }
        public string LogFilePath { get; private set; }

        public FilePathManager()
        {
            InitializeFilePaths();
        }

        private void InitializeFilePaths()
        {
            SettingsFilePath = Path.Combine(_baseDirectory, "settings.json");
        }

        public string GetFullPath(string relativePath)
        {
            return Path.Combine(_baseDirectory, relativePath);
        }
    }

}