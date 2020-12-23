using System;
using System.Diagnostics;
using QuickUnity.Core;
using UnityEngine;

namespace QuickUnity.Core
{
    public static class AppSettings
    {
        /// <summary>
        /// Sets target framerate
        /// </summary>
        /// <param name="frameRate"></param>
        public static void SetTargetFrame(int frameRate)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }

        /// <summary>
        /// Sets multi touch enable.
        /// </summary>
        /// <param name="enable"></param>
        public static void SetMultiTouchEnabled(bool enable)
        {
            Input.multiTouchEnabled = enable;
        }

        public static class Mobile
        {
            public static class iOS
            {
                /// <summary>
                /// <para>Sets file flag to be excluded from iCloud/iTunes backup.</para>
                /// </summary>
                /// <param name="filePath"></param>
                [Conditional("UNITY_IOS")]
                public static void SetNoBackupFlag()
                {
#if UNITY_IPHONE || UNITY_IOS
                    UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
#endif
                }

                /// <summary>
                /// <para>Binary formatter for ios.</para>
                /// </summary>
                [Conditional("UNITY_IOS")]
                public static void EnableMonoReflectionSerializer()
                {
                    Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
                }
            }
        }
    }
}
