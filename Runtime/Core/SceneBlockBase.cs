using System;
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
        protected internal virtual void AwakeMe()
        {
        }

        /// <summary>
        /// OnEnableMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void OnEnableMe()
        {
        }

        /// <summary>
        /// StartMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void StartMe()
        {
        }

        /// <summary>
        /// FixedUpdateMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void FixedUpdateMe()
        {
        }

        /// <summary>
        /// UpdateMe function is automatically called from SceneController.
        /// </summary>
        /// <see cref="MonoBehaviourEntryPointBase.cs"/>
        protected internal virtual void UpdateMe()
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
