using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using QuickUnity.Extensions.Unity;

namespace QuickUnity.Extensions.DotNet
{
    /// <summary>
    /// IO Extensions
    /// ref https://docs.microsoft.com/ja-jp/dotnet/api/system.io?view=netframework-4.7.2
    /// </summary>
    public static class ExIO
    {
        private static readonly byte[] EmptyBytes = new byte[0];

        /// <summary>
        /// Creates the directory not exists.
        /// </summary>
        /// <param name="folderPath">Folder path.</param>
        public static void CreateDirectoryNotExist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        /// <summary>
        /// Writes all text.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteAllTextAsUTF8(string filePath, string content)
        {
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        /// <summary>
        /// Writes all text.
        /// File Path is create from the folder path and file name.
        /// </summary>
        /// <param name="folderPath">Folder path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="contents">Content.</param>
        public static void WriteAllTextAsUTF8(string folderPath, string fileName, string contents)
        {
            WriteAllTextAsUTF8(Path.Combine(folderPath, fileName), contents);
        }

        /// <summary>
        /// Writes all bytes.
        /// </summary>
        /// <param name="folderPath">Folder path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
        public static void WriteAllBytes(string folderPath, string fileName, byte[] bytes)
        {
            File.WriteAllBytes(Path.Combine(folderPath, fileName), bytes);
        }

        /// <summary>
        /// Reads all text as utf8.
        /// File Path is create from the folder path and file name.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadAllTextAsUTF8(string folderPath, string fileName)
        {
            return ReadAllTextAsUTF8(Path.Combine(folderPath, fileName));
        }

        /// <summary>
        /// Reads all text as utf8.
        /// </summary>
        /// <param name="filePath"></param>
        public static string ReadAllTextAsUTF8(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
            catch (DirectoryNotFoundException e)
            {
                ExDebug.LogWarning($"FilePath:{filePath}\nDirectoryNotFoundException:{e.StackTrace}");
                return "";
            }
            catch (FileNotFoundException e)
            {
                ExDebug.LogWarning($"FilePath:{filePath}\nFileNotFoundException:{e.StackTrace}");
                return "";
            }
            catch (Exception e)
            {
                ExDebug.LogError($"FilePath:{filePath}\nException:{e.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Reads all bytes.
        /// File Path is create from the folder path and file name.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string folderPath, string fileName)
        {
            return ReadAllBytes(Path.Combine(folderPath, fileName));
        }

        /// <summary>
        /// Reads all bytes.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (DirectoryNotFoundException e)
            {
                ExDebug.LogWarning($"FilePath:{filePath}\nDirectoryNotFoundException:{e.StackTrace}");
                return EmptyBytes;
            }
            catch (FileNotFoundException e)
            {
                ExDebug.LogWarning($"FilePath:{filePath}\nFileNotFoundException:{e.StackTrace}");
                return EmptyBytes;
            }
            catch (Exception e)
            {
                ExDebug.LogError($"FilePath:{filePath}\nException:{e.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Binaries the formatter serialize.
        /// </summary>
        /// <param name="folderPath">Folder path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="content">Content.</param>
        public static void BinaryFormatterSerialize(string folderPath, string fileName, string content)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream =
                new FileStream(Path.Combine(folderPath, fileName), FileMode.Create, FileAccess.Write);
            binaryFormatter.Serialize(fileStream, content);
            fileStream.Close();
        }

        /// <summary>
        /// Binaries the formatter deserialize.
        /// </summary>
        /// <returns>The formatter deserialize.</returns>
        /// <param name="folderPath">Folder path.</param>
        /// <param name="fileName">File name.</param>
        public static string BinaryFormatterDeserialize(string folderPath, string fileName)
        {
            string filePath = Path.Combine(folderPath, fileName);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            string saveData = (string) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return saveData;
        }

        /// <summary>
        /// Delete all the files in the directory.
        /// </summary>
        /// <param name="folderPath">Folder path.</param>
        /// <param name="containsValue">Contains value.</param>
        public static void DeleteAllFilesBy(string folderPath, string containsValue = "")
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            foreach (string file in Directory.GetFiles(folderPath))
            {
                if (containsValue == string.Empty)
                {
                    File.Delete(file);
                }
                else
                {
                    if (file.Contains(containsValue))
                    {
                        File.Delete(file);
                    }
                }
            }

            foreach (string subDir in Directory.GetDirectories(folderPath))
            {
                DeleteAllFilesBy(subDir);
            }
        }

        /// <summary>
        /// Retrieve all the files in the directory.
        /// </summary>
        /// <returns>The all files.</returns>
        /// <param name="rootFolderPath">Folder path.</param>
        public static string[] GetAllFiles(string rootFolderPath)
        {
            if (!Directory.Exists(rootFolderPath))
            {
                return new string[] { };
            }

            return Directory.GetFiles(rootFolderPath, "*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Retrieve all the folders in the directory.
        /// </summary>
        /// <param name="rootFolderPath"></param>
        /// <returns></returns>
        public static string[] GetAllDirectories(string rootFolderPath)
        {
            if (!Directory.Exists(rootFolderPath))
            {
                return new string[] { };
            }

            return Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Rename file name.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="newFileName">New file name.</param>
        public static void Rename(string filePath, string newFileName)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return;
            }

            string directoryName = Path.GetDirectoryName(filePath);
            string destFileName = Path.Combine(directoryName, newFileName);
            FileInfo newFileInfo = new FileInfo(destFileName);
            if (newFileInfo.Exists)
            {
                return;
            }

            fileInfo.MoveTo(destFileName);
        }

        /// <summary>
        /// Copy file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newFileName"></param>
        public static void Copy(string filePath, string newFileName)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return;
            }

            string directoryName = Path.GetDirectoryName(filePath);
            string destFileName = Path.Combine(directoryName, newFileName);
            FileInfo newFileInfo = new FileInfo(destFileName);
            if (newFileInfo.Exists)
            {
                return;
            }

            fileInfo.CopyTo(destFileName);
        }

        /// <summary>
        /// Safe file delete.
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFileNotThrow(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            File.Delete(filePath);
        }

        /// <summary>
        /// Safe directory delete.
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteDirectoryNotThrow(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            Directory.Delete(directoryPath);
        }

        /// <summary>
        /// Delete all files and directories.
        /// </summary>
        /// <param name="rootDirectoryPath">Folder path.</param>
        public static void DeleteAllFilesAndDirectories(string rootDirectoryPath)
        {
            string[] filePaths = GetAllFiles(rootDirectoryPath);

            if (filePaths.Length <= 0)
            {
                return;
            }

            foreach (string path in filePaths)
            {
                DeleteFileNotThrow(path);
            }

            IEnumerable<string> direcToryPaths = GetAllDirectories(rootDirectoryPath).Reverse();
            foreach (string path in direcToryPaths)
            {
                DeleteDirectoryNotThrow(path);
            }
        }

        /// <summary>
        /// Move the file.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="newFilePath"></param>
        public static void MoveTo(string sourceFilePath, string newFilePath)
        {
            if (!File.Exists(sourceFilePath))
            {
                return;
            }

            CreateDirectoryNotExist(Path.GetDirectoryName(newFilePath));
            File.Move(sourceFilePath, newFilePath);
            DeleteFileNotThrow(sourceFilePath);
        }

        public static void DirectoryCopy(string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }

        public static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            // Remove additional special characters that Unity doesn't like
            foreach (char c in "/:?<>*|\\~")
                name = name.Replace(c, '_');
            return name.Trim();
        }

        public static long DirectorySizeIfNullZero(string folderPath)
        {
            if (!IsFolder(folderPath))
            {
                return 0;
            }

            var filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            long totalSize = 0;
            foreach (var filePath in filePaths)
            {
                totalSize += FileSizeIfNullZero(filePath);
            }

            return totalSize;
        }

        public static long FileSizeIfNullZero(string filePath)
        {
            return !File.Exists(filePath) ? 0 : new FileInfo(filePath).Length;
        }

        public static void RemovePathAndMeta(string path, bool removeEmptyParent = false)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            if (File.Exists(path + ".meta"))
                File.Delete(path + ".meta");
            if (removeEmptyParent)
            {
                var parent = Directory.GetParent(path);
                if (parent.GetDirectories().Length == 0 && parent.GetFiles().Length == 0)
                    RemovePathAndMeta(parent.ToString(), removeEmptyParent);
            }
        }

        public static bool IsFolder(string path)
        {
            try
            {
                return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    return false;
                }

                throw;
            }
        }
    }
}