using QuickUnity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class SceneFragmentBase : MonoBehaviourBase
    {
        // TODO シーンが消える前

        protected virtual void Awake() => QuickSceneManager.AddSceneFragment(GetType().Name, this);
        protected virtual void OnDestroy() => QuickSceneManager.RemoveSceneFragment(GetType().Name);

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

        private void OnValidate()
        {
            gameObject.name = GetType().Name;
            OnValidateMe();
        }

        protected virtual void OnValidateMe() { }
    }
}
