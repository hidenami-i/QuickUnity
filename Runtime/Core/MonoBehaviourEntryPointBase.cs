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
        /// Awake function is automatically called from Unity.
        /// </summary>
        protected virtual void Awake()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.AwakeMe();
            }
        }

        /// <summary>
        /// Awake function is automatically called from Unity.
        /// </summary>
        protected virtual void OnEnable()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.OnEnableMe();
            }
        }

        /// <summary>
        /// Start function is automatically called from Unity.
        /// </summary>
        protected virtual void Start()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.StartMe();
            }
        }

        /// <summary>
        /// OnDisable function is automatically called from Unity.
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.OnDisableMe();
            }
        }

        /// <summary>
        /// OnDestroy function is automatically called from Unity.
        /// </summary>
        protected virtual void OnDestroy()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.OnDestroyMe();
            }
        }

        /// <summary>
        /// OnApplicationFocus function is automatically called from Unity.
        /// </summary>
        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.OnApplicationFocusMe(hasFocus);
            }
        }

        /// <summary>
        /// OnApplicationPause function is automatically called from Unity.
        /// </summary>
        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.OnApplicationPauseMe(pauseStatus);
            }
        }

        /// <summary>
        /// Update function is automatically called from Unity.
        /// </summary>
        protected virtual void Update()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.UpdateMe();
            }
        }

        /// <summary>
        /// LateUpdate function is automatically called from Unity.
        /// </summary>
        protected virtual void LateUpdate()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.LateUpdateMe();
            }
        }

        /// <summary>
        /// FixedUpdate function is automatically called from Unity.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            foreach (SceneBlockBase sceneBlock in sceneBlockList)
            {
                if (sceneBlock == null)
                {
                    continue;
                }

                sceneBlock.FixedUpdateMe();
            }
        }

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
