using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Managers.FileReaders
{
    public class FileReaderMobile : MonoBehaviour, IFileReader
    {
        public void LoadFile(string path, Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(LoadCoroutine(path, onSuccess, onError));
        }

        private IEnumerator LoadCoroutine(string path, Action<string> onSuccess, Action<string> onError)
        {
            using var www = UnityWebRequest.Get(path);
            yield return www.SendWebRequest();
            
            if(www.result != UnityWebRequest.Result.Success)
                onError?.Invoke($"Failed to load file from {path}");
            else
                onSuccess?.Invoke(www.downloadHandler.text);
        }
    }
}