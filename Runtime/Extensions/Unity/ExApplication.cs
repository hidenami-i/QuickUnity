using System;
using UnityEngine;

namespace QuickUnity.Extensions.Unity
{
    public static class ExApplication
    {
        /// <summary>
        /// OpenURL.
        /// url is urlencoded.
        /// </summary>
        public static void OpenURLAsEscapeUri(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            Application.OpenURL(Uri.EscapeUriString(url));
        }

        /// <summary>
        /// Set 60 target frame rate.
        /// </summary>
        public static void SetRealMachineTargetFrameRate60()
        {
            SetRealMachineTargetFrameRate(60);
        }

        /// <summary>
        /// Set 30 target frame rate.
        /// </summary>
        public static void SetRealMachineTargetFrameRate30()
        {
            SetRealMachineTargetFrameRate(30);
        }

        /// <summary>
        /// Set target frame rate.
        /// On the "Unity Editor", this setting is ignored.
        /// </summary>
        /// <param name="frameRate"></param>
        public static void SetRealMachineTargetFrameRate(int frameRate)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }

        /// <summary>
        /// Changes the maximum time available in one frame when reading data asynchronously.
        /// Set it to High when loading and loading screens.
        ///
        /// Low 2ms
        /// BelowNormal 4ms
        /// Normal 10ms
        /// High 50ms
        ///
        /// </summary>
        /// <param name="threadPriority"></param>
        public static void SetBackgroundLoadingPriority(ThreadPriority threadPriority)
        {
            Application.backgroundLoadingPriority = threadPriority;
        }
    }
}