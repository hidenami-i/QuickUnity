using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickUnity.Core
{
    [DisallowMultipleComponent]
    public abstract class MonoBehaviourEntryPointBase : MonoBehaviourBase
    {
        [SerializeField] private List<SceneBlockBase> sceneBlockList;

        private readonly Dictionary<string, SceneBlockBase> sceneBlockCache = new Dictionary<string, SceneBlockBase>();

        /// <summary>
        /// RefreshBy function.
        /// </summary>
        /// <see cref="SceneBlockBase.cs"/>
        protected void RefreshBy(Predicate<SceneBlockBase> match)
        {
            SceneBlockBase sceneBlock = sceneBlockList.Find(match);
            if (sceneBlock != null)
            {
                sceneBlock.Refresh();
            }
        }

        /// <summary>
        /// RefreshAll function.
        /// </summary>
        /// <see cref="SceneBlockBase.cs"/>
        protected void RefreshAll()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.Refresh();
            }
        }

        /// <summary>
        /// Gets the SceneBlock component.
        /// </summary>
        /// <param name="sceneBlock"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected bool TryGetSceneBlock<T>(out T sceneBlock) where T : SceneBlockBase
        {
            sceneBlock = null;

            if (sceneBlockCache.TryGetValue(typeof(T).Name, out SceneBlockBase cache))
            {
                sceneBlock = (T) cache;
                return true;
            }

            foreach (SceneBlockBase block in sceneBlockList)
            {
                if (block.GetType() == typeof(T))
                {
                    sceneBlock = (T) block;
                    sceneBlockCache.Add(typeof(T).Name, block);
                    return true;
                }
            }

            return false;
        }
    }
}
