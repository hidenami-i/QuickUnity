using UnityEditor;
using UnityEngine;

namespace Editor.Extensions
{
    public static class ExUnityEditor
    {
        public static bool IsPlayingUnityEditor()
        {
            return EditorApplication.isPlaying || Application.isPlaying || EditorApplication.isCompiling;
        }
    }
}