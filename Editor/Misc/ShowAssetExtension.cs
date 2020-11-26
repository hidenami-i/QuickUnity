using UnityEngine;
using UnityEditor;
using System.IO;

namespace QuickUnity.Editor.Misc
{
    [InitializeOnLoad]
    public static class ShowAssetExtension
    {
        private static readonly Color Color;
        private static string prevGuid;
        private static GUIStyle labelStyle;
        private static int columnsCount = 0;
        private const int TwoColumns = 1;

        static ShowAssetExtension()
        {
            if (EditorGUIUtility.isProSkin)
            {
                Color = Color.grey;
            }
            else
            {
                Color = Color.white;
            }

            EditorApplication.projectWindowItemOnGUI += ListItemOnGUI;
        }

        private static void ListItemOnGUI(string guid, Rect rect)
        {
            if (Event.current.type != EventType.Repaint) return;
            if (prevGuid == guid) return;

            prevGuid = guid;

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(EditorStyles.boldLabel) {normal = {textColor = Color}};
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

            if (obj != null && AssetDatabase.IsMainAsset(obj) &&
                !(obj is DefaultAsset && !AssetDatabase.IsForeignAsset(obj)))
            {
                var extension = Path.GetExtension(assetPath);
                var fileName = Path.GetFileNameWithoutExtension(assetPath);
                GUIContent content = new GUIContent(fileName);
                rect.x += (16f + GUI.skin.label.CalcSize(content).x -
                           GUI.skin.label.margin.left * (TwoColumns - columnsCount));
                rect.y += 1f;
                EditorGUI.LabelField(rect, extension, labelStyle);
            }
        }
    }
}
