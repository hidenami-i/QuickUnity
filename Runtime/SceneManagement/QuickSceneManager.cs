using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public class QuickSceneManager : SingletonMonoBehaviourBase<QuickSceneManager>
    {
        protected override bool IsPersistent() => true;

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
    }
}
