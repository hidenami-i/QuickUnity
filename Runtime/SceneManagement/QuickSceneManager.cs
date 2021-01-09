using System.Collections.Generic;
using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class QuickSceneManager : SingletonMonoBehaviourBase<QuickSceneManager>
    {
        private static readonly Dictionary<string, FragmentManager> FragmentManagerCache =
            new Dictionary<string, FragmentManager>();

        protected override bool IsPersistent() => true;

        public static void AddFragmentManager<T>(T fragmentManager) where T : FragmentManager
        {
            FragmentManagerCache.Add(typeof(T).Name, fragmentManager);
        }

        public static void RemoveFragmentManager<T>(T fragmentManager) where T : FragmentManager
        {
            FragmentManagerCache.Remove(typeof(T).Name);
        }

        public static void RefreshAll()
        {
            foreach (var keyValuePair in FragmentManagerCache)
            {
                keyValuePair.Value.RefreshAll();
            }
        }

        public static void RefreshAllBy<T>() where T : FragmentManager
        {
            FragmentManager result = GetFragmentManager<T>();
            if (result == null) return;
            result.RefreshAll();
        }

        public static FragmentManager GetFragmentManager<T>() where T : FragmentManager
        {
            if (FragmentManagerCache.TryGetValue(typeof(T).Name, out FragmentManager fragmentManager))
            {
                return fragmentManager;
            }

            return null;
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
            foreach (var keyValuePair in FragmentManagerCache)
            {
                keyValuePair.Value.OnSceneLoaded(scene, loadSceneMode);
            }
        }

        protected virtual void OnSceneUnloaded(Scene scene)
        {
            foreach (var keyValuePair in FragmentManagerCache)
            {
                keyValuePair.Value.OnSceneUnloaded(scene);
            }
        }

        protected virtual void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            foreach (var keyValuePair in FragmentManagerCache)
            {
                keyValuePair.Value.OnActiveSceneChanged(prevScene, nextScene);
            }
        }
    }
}
