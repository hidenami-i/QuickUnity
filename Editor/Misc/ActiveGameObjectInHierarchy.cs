using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QuickUnity.Editor.Misc
{
    /// <summary>
    /// Active game object hierarchy.
    /// </summary>
    public static class ActiveGameObjectInHierarchy
    {
        private const int Width = 16;

        [InitializeOnLoadMethod]
        private static void Execute()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
        }

        static void OnGUI(int instanceID, Rect selectionRect)
        {
            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go == null)
            {
                return;
            }

            var pos = selectionRect;
            pos.x = pos.xMax - Width;
            pos.width = Width;

            var newActive = GUI.Toggle(pos, go.activeSelf, string.Empty);

            if (newActive == go.activeSelf)
            {
                return;
            }

            go.SetActive(newActive);
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}