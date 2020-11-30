using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuickUnity.Extensions.DotNet;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickUnity.Editor.Extensions
{
    public static class ExAssetDatabase
    {
        public static Object FindAsset(string filter, Type type, string fullAssetName)
        {
            var guids = AssetDatabase.FindAssets(filter, null);
            if (guids.IsNullOrEmpty())
            {
                Debug.LogError($"guids is null. by {filter}");
                return null;
            }

            var list = guids.Select(AssetDatabase.GUIDToAssetPath).Where(x => Path.GetFileName(x) == fullAssetName);
            if (list.IsNullOrEmpty())
            {
                Debug.LogError($"{fullAssetName} file is not found.");
                return null;
            }

            return AssetDatabase.LoadAssetAtPath(list.FirstOrDefault(), type);
        }

        public static List<string> FindAllGuidPathByType<T>(string typeName, string[] searchInFolders) where T : Object
        {
            var name = typeName.IsNullOrEmpty() ? typeof(T).Name : typeName;
            var searchFilter = "t:" + name;
            var guids = AssetDatabase.FindAssets(searchFilter, searchInFolders);
            return guids.ToList();
        }

        public static List<T> FindAllAssetByTypeName<T>(string typeName = "", string[] searchInFolders = null)
            where T : Object
        {
            var guidList = FindAllGuidPathByType<T>(typeName, searchInFolders);

            var list = new List<T>();
            if (guidList.IsNullOrEmpty())
            {
                return list;
            }

            foreach (var guid in guidList)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(assetPath))
                {
                    continue;
                }

                list.Add(AssetDatabase.LoadAssetAtPath<T>(assetPath));
            }

            return list;
        }

        public static List<AnimationClip> FindAllAnimationClipAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<AnimationClip>("", searchInFolders);
        }

        public static List<AudioClip> FindAllAudioClipAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<AudioClip>("", searchInFolders);
        }

        public static List<Font> FindAllFontAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<Font>("", searchInFolders);
        }

        public static List<Material> FindAllMaterialAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<Material>("", searchInFolders);
        }

        public static List<Mesh> FindAllMeshAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<Mesh>("", searchInFolders);
        }

        public static List<GameObject> FindAllPrefabAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<GameObject>("Prefab", searchInFolders);
        }

        public static List<SceneAsset> FindAllSceneAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<SceneAsset>("", searchInFolders);
        }

        public static List<Shader> FindAllShaderAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<Shader>("", searchInFolders);
        }

        public static List<Sprite> FindAllSpriteAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<Sprite>("", searchInFolders);
        }

        public static List<Texture> FindAllTextureAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<Texture>("", searchInFolders);
        }

        public static List<TextAsset> FindAllTextAsset(string[] searchInFolders = null)
        {
            return FindAllAssetByTypeName<TextAsset>("", searchInFolders);
        }

        /// <summary>
        /// Open the script file.
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="scriptLine"></param>
        public static void OpenScript(string scriptName, int scriptLine = 0)
        {
            var monoScriptList = FindAllAssetByTypeName<MonoScript>();
            if (monoScriptList.IsNullOrEmpty())
            {
                return;
            }

            foreach (var monoScript in monoScriptList)
            {
                if (AssetDatabase.OpenAsset(monoScript, scriptLine))
                {
                    Debug.LogError($"Couldn't open {scriptName} script.");
                    break;
                }
            }
        }

        public static string GetSelectedPathOrFallback()
        {
            var path = "Assets";

            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }

            return path;
        }
    }
}