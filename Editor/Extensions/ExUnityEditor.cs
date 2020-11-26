using UnityEditor;
using UnityEngine;

namespace QuickUnity.Editor.Extensions
{
    public static class ExUnityEditor
    {
        public static bool IsPlayingUnityEditor()
        {
            return EditorApplication.isPlaying || Application.isPlaying || EditorApplication.isCompiling;
        }
    }
}
