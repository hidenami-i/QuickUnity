using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using QuickUnity.Editor.Utility;
using QuickUnity.Extensions.DotNet;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickUnity.Editor.Menu
{
    public static class CreateUnityConstantsMenu
    {
        private static readonly string RootPath =
            Path.Combine(Application.dataPath, "App/auto_generated/Constants");

        [MenuItem("QuickUnity/Generate Constants")]
        private static void Execute(MenuCommand cmd)
        {
            ExIO.CreateDirectoryNotExist(RootPath);

            try
            {
                GenerateScript("LayerNameConstants", builder =>
                {
                    LayerNameStringBuilder(builder);
                    LayerMaskStringBuilder(builder);
                });

                GenerateScript("SortingLayerNameConstants", SortingLayerStringBuilder);
                GenerateScript("PrefabPathConstants", PrefabPathStringBuilder);
                GenerateScript("SceneNameConstants", SceneBuildIndexStringBuilder);
                GenerateScript("TagNameConstants", TagStringBuilder);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("Constants Generator Failed", "", "OK");
                return;
            }

            EditorUtility.DisplayDialog("Constants Generator Completed", "", "OK");
            AssetDatabase.Refresh();
        }

        private static void GenerateScript(string className, Action<StringBuilder> action)
        {
            StringBuilder builder = new StringBuilder();

            builder.SetNameSpace("App");
            builder.AppendLine("{");
            builder.SetSummaryComment("This class is auto-generated do not modify.", 1);
            builder.AppendLine();
            builder.Indent1().AppendFormat($"public static class {className}").AppendLine();
            builder.Indent1().AppendLine("{");
            action.Invoke(builder);
            builder.Indent1().AppendLine("}");
            builder.AppendLine("}");

            ExIO.CreateDirectoryNotExist(RootPath);

            var fileName = $"{className}.cs";
            ExIO.WriteAllText(RootPath, fileName, builder.ToString());
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        private static void LayerNameStringBuilder(StringBuilder builder)
        {
            builder.Indent2().AppendLine("// Layer");
            foreach (var n in InternalEditorUtility.layers.Select(c => new
            {
                var = RemoveInvalidChars(c),
                val = LayerMask.NameToLayer(c)
            }))
            {
                builder.Indent2().AppendFormat(@"public const int {0} = {1};", n.var, n.val).AppendLine();
            }
        }

        private static void LayerMaskStringBuilder(StringBuilder builder)
        {
            builder.Indent2().AppendLine();
            builder.Indent2().AppendLine("// LayerMask");
            foreach (var n in InternalEditorUtility.layers.Select(c => new
            {
                var = RemoveInvalidChars(c),
                val = 1 << LayerMask.NameToLayer(c)
            }))
            {
                builder.Indent2().AppendFormat(@"public const int {0}Mask = {1};", n.var, n.val).AppendLine();
            }
        }

        private static void SortingLayerStringBuilder(StringBuilder builder)
        {
            Type type = typeof(InternalEditorUtility);
            PropertyInfo prop = type.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            var sortingLayerNames = prop.GetValue(null, null) as string[];

            var sortingLayerNameDic = sortingLayerNames.ToDictionary(value => value);
            foreach (var keyValuePair in sortingLayerNameDic)
            {
                var key = RemoveInvalidChars(keyValuePair.Key);
                var name = keyValuePair.Value;
                builder.Indent2().AppendLine($"public const string {key} = \"{name}\";");
            }
        }

        private static void PrefabPathStringBuilder(StringBuilder builder)
        {
            var assets = AssetDatabase.FindAssets("t:Prefab", new[] {"Assets/App"});
            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                if (!path.Contains(nameof(Resources)))
                {
                    continue;
                }

                var pathWithoutExtension = path.Replace(Path.GetExtension(path), string.Empty);
                var prefabPath = pathWithoutExtension.SubStringAfter("Resources/");
                var variableName = RemoveInvalidChars(prefabPath.Replace("/", "_"));

                builder.Indent2().AppendFormat(@"public const string {0} = ""{1}"";", RemoveInvalidChars(variableName),
                    prefabPath).AppendLine();
            }
        }

        private static void SceneBuildIndexStringBuilder(StringBuilder builder)
        {
            var scenes = EditorBuildSettings.scenes;
            if (scenes == null || scenes.Length <= 0)
            {
                return;
            }

            for (var i = 0; i < scenes.Length; i++)
            {
                EditorBuildSettingsScene scene = scenes[i];
                if (!scene.enabled)
                {
                    continue;
                }

                var sceneName = RemoveInvalidChars(Path.GetFileNameWithoutExtension(scene.path));

                if (HasInvalidChars(sceneName))
                {
                    Debug.LogWarning($"{sceneName} has invalid chars.");
                    continue;
                }

                builder.Indent2().AppendLine($"public const string {sceneName} = \"{sceneName}\";");
            }
        }

        private static void TagStringBuilder(StringBuilder builder)
        {
            var tags = InternalEditorUtility.tags;
            if (tags == null || tags.Length <= 0)
            {
                return;
            }

            foreach (var tag in tags)
            {
                var name = RemoveInvalidChars(tag);
                builder.Indent2().AppendLine($@"public const string {name} = ""{name}"";");
            }
        }

        private static readonly string[] InvalidChars =
        {
            " ", "!", "\"", "#", "$", "%", "&", "\'", "(", ")", "-", "=", "^", "~", "\\", "|", "[", "{", "@",
            "`", "]", "}", ":", "*", ";", "+", "/", "?", ".", ">", ",", "<", "　"
        };

        private static string RemoveInvalidChars(string str)
        {
            Array.ForEach(InvalidChars, c => str = str.Replace(c, string.Empty));
            return str;
        }

        private static bool HasInvalidChars(string str)
        {
            foreach (var invalidChar in InvalidChars)
            {
                if (str.Contains(invalidChar))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
