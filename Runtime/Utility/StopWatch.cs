using System.Diagnostics;
using QuickUnity.Extensions.Unity;

namespace QuickUnity.Utility
{
    public static class StopWatch
    {
        private static Stopwatch stopWatch;

#if UNITY_EDITOR || !DEVELOPMENT_BUILD
        [Conditional("UNITY_EDITOR")]
#endif
        public static void Start()
        {
            if (stopWatch == null)
            {
                stopWatch = new Stopwatch();
            }

            if (stopWatch.IsRunning)
            {
                ExDebug.Log("Stop watch running!!!");
                return;
            }

            stopWatch.Reset();
            stopWatch.Start();
        }

#if UNITY_EDITOR || !DEVELOPMENT_BUILD
        [Conditional("UNITY_EDITOR")]
#endif
        public static void Stop(string message = "")
        {
            stopWatch.Stop();
            ExDebug.Log($"{message} ProcessTime：{stopWatch.Elapsed.TotalSeconds}");
        }
    }
}