namespace QuickUnity.Core
{
    public class SingletonBase<T> where T : new()
    {
        private static T instance;

        private static readonly object LockObject = new object();

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

                    instance = new T();
                    return instance;
                }
            }
        }

        /// <summary>
        /// Block Create Constructor
        /// </summary>
        protected SingletonBase()
        {
        }
    }
}
