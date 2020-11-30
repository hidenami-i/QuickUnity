using System;
using System.Diagnostics;
using UnityEngine;

namespace QuickUnity.Core
{
    public static class AppSettings
    {
        /// <summary>
        /// Set target framerate
        /// </summary>
        /// <param name="frameRate"></param>
        public static void SetTargetFrame(int frameRate)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }

        /// <summary>
        /// Set multi touch enable.
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
                /// <para>Set file flag to be excluded from iCloud/iTunes backup.</para>
                /// </summary>
                /// <param name="filePath"></param>
                [Conditional("UNITY_IOS")]
                public static void SetNoBackupFlag(string filePath)
                {
#if UNITY_IPHONE || UNITY_IOS
					UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
#endif
                }

                /// <summary>
                /// <para>binary formatter for ios.</para>
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