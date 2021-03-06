using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.Editor.Extensions
{
    public static class ExEditorSceneManager
    {
        public static List<GameObject> GetAllGameObjects()
        {
            return Resources.FindObjectsOfTypeAll<GameObject>().ToList();
        }

        public static List<GameObject> GetAllGameObjectsByActiveScene()
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            GameObject[] rootGameObjects = scene.GetRootGameObjects();

            List<GameObject> allGameObjects = new List<GameObject>();

            foreach (GameObject root in rootGameObjects)
            {
                Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in transforms)
                {
                    allGameObjects.Add(child.gameObject);
                }
            }

            return allGameObjects;
        }

        public static void SetActiveSelectionObjectScene()
        {
            GameObject targetObject = Selection.objects[0] as GameObject;
            var sceneList = GetAllScenes().ToList();
            foreach (Scene scene in sceneList)
            {
                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    Transform[] gameObjects = root.GetComponentsInChildren<Transform>(true);
                    foreach (Transform gameObject in gameObjects)
                    {
                        if (gameObject.gameObject.GetInstanceID() == targetObject.GetInstanceID())
                        {
                            EditorSceneManager.SetActiveScene(scene);
                            goto RESULT;
                        }
                    }
                }
            }

            RESULT: ;
        }

        public static List<Scene> GetAllScenes()
        {
            List<Scene> sceneList = new List<Scene>();
            int count = EditorSceneManager.sceneCount;

            if (count <= 0)
            {
                return sceneList;
            }

            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                sceneList.Add(EditorSceneManager.GetSceneAt(i));
            }

            return sceneList;
        }
    }
}
