using System.Collections.Generic;
using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class QuickSceneManager : SingletonMonoBehaviourBase<QuickSceneManager>
    {
        /// <summary> SceneFragment class cache. </summary>
        private static readonly Dictionary<string, SceneFragmentBase> SceneFragmentCache =
            new Dictionary<string, SceneFragmentBase>();

        protected override bool IsPersistent() => true;

        public static void AddSceneFragment(string name, SceneFragmentBase sceneFragment)
        {
            if (SceneFragmentCache.ContainsKey(name)) return;
            SceneFragmentCache.Add(name, sceneFragment);
        }

        public static void RemoveSceneFragment(string name)
        {
            if (!SceneFragmentCache.ContainsKey(name)) return;
            SceneFragmentCache.Remove(name);
        }

        public static void Refresh()
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                if (keyValuePair.Value == null)
                {
                    continue;
                }

                keyValuePair.Value.Refresh();
            }
        }

        // public static AsyncOperation LoadRootScene(string sceneName)
        // {
        //     var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //     SceneEntity entity = new SceneEntity();
        // }

        public static AsyncOperation LoadAddSceneAsync(int sceneBuildIndex)
        {
            return SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        }

        public static AsyncOperation LoadAddSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public static AsyncOperation LoadSingleSceneAsync(int sceneBuildIndex)
        {
            return SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        }

        public static AsyncOperation LoadSingleSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        public static void ReplaceSceneAsync()
        {
            // SceneManager.UnloadSceneAsync()
        }

        protected void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        protected void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                keyValuePair.Value.OnSceneLoaded(scene, loadSceneMode);
            }
        }

        protected virtual void OnSceneUnloaded(Scene scene)
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                keyValuePair.Value.OnSceneUnloaded(scene);
            }
        }

        protected virtual void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                keyValuePair.Value.OnActiveSceneChanged(prevScene, nextScene);
            }
        }
    }
}
