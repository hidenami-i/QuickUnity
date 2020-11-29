using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;

namespace QuickUnity.Editor.Misc
{
    [InitializeOnLoad]
    public class SceneActiveSwitcher : UnityEditor.Editor
    {
        static SceneActiveSwitcher()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawComponentIcons;
        }

        private static void DrawComponentIcons(int instanceID, Rect rect)
        {
            if (Application.isPlaying)
            {
                return;
            }

            rect.x += rect.width - 40;
            rect.width = 40;

            if (EditorUtility.InstanceIDToObject(instanceID))
            {
                return;
            }

            var setups = EditorSceneManager.GetSceneManagerSetup();
            if (null == setups || setups.Length <= 0)
            {
                return;
            }

            MethodInfo miGetSceneByHandle =
                typeof(EditorSceneManager).GetMethod("GetSceneByHandle", BindingFlags.NonPublic | BindingFlags.Static);
            Scene s = (Scene) miGetSceneByHandle.Invoke(null, new object[] {instanceID});

            if (s.isLoaded != GUI.Toggle(rect, s.isLoaded, ""))
            {
                foreach (SceneSetup t in setups)
                {
                    if (t.path == s.path)
                    {
                        t.isLoaded = !s.isLoaded;
                        if (s.isLoaded)
                        {
                            EditorSceneManager.SaveScene(s);
                            EditorSceneManager.CloseScene(s, false);
                        }
                        else
                        {
                            EditorSceneManager.OpenScene(s.path, OpenSceneMode.Additive);
                        }

                        break;
                    }
                }
            }
        }
    }
}