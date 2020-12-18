using System.Collections.Generic;
using System.Linq;
using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public class QuickSceneManager : SingletonMonoBehaviourBase<QuickSceneManager>
    {
        [SerializeField] private List<FragmentManager> fragmentManagerList;

        protected override bool IsPersistent() => true;

        // public static AsyncOperation LoadRootScene(string sceneName)
        // {
        //     var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //     SceneEntity entity = new SceneEntity();
        // }

        public static void AddFragmentManager(FragmentManager fragmentManager)
        {
            Me.fragmentManagerList.Add(fragmentManager);
        }

        public static void RemoveFragmentManager(FragmentManager fragmentManager)
        {
            Me.fragmentManagerList.Remove(fragmentManager);
        }

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

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            var a = SceneManager.GetActiveScene().GetRootGameObjects()
                .First(x => x.GetComponent<FragmentManager>());
            Debug.Log(a.name);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        /// <summary>
        /// Called between Awake Function and Start Function.
        /// </summary>
        /// <param name="scene">Called Scene</param>
        /// <param name="loadSceneMode"></param>
        protected void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
        }

        protected void OnSceneUnloaded(Scene scene)
        {
        }

        protected void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
        }
    }
}
