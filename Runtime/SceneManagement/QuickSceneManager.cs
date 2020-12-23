using System.Collections.Generic;
using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class QuickSceneManager : SingletonMonoBehaviourBase<QuickSceneManager>
    {
#if UNITY_EDITOR
        /// <summary> For Inspector </summary>
        [SerializeField] private List<string> fragmentManagerList = new List<string>();
#endif

        private static readonly Dictionary<string, FragmentManager> FragmentManagerCache =
            new Dictionary<string, FragmentManager>();

        protected override bool IsPersistent() => true;

        public static void AddFragmentManager<T>(T fragmentManager) where T : FragmentManager
        {
#if UNITY_EDITOR
            Me.fragmentManagerList.Add(typeof(T).Name);
#endif
            FragmentManagerCache.Add(typeof(T).Name, fragmentManager);
        }

        public static void RemoveFragmentManager<T>(T fragmentManager) where T : FragmentManager
        {
#if UNITY_EDITOR
            Me.fragmentManagerList.Remove(typeof(T).Name);
#endif
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
            return FragmentManagerCache[typeof(T).Name];
        }

        // public static AsyncOperation LoadRootScene(string sceneName)
        // {
        //     var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //     SceneEntity entity = new SceneEntity();
        // }

        public static AsyncOperation LoadAddScene(int sceneBuildIndex)
        {
            return SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        }

        public static AsyncOperation LoadAddScene(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public static AsyncOperation LoadSingleScene(int sceneBuildIndex)
        {
            return SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        }

        public static AsyncOperation LoadSingleScene(string sceneName)
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
        }

        protected virtual void OnSceneUnloaded(Scene scene)
        {
        }

        protected virtual void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
        }
    }
}
