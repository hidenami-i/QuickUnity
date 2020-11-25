using UnityEditor;
using UnityEngine;

namespace Editor.Extensions
{
    public class ExEditorWindow
    {
        public static EditorWindow GetProjectWindow()
        {
            return GetExistingWindowByName("Project");
        }

        public static EditorWindow GetExistingWindowByName(string name)
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (EditorWindow item in windows)
            {
                if (item.titleContent.text == name)
                {
                    return item;
                }
            }

            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Project")
            {
                return EditorWindow.focusedWindow;
            }

            return default;
        }
    }
}