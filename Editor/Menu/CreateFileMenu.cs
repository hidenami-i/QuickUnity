﻿using System.IO;
using Editor.Extensions;
using QuickUnity.Extensions.DotNet;
using UnityEditor;
using UnityEngine;

namespace QuickUnity.Menu
{
    public static class CreateFileMenu
    {
        [MenuItem("Assets/QuickUnity/CreateFile/Sample.txt")]
        public static void CreateTextFile()
        {
            CreateFile("Sample.txt");
        }

        [MenuItem("Assets/CreateFile/Sample.md")]
        public static void CreateReadMeFile()
        {
            CreateFile("Sample.md");
        }

        [MenuItem("Assets/CreateFile/Sample.json")]
        public static void CreateJsonFile()
        {
            CreateFile("Sample.json");
        }

        private static void CreateFile(string fileName)
        {
            string folderPath = ExAssetDatabase.GetSelectedPathOrFallback();

            if (!ExIO.IsFolder(folderPath))
            {
                Debug.LogWarning("you should select folder.");
                return;
            }

            string filePath = Path.Combine(folderPath, fileName);

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
