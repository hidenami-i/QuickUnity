using System.Collections.Generic;
using QuickUnity.Core;
using UnityEngine;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public class FragmentManager : MonoBehaviourBase
    {
        [SerializeField] private List<SceneFragmentBase> sceneFragmentList;

        /// <summary>
        /// SceneFragment class cache.
        /// </summary>
        private readonly Dictionary<string, SceneFragmentBase> sceneFragmentCache =
            new Dictionary<string, SceneFragmentBase>();

        /// <summary>
        /// Refresh SceneFragment.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Refresh<T>() where T : SceneFragmentBase
        {
            if (TryGetSceneFragment(out T sceneFragment))
            {
                sceneFragment.Refresh();
            }
        }

        /// <summary>
        /// Refresh all SceneFragments.
        /// </summary>
        /// <see cref="SceneFragmentBase"/>
        public void RefreshAll()
        {
            foreach (SceneFragmentBase sceneFragment in sceneFragmentList)
            {
                if (sceneFragment == null)
                {
                    continue;
                }

                sceneFragment.Refresh();
            }
        }

        /// <summary>
        /// Gets the SceneFragmentBase component.
        /// </summary>
        /// <param name="sceneFragment"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetSceneFragment<T>(out T sceneFragment) where T : SceneFragmentBase
        {
            sceneFragment = null;

            if (sceneFragmentCache.TryGetValue(typeof(T).Name, out SceneFragmentBase cache))
            {
                sceneFragment = (T) cache;
                return true;
            }

            foreach (SceneFragmentBase block in sceneFragmentList)
            {
                if (block.GetType() == typeof(T))
                {
                    sceneFragment = (T) block;
                    sceneFragmentCache.Add(typeof(T).Name, block);
                    return true;
                }
            }

            return false;
        }
    }
}
