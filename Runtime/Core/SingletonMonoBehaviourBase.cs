using UnityEngine;

namespace QuickUnity.Core
{
    /// <summary>
    /// Base Singleton
    /// </summary>
    /// <typeparam name="T">MonoBehaviour</typeparam>
    public abstract class SingletonMonoBehaviourBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        private static readonly object LockObject = new object();

        /// <summary>
        /// true is DontDestroyObject.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsPersistent()
        {
            return true;
        }

        /// <summary>
        /// Get T Singleton Me
        /// </summary>
        /// <returns>T</returns>
        protected static T Me
        {
            get
            {
                lock (LockObject)
                {
                    if (instance != null)
                    {
                        return instance;
                    }

                    instance = FindObjectOfType<T>();

                    if (instance != null)
                    {
                        return instance;
                    }

                    GameObject managerObject = new GameObject("[Singleton]" + typeof(T).Name);
                    instance = managerObject.AddComponent<T>();
                    return instance;
                }
            }
            private set => instance = value;
        }

        /// <summary>
        /// Block Create Constructor
        /// </summary>
        protected SingletonMonoBehaviourBase()
        {
        }

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = GetComponent<T>();
            if (IsPersistent())
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            Me = null;
        }
    }
}