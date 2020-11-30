using System.Linq;
using QuickUnity.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace QuickUnity.Editor.Misc
{
    /// <summary>
    /// Display in Hierarchy whether invalid components are attached.
    /// </summary>
    public static class DisplayInvalidComponentInHierarchy
    {
        private static readonly Texture Icon = ExEditorGUI.LoadErrorIcon();

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
        }

        private static void OnGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go == null)
            {
                return;
            }

            var valid = go.GetComponents<MonoBehaviour>().Any(c => c == null);

            if (!valid)
            {
                return;
            }

            Rect pos = selectionRect;
            pos.x -= 8;
            pos.y -= 4;
            pos.width = 16;

            GUIStyle style = new GUIStyle {fontStyle = FontStyle.Bold};
            GUIStyleState guiStyleState = style.normal;
            guiStyleState.textColor = Color.yellow;
            GUI.Label(pos, new GUIContent
            {
                image = Icon
            }, style);
        }
    }
}