using System.Collections.Generic;
using System.IO;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Extensions.Security;
using QuickUnity.Extensions.Unity;
using QuickUnity.Utility;

namespace QuickUnity.Database
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// file path cache.
        /// </summary>
        private static readonly Dictionary<string, string> FilePathCache = new Dictionary<string, string>();

        /// <summary>
        /// password cache.
        /// </summary>
        private static readonly Dictionary<string, string> PCache = new Dictionary<string, string>();

        /// <summary>
        /// salt cache.
        /// </summary>
        private static readonly Dictionary<string, string> SCache = new Dictionary<string, string>();

        /// <summary>
        /// Gets the default file path cache.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static string DefaultFilePath(IDatabase database)
        {
#if UNITY_EDITOR
            PathCache.Initialize();
#endif

#if UNITY_EDITOR

            if (!FilePathCache.TryGetValue(database.EntityName, out var filePath))
            {
                filePath = Path.Combine(DatabaseSettings.Location.RootFolderPath, "../",
                    DatabaseSettings.Location.FolderName, database.Schema, database.EntityName + ".json");
                FilePathCache.Add(database.EntityName, filePath);
            }

#else
			if (!filePathCache.TryGetValue(database.Name, out string filePath)) {
				filePath =
 Path.Combine(DatabaseSettings.Location.RootFolderPath, DatabaseSettings.Location.FolderName, Encrypt.MD5ToString(database.Schema), Encrypt.MD5ToString(database.Name) + ".bytes");
				filePathCache.Add(database.Name, filePath);
			}

#endif

            return filePath;
        }

        /// <summary>
        /// Gets the password cache.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static string Password(ISavable database)
        {
            if (!PCache.TryGetValue(database.EntityName, out var p))
            {
                p = PBKDF25.Encrypt(database.EntityName);
                PCache.Add(database.EntityName, p);
            }

            return p;
        }

        /// <summary>
        /// Gets the salt cache.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static string Salt(ISavable database)
        {
            if (!SCache.TryGetValue(database.Name, out var s))
            {
                s = PBKDF25.Encrypt(database.Name);
                SCache.Add(database.Name, s);
            }

            return s;
        }

        /// <summary>
        /// Default save to local.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="shouldDeleteContent"></param>
        public static void Save(this IDatabase database, bool shouldDeleteContent = false)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Save(database, DefaultFilePath(database), true, false);
#else
			EncryptSave(database, DefaultFilePath(database), Password(database), Salt(database), shouldDeleteContent);
#endif
        }

        /// <summary>
        /// Default load from local.
        /// </summary>
        /// <param name="database"></param>
        public static void Load(this IDatabase database)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Load(database, DefaultFilePath(database));
#else
			EncryptLoad(database, DefaultFilePath(database), Password(database), Salt(database));
#endif
        }

        /// <summary>
        /// Default Delete file contents.
        /// </summary>
        /// <param name="database"></param>
        public static void DeleteContents(this IDatabase database)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Save(database, DefaultFilePath(database), true, true);
#else
			EncryptSave(database, DefaultFilePath(database), "", "", true);
#endif
        }

        /// <summary>
        /// Save to local.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        /// <param name="isPrettyPrint"></param>
        /// <param name="isDeleteContent"></param>
        public static void Save(this IDatabase database, string filePath, bool isPrettyPrint, bool isDeleteContent)
        {
            ExIO.WriteAllTextAsUTF8(filePath, isDeleteContent ? "" : database.ToJson(isPrettyPrint));
            RuntimeUnityEditor.AssetDataBaseRefresh();
        }

        /// <summary>
        /// Load from local. and initialize database instance.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        public static void Load(this IDatabase database, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                ExDebug.LogWarning($"File path is null or empty. Database name is {database.EntityName}");
                return;
            }

            database.FromJson(ExIO.ReadAllTextAsUTF8(filePath));
        }

        /// <summary>
        /// Encrypt Save to local.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        /// <param name="isDeleteContent"></param>
        public static void EncryptSave(this IDatabase database, string filePath, bool isDeleteContent)
        {
            var contents = Aes128.Encrypt(isDeleteContent ? "" : database.ToJson(false), Password(database),
                Salt(database));
            File.WriteAllBytes(filePath, contents);
            RuntimeUnityEditor.AssetDataBaseRefresh();
        }

        /// <summary>
        /// Encrypt Save to local.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="isDeleteContent"></param>
        public static void EncryptSave(this IDatabase database, string filePath, string password, string salt,
            bool isDeleteContent)
        {
            var contents = Aes128.Encrypt(isDeleteContent ? "" : database.ToJson(false), password, salt);
            File.WriteAllBytes(filePath, contents);
            RuntimeUnityEditor.AssetDataBaseRefresh();
        }

        /// <summary>
        /// Encrypt Load from local. and initialize database instance.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        public static void EncryptLoad(this IDatabase database, string filePath)
        {
            var contents = ExIO.ReadAllBytes(filePath);
            var jsonData =
                System.Text.Encoding.UTF8.GetString(Aes128.Decrypt(contents, Password(database), Salt(database)));
            database.FromJson(jsonData);
        }

        /// <summary>
        /// Encrypt Load from local. and initialize database instance.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        public static void EncryptLoad(this IDatabase database, string filePath, string password, string salt)
        {
            var contents = ExIO.ReadAllBytes(filePath);
            var jsonData = System.Text.Encoding.UTF8.GetString(Aes128.Decrypt(contents, password, salt));
            database.FromJson(jsonData);
        }
    }
}