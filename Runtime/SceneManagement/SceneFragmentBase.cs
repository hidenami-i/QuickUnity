using QuickUnity.Core;
using UnityEngine;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class SceneFragmentBase : MonoBehaviourBase
    {
        /// <summary>
        /// Refresh function is called from SceneController.
        /// </summary>
        /// <see cref="FragmentManager.cs"/>
        public virtual void Refresh()
        {
        }
    }
}
