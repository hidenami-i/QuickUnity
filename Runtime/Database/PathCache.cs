using UnityEngine;

namespace QuickUnity.Database
{
    /// <summary>
    /// Application path cache class for async.
    /// </summary>
    public static class PathCache
    {
        /// <summary>
        /// Gets Application.dataPath
        /// </summary>
        public static string DataPath { get; private set; }

        /// <summary>
        /// Gets Application.persistentDataPath
        /// </summary>
        public static string PersistentDataPath { get; private set; }

        /// <summary>
        /// Gets Application.streamingAssetsPath
        /// </summary>
        public static string StreamingAssetsPath { get; private set; }

        /// <summary>
        /// Gets Application.temporaryCachePath
        /// </summary>
        public static string TemporaryCachePath { get; private set; }

        /// <summary>
        /// Setup path cache.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Setup()
        {
            DataPath = Application.dataPath;
            PersistentDataPath = Application.persistentDataPath;
            StreamingAssetsPath = Application.streamingAssetsPath;
            TemporaryCachePath = Application.temporaryCachePath;
        }
    }
}
