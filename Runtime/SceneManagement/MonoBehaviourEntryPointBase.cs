using System;
using System.Collections.Generic;
using QuickUnity.Core;
using QuickUnity.SceneManagement;
using UnityEngine;

namespace QuickUnity.SceneManagement
{
    [DisallowMultipleComponent]
    public abstract class MonoBehaviourEntryPointBase : MonoBehaviourBase
    {
        [SerializeField] private List<SceneFragmentBase> sceneFragmentList;

        private readonly Dictionary<string, SceneFragmentBase> sceneFragmentCache =
            new Dictionary<string, SceneFragmentBase>();

        public void Refresh<T>() where T : SceneFragmentBase
        {
        }

        /// <summary>
        /// Refresh SceneFragment.
        /// </summary>
        /// <see cref="SceneFragmentBase"/>
        protected void RefreshBy(Predicate<SceneFragmentBase> match)
        {
            SceneFragmentBase sceneBlock = sceneFragmentList.Find(match);
            if (sceneBlock != null)
            {
                sceneBlock.Refresh();
            }
        }

        /// <summary>
        /// Refresh all SceneFragments.
        /// </summary>
        /// <see cref="SceneFragmentBase"/>
        protected void RefreshAll()
        {
            foreach (SceneFragmentBase sceneFragment in sceneFragmentList)
            {
                sceneFragment.Refresh();
            }
        }

        /// <summary>
        /// Gets the SceneFragmentBase component.
        /// </summary>
        /// <param name="sceneFragment"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected bool TryGetSceneFragment<T>(out T sceneFragment) where T : SceneFragmentBase
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
