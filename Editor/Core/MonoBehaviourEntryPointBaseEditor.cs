using System.Collections.Generic;
using System.Linq;
using QuickUnity.Core;
using QuickUnity.Editor.Extensions;
using UnityEditor;

namespace QuickUnity.Editor.Core
{
    [CustomEditor(typeof(MonoBehaviourEntryPointBase), true)]
    public class MonoBehaviourEntryPointBaseEditor : MonoBehaviourBaseEditor
    {
        public override void OnInspectorGUI()
        {
            ExEditorGUI.Space();
            if (ExEditorGUI.Button("Sets Scene Blocks"))
            {
                ExEditorSceneManager.SetActiveSelectionObjectScene();

                SerializedObject serializedObject = new SerializedObject(target);
                SerializedProperty serializeProperty = serializedObject.FindProperty("sceneBlockList");

                var gameObjectList = ExEditorSceneManager.GetAllGameObjectsByActiveScene();
                var sceneBlockBaseList = new List<SceneBlockBase>();
                foreach (var gameObject in gameObjectList)
                {
                    if (gameObject.TryGetComponent(out SceneBlockBase sceneBlockBase))
                    {
                        sceneBlockBaseList.Add(sceneBlockBase);
                    }
                }

                if (sceneBlockBaseList.Any())
                {
                    serializedObject.Update();
                    serializeProperty.arraySize = sceneBlockBaseList.Count;
                    for (var index = 0; index < sceneBlockBaseList.Count; index++)
                    {
                        SceneBlockBase sceneBlockBase = sceneBlockBaseList[index];
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