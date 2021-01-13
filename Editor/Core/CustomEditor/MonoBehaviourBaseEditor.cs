using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QuickUnity.Attributes;
using QuickUnity.Core;
using QuickUnity.Editor.Extensions;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Extensions.Unity;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickUnity.Editor.Core
{
    /// <summary>
    /// Sets auto component and asset.
    /// </summary>
    [CustomEditor(typeof(MonoBehaviourBase), true)]
    public class MonoBehaviourBaseEditor : UnityEditor.Editor
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Instance;

        public override void OnInspectorGUI()
        {
            ExEditorGUI.Space();
            if (ExEditorGUI.Button("Sets child Components and Assets"))
            {
                SetComponent();
            }

            ExEditorGUI.Space();

            base.OnInspectorGUI();
        }

        public void SetComponent()
        {
            GameObject selectObject = Selection.gameObjects.First();
            var descendants = selectObject.Descendants();
            if (descendants.IsNullOrEmpty())
            {
                return;
            }

            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.Update();

            var fieldInfos = target.GetType().FieldInfoWithBaseClass(BindingFlags);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                Type fieldType = fieldInfo.FieldType;

                // attributes
                var attributes = fieldInfo.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    SerializedProperty serializedProperty = serializedObject.FindProperty(fieldInfo.Name);

                    if (attribute is SetArrayComponentAttribute setArrayComponent)
                    {
                        if (!(fieldType.IsArray || fieldType.IsGenericType))
                        {
                            ExDebug.LogWarning(
                                "SetArrayComponentAttribute needs to be array type of genericType field type.");
                            continue;
                        }

                        if (string.IsNullOrEmpty(setArrayComponent.MatchName))
                        {
                            ExDebug.LogWarning("MatchName is null or empty.");
                            continue;
                        }

                        SetArrayComponent(descendants, setArrayComponent, fieldType, serializedProperty);
                        continue;
                    }

                    if (attribute is SetComponentAttribute setComponentAttribute)
                    {
                        var fieldInfoName = TrimMember(fieldInfo.Name.ConvertsSnakeToLowerCamel());
                        var findName = string.IsNullOrEmpty(setComponentAttribute.TargetGameObjectName)
                            ? fieldInfoName
                            : setComponentAttribute.TargetGameObjectName;
                        GameObject gameObject = descendants.Find(x => x.name == findName) ??
                                                descendants.Find(x => x.name == findName.ConvertsSnakeToUpperCamel());

                        if (gameObject == null)
                        {
                            ExDebug.LogError(
                                $"Target object name [{findName}] is null. in [{EditorSceneManager.GetActiveScene()}] scene.");
                            continue;
                        }

                        if (fieldType == typeof(GameObject))
                        {
                            serializedProperty.objectReferenceValue = gameObject;
                            continue;
                        }

                        Component component = gameObject.GetComponent(fieldType);
                        if (component == null)
                        {
                            ExDebug.LogError(
                                $"Target object name [{findName}] is null. in [{EditorSceneManager.GetActiveScene()}] scene.");
                            continue;
                        }

                        serializedProperty.objectReferenceValue = component;
                        continue;
                    }

                    if (attribute is SetObjectFromAssetsAttribute setObjectFromAssetsAttribute)
                    {
                        var filter = setObjectFromAssetsAttribute.AssetNameWithoutExtension + " " +
                                     $"t:{GetSearchFilterTypeString(setObjectFromAssetsAttribute, fieldType)}";

                        Object obj = ExAssetDatabase.FindAsset(filter, fieldType,
                            setObjectFromAssetsAttribute.AssetName);
                        if (obj == null)
                        {
                            Debug.LogError(
                                $"Object is null.\nfilter:{filter}\nfieldType:{fieldType}\nAssetName:{setObjectFromAssetsAttribute.AssetName}");
                            return;
                        }

                        // prefab
                        if (obj.GetType() == typeof(GameObject) && fieldType != typeof(GameObject))
                        {
                            GameObject prefab = obj as GameObject;
                            Component component = prefab.GetComponent(fieldType);
                            if (component != null)
                            {
                                obj = component;
                            }
                        }

                        serializedProperty.objectReferenceValue = obj;
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private static string GetSearchFilterTypeString(SetObjectFromAssetsAttribute setObjectFromAssetsAttribute,
            Type fieldType)
        {
            var filter = setObjectFromAssetsAttribute.SearchFilterType == null
                ? fieldType.Name
                : setObjectFromAssetsAttribute.SearchFilterType.Name;

            if (setObjectFromAssetsAttribute.IsPrefab)
            {
                filter = "Prefab";
            }

            return filter;
        }

        private static void SetArrayComponent(List<GameObject> descendants,
            SetArrayComponentAttribute setArrayComponentAttribute, Type fieldType,
            SerializedProperty serializedProperty)
        {
            var gameObjectList =
                descendants.FindAll(x => x.name.Contains(setArrayComponentAttribute.MatchName)).ToList();
            if (gameObjectList.IsNullOrEmpty())
            {
                ExDebug.LogWarning(
                    $"There is no GameObject whose name contains {setArrayComponentAttribute.MatchName}");
                return;
            }

            Type arrayType = fieldType.IsArray ? fieldType.GetElementType() : fieldType.GenericTypeArguments[0];
            serializedProperty.arraySize = gameObjectList.Count;

            for (var index = 0; index < gameObjectList.Count; index++)
            {
                GameObject gameObject = gameObjectList[index];
                SerializedProperty property = serializedProperty.GetArrayElementAtIndex(index);

                if (arrayType == typeof(GameObject))
                {
                    property.objectReferenceValue = gameObject;
                    continue;
                }

                Component component = gameObject.GetComponent(arrayType);
                if (component == null)
                {
                    Debug.LogError(
                        $"Target object name [{setArrayComponentAttribute.MatchName}] is null. in [{EditorSceneManager.GetActiveScene()}] scene.");
                    continue;
                }

                property.objectReferenceValue = component;
            }
        }

        private static string TrimMember(string value)
        {
            if (value.StartsWith("m_"))
            {
                value = value.TrimStart('m', '_');
            }
            else if (value.StartsWith("_"))
            {
                value = value.TrimStart('_');
            }

            return value;
        }
    }
}
