using System;
using System.Collections;
using System.Collections.Generic;
using Managers.enums;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneChanger : MonoBehaviour
    {
        private readonly Stack<string> _sceneStack = new ();

        private void Start()
        {
            AddSceneToStack(GetSceneName(SceneType.MainMenu));
        }

        public void LoadScene(SceneType type, bool isOnline = false)
        {
            string sceneName = GetSceneName(type);
            if (isOnline && NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        

        public void LoadSceneAsync(SceneType type, Action onLoaded = null)
        {
            AddSceneToStack(SceneManager.GetActiveScene().name);
            string sceneName = GetSceneName(type);
            StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onLoaded));
        }

        public void GoBack()
        {
            Debug.Log("Go Back");
            foreach (string sceneName in _sceneStack)
            {
                Debug.Log(sceneName);
            }
            if (_sceneStack.Count > 0)
            {
                string sceneName = _sceneStack.Peek();
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }

        public void RefreshScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        private void AddSceneToStack(string sceneName)
        {
            if (_sceneStack.Count == 0 || _sceneStack.Peek() != sceneName)
            {
                _sceneStack.Push(sceneName);
            }
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action onLoaded)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!(operation==null || operation.isDone))
            {
                yield return null;
            }
            onLoaded?.Invoke();
        }

        private string GetSceneName(SceneType type)
        {
            return type + "Scene";
        }
    }
}