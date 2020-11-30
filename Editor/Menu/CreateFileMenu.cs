using System.IO;
using QuickUnity.Editor.Extensions;
using QuickUnity.Extensions.DotNet;
using UnityEditor;
using UnityEngine;

namespace QuickUnity.Editor.Menu
{
    public static class CreateFileMenu
    {
        [MenuItem("Assets/QuickUnity/CreateFile/Sample.txt")]
        public static void CreateTextFile()
        {
            CreateFile("Sample.txt");
        }

        [MenuItem("Assets/QuickUnity/CreateFile/ample.md")]
        public static void CreateReadMeFile()
        {
            CreateFile("Sample.md");
        }

        [MenuItem("Assets/QuickUnity/CreateFile/Sample.json")]
        public static void CreateJsonFile()
        {
            CreateFile("Sample.json");
        }

        private static void CreateFile(string fileName)
        {
            var folderPath = ExAssetDatabase.GetSelectedPathOrFallback();

            if (!ExIO.IsFolder(folderPath))
            {
                Debug.LogWarning("you should select folder.");
                return;
            }

            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                Debug.LogWarning($"already file exists. {fileName}");
                return;
            }

            File.WriteAllText(filePath, "", System.Text.Encoding.UTF8);
            AssetDatabase.Refresh();
        }
    }
}