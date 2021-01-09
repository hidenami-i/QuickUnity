using UnityEngine;

namespace QuickUnity.Core
{
    /// <summary> Application cache class for async. </summary>
    public static class ApplicationCache
    {
        /// <summary> Gets Application.dataPath </summary>
        public static string DataPath { get; private set; }

        /// <summary> Gets Application.persistentDataPath </summary>
        public static string PersistentDataPath { get; private set; }

        /// <summary> Gets Application.streamingAssetsPath </summary>
        public static string StreamingAssetsPath { get; private set; }

        /// <summary> Gets Application.temporaryCachePath </summary>
        public static string TemporaryCachePath { get; private set; }

        /// <summary> Gets Application.identifier </summary>
        public static string Identifier { get; private set; }

        /// <summary> Gets Application.platform </summary>
        public static RuntimePlatform Platform { get; private set; }

        /// <summary> Gets Application.version </summary>
        public static string Version { get; private set; }

        /// <summary> Gets Application.companyName </summary>
        public static string CompanyName { get; private set; }

        /// <summary> Gets Application.productName </summary>
        public static string ProductName { get; private set; }

        /// <summary> Gets Application.unityVersion </summary>
        public static string UnityVersion { get; private set; }

        /// <summary> Setup application cache. </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Setup()
        {
            DataPath = Application.dataPath;
            PersistentDataPath = Application.persistentDataPath;
            StreamingAssetsPath = Application.streamingAssetsPath;
            TemporaryCachePath = Application.temporaryCachePath;
            Identifier = Application.identifier;
            Platform = Application.platform;
            Version = Application.version;
            CompanyName = Application.companyName;
            ProductName = Application.productName;
            UnityVersion = Application.unityVersion;
        }
    }
}
