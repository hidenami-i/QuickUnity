using System.Diagnostics;
using UnityEditor;

namespace QuickUnity.Utility
{
    public static class RuntimeUnityEditor
    {
        /// <summary>
        /// Refresh AssetDatabase.
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void AssetDataBaseRefresh()
        {
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}
