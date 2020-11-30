using UnityEngine;

namespace QuickUnity.Core
{
    /// <summary>
    /// Base Singleton
    /// </summary>
    /// <typeparam name="T">MonoBehaviour</typeparam>
    public abstract class SingletonMonoBehaviourBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        protected static bool applicationIsQuitting;

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
                    if (applicationIsQuitting)
                    {
                        return null;
                    }

                    if (instance != null)
                    {
                        return instance;
                    }

                    System.Type type = typeof(T);
                    var objects = FindObjectsOfType<T>();

                    if (objects.Length > 1)
                    {
                        return objects[0];
                    }

                    GameObject managerObject = new GameObject("[Singleton]" + type.Name);
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
            if (instance == null)
            {
                instance = this as T;
                if (IsPersistent())
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            Me = null;
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}