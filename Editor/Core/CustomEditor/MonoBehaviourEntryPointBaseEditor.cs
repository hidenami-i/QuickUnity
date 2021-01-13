using System.Collections.Generic;
using System.Linq;
using QuickUnity.Editor.Extensions;
using QuickUnity.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace QuickUnity.Editor.Core
{
    [UnityEditor.CustomEditor(typeof(FragmentManager), true)]
    public class MonoBehaviourEntryPointBaseEditor : MonoBehaviourBaseEditor
    {
        public override void OnInspectorGUI()
        {
            ExEditorGUI.Space();
            if (ExEditorGUI.Button("Sets Scene Fragments"))
            {
                ExEditorSceneManager.SetActiveSelectionObjectScene();

                SerializedObject serializedObject = new SerializedObject(target);
                SerializedProperty serializeProperty = serializedObject.FindProperty("sceneFragmentList");

                var gameObjectList = ExEditorSceneManager.GetAllGameObjectsByActiveScene();
                var sceneFragmentBaseList = new List<SceneFragmentBase>();
                foreach (GameObject gameObject in gameObjectList)
                {
                    if (gameObject.TryGetComponent(out SceneFragmentBase sceneFragmentBase))
                    {
                        sceneFragmentBaseList.Add(sceneFragmentBase);
                    }
                }

                if (sceneFragmentBaseList.Any())
                {
                    serializedObject.Update();
                    serializeProperty.arraySize = sceneFragmentBaseList.Count;
                    for (var index = 0; index < sceneFragmentBaseList.Count; index++)
                    {
                        SceneFragmentBase sceneBlockBase = sceneFragmentBaseList[index];
                        SerializedProperty property = serializeProperty.GetArrayElementAtIndex(index);
                        property.objectReferenceValue = sceneBlockBase;
                    }

                    serializedObject.ApplyModifiedProperties();
                }
            }

            base.OnInspectorGUI();
        }
    }
}
