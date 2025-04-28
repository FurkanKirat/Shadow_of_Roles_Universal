using System;

namespace Managers.FileReaders
{
    public interface IFileReader
    {
        void LoadFile(string path, Action<string> onSuccess, Action<string> onError);
    }
}