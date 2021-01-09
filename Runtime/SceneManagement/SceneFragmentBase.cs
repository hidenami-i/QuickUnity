using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class SceneFragmentBase : MonoBehaviourBase
    {
        /// <summary>
        /// Refresh function is called from SceneController.
        /// </summary>
        /// <see cref="FragmentManager"/>
        public virtual void Refresh() { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        public virtual void OnSceneUnloaded(Scene scene) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prevScene"></param>
        /// <param name="nextScene"></param>
        public virtual void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }
    }
}
