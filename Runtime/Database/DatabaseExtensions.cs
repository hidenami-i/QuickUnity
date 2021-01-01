using System.Collections.Generic;
using System.IO;
using QuickUnity.Core;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Extensions.Security;
using QuickUnity.Extensions.Unity;
using QuickUnity.Utility;

namespace QuickUnity.Database
{
    public static class DatabaseExtensions
    {
        /// <summary> file path cache. </summary>
        private static readonly Dictionary<string, string> FilePathCache = new Dictionary<string, string>();

        /// <summary> password cache. </summary>
        private static readonly Dictionary<string, string> PCache = new Dictionary<string, string>();

        /// <summary> salt cache. </summary>
        private static readonly Dictionary<string, string> SCache = new Dictionary<string, string>();

        /// <summary>
        /// Default save to local.
        /// </summary>
        /// <param name="database"></param>
        public static void Save(this IDatabase database)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Save(database, FilePath(database), true, false);
#else
			SaveAsEncrypted(database, FilePath(database), Password(database), Salt(database), false);
#endif
        }

        /// <summary>
        /// Default load from local.
        /// </summary>
        /// <param name="database"></param>
        public static void Load(this IDatabase database)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Load(database, FilePath(database));
#else
			LoadAsEncrypted(database, FilePath(database), Password(database), Salt(database));
#endif
        }

        /// <summary>
        /// Default Delete file contents.
        /// </summary>
        /// <param name="database"></param>
        public static void DeleteContents(this IDatabase database)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Save(database, FilePath(database), true, true);
#else
			SaveAsEncrypted(database, FilePath(database), "", "", true);
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
#if UNITY_EDITOR
            CreateDatabaseFolder();
#endif

            ExIO.WriteAllText(filePath, isDeleteContent ? "" : database.ToJson(isPrettyPrint));
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
                ExDebug.LogWarning($"File path is null or empty. Database name is {database.TableName}");
                return;
            }

            database.FromJson(ExIO.ReadAllText(filePath));
        }

        /// <summary>
        /// Encrypt Save to local.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="filePath"></param>
        /// <param name="isDeleteContent"></param>
        public static void SaveAsEncrypted(this IDatabase database, string filePath, bool isDeleteContent)
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
        public static void SaveAsEncrypted(this IDatabase database, string filePath, string password, string salt,
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
        public static void LoadAsEncrypted(this IDatabase database, string filePath)
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
        public static void LoadAsEncrypted(this IDatabase database, string filePath, string password, string salt)
        {
            var contents = ExIO.ReadAllBytes(filePath);
            var jsonData = System.Text.Encoding.UTF8.GetString(Aes128.Decrypt(contents, password, salt));
            database.FromJson(jsonData);
        }

        /// <summary>
        /// Creates a folder to store the data.
        /// </summary>
        public static void CreateDatabaseFolder()
        {
            ExIO.CreateDirectoryNotExist(FolderPath);
        }

        /// <summary>
        /// Gets the default file path cache.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static string FilePath(IDatabase database)
        {
            if (!FilePathCache.TryGetValue(database.TableName, out var filePath))
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                filePath = Path.Combine(FolderPath, $"{database.TableName}.json");
#else
                filePath = Path.Combine(FolderPath, $"{Encrypt.MD5ToString(database.TableName)}.bytes");
#endif
                FilePathCache.Add(database.TableName, filePath);
            }

            return filePath;
        }

        /// <summary>
        /// Gets the password cache.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static string Password(IDatabase database)
        {
            if (!PCache.TryGetValue(database.TableName, out var password))
            {
                password = PBKDF25.Encrypt(database.TableName);
                PCache.Add(database.TableName, password);
            }

            return password;
        }

        /// <summary>
        /// Gets the salt cache.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static string Salt(IDatabase database)
        {
            if (!SCache.TryGetValue(database.TableName, out var salt))
            {
                salt = PBKDF25.Encrypt(database.TableName);
                SCache.Add(database.TableName, salt);
            }

            return salt;
        }

        /// <summary>
        /// Gets the folder path where the data will be saved.
        /// </summary>
        /// <returns></returns>
        private static string FolderPath
        {
            get
            {
#if UNITY_EDITOR
                return Path.Combine(
                    ApplicationCache.DataPath,
                    "../Database");
#elif !UNITY_EDITOR && DEVELOPMENT_BUILD
                return Path.Combine(
                    ApplicationCache.PersistentDataPath,
                    ApplicationCache.CompanyName);
#else
                return Path.Combine(
                    ApplicationCache.PersistentDataPath,
                    Encrypt.MD5ToString(ApplicationCache.CompanyName));
#endif
            }
        }
    }
}
