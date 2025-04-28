using System;
using System.IO;

namespace Managers.FileReaders
{
    public class FileReaderPC : IFileReader
    {
        public void LoadFile(string path, Action<string> onSuccess, Action<string> onError)
        {
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                onSuccess?.Invoke(content);
            }
            else
            {
                onError?.Invoke($"File not found at path: {path}");
            }
        }
    }
}