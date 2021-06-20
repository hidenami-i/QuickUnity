using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        private static bool shouldSetActiveScene;

        protected override bool IsPersistent() => true;

        /**
         * Add a SceneFragment.
         * <param name="name"></param>
         * <param name="sceneFragment"></param>
         */
        public static void AddSceneFragment(string name, SceneFragmentBase sceneFragment)
        {
            if (SceneFragmentCache.ContainsKey(name)) return;
            SceneFragmentCache.Add(name, sceneFragment);
        }

        /**
         * Delete a cached SceneFragment.
         * <param name="name"></param>
         */
        public static void RemoveSceneFragment(string name)
        {
            if (!SceneFragmentCache.ContainsKey(name)) return;
            SceneFragmentCache.Remove(name);
        }

        /**
         * Refresh all scene fragments.
         */
        public static void Refresh()
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                if (keyValuePair.Value == null)
                    continue;

                keyValuePair.Value.Refresh();
            }
        }

        /**
         * Asynchronously add-loads a scene.
         * Then activate the added scene.
         * <param name="sceneName"></param>
         * <param name="allowMultipleScene"></param>
         */
        public static AsyncOperation LoadAddSceneSetActiveAsync(string sceneName, bool allowMultipleScene = false)
        {
            return LoadAddSceneAsyncInternal(sceneName, true, allowMultipleScene);
        }

        /**
         * Asynchronously add-loads a scene.
         * <param name="sceneName"></param>
         * <param name="allowMultipleScene"></param>
         */
        public static AsyncOperation LoadAddSceneAsync(string sceneName, bool allowMultipleScene = false)
        {
            return LoadAddSceneAsyncInternal(sceneName, false, allowMultipleScene);
        }

        public static async UniTask Test(
            string sceneName,
            CancellationToken cancellationToken,
            IProgress<float> progress)
        {
            CancellationTokenSource linkToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await LoadAddSceneAsyncInternal(sceneName, false, false)
                .ToUniTask(progress, cancellationToken: linkToken.Token);
        }

        /**
         * Asynchronously add-loads a scene.
         * <param name="sceneName"></param>
         * <param name="setActive"></param>
         * <param name="allowMultipleScene"></param>
         */
        internal static AsyncOperation LoadAddSceneAsyncInternal(
            string sceneName,
            bool setActive,
            bool allowMultipleScene)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!allowMultipleScene && scene.isLoaded)
            {
                return (AsyncOperation) null;
            }

            shouldSetActiveScene = setActive;
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /**
         *
         */
        public static async UniTask ReplaceSceneAsync(string newSceneName)
        {
            Scene scene = SceneManager.GetActiveScene();
            await SceneManager.UnloadSceneAsync(scene);
            await LoadAddSceneAsyncInternal(newSceneName, true, false);
        }

        /**
         * Discard all scenes and load a new scene asynchronously.
         */
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
            if (shouldSetActiveScene)
            {
                SceneManager.SetActiveScene(scene);
                shouldSetActiveScene = false;
            }

            foreach (var keyValuePair in SceneFragmentCache)
            {
                if (keyValuePair.Value == null)
                {
                    continue;
                }

                keyValuePair.Value.OnSceneLoaded(scene, loadSceneMode);
            }
        }

        protected virtual void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                if (keyValuePair.Value == null)
                {
                    continue;
                }

                keyValuePair.Value.OnActiveSceneChanged(prevScene, nextScene);
            }
        }

        protected virtual void OnSceneUnloaded(Scene scene)
        {
            foreach (var keyValuePair in SceneFragmentCache)
            {
                if (keyValuePair.Value == null)
                {
                    continue;
                }

                keyValuePair.Value.OnSceneUnloaded(scene);
            }
        }
    }
}
