using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickUnity.Core
{
    [DisallowMultipleComponent]
    public abstract class SceneBlockBase : MonoBehaviourBase
    {
        /// <summary>
        /// AwakeMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual async UniTask AwakeMe()
        {
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// OnEnableMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual async void OnEnableMe()
        {
        }

        /// <summary>
        /// StartMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual async void StartMe()
        {
        }

        /// <summary>
        /// FixedUpdateMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual async void FixedUpdateMe()
        {
        }

        /// <summary>
        /// UpdateMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual async void UpdateMe()
        {
        }

        /// <summary>
        /// LateUpdateMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void LateUpdateMe()
        {
        }

        /// <summary>
        /// OnApplicationPauseMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void OnApplicationPauseMe(bool pauseStatus)
        {
        }

        /// <summary>
        /// OnDisableMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void OnDisableMe()
        {
        }

        /// <summary>
        /// OnDestroyMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void OnDestroyMe()
        {
        }

        /// <summary>
        /// OnApplicationFocusMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void OnApplicationFocusMe(bool hasFocus)
        {
        }

        /// <summary>
        /// Refresh function is called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        public virtual void Refresh()
        {
        }
    }
}
