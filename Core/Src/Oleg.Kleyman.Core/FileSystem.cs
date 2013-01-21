using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core
{
    public class FileSystem : IFileSystem
    {
        #region IFileSystem Members

        /// <summary>
        ///     Copies a file.
        /// </summary>
        /// <param name="sourceFilePath"> The source file to copy. </param>
        /// <param name="destinationFilePath"> The destination path to copy the file too. </param>
        /// <returns>
        ///     A <see cref="FileInfo" /> object of the copied file.
        /// </returns>
        public FileInfo CopyFile(string sourceFilePath, string destinationFilePath)
        {
            File.Copy(sourceFilePath, destinationFilePath);
            var info = new FileInfo(destinationFilePath);
            return info;
        }

        /// <summary>
        ///     Gets files from a directory with a specific extension.
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="extension"> The extension of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="FileInfo" /> array.
        /// </returns>
        public FileInfo[] GetFilesByExtension(string targetDirectory, string extension)
        {
            const string extensionFormat = "*.{0}";
            var extensionSearchPattern = string.Format(extensionFormat, extension);
            var files = from file in Directory.GetFiles(targetDirectory, extensionSearchPattern)
                        let filePath = Path.Combine(targetDirectory, file)
                        select new FileInfo(filePath);
            return files.ToArray();
        }

        /// <summary>
        ///     Gets files from a directory by a list of extensions..
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="extensions"> The extensions of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="FileInfo" /> array.
        /// </returns>
        public FileInfo[] GetFilesByExtensions(string targetDirectory, IEnumerable<string> extensions)
        {
            var files = from extension in extensions
                        from file in GetFilesByExtension(targetDirectory, extension)
                        select file;

            return files.ToArray();
        }

        /// <summary>
        ///     Checked whether a specific file exists or not.
        /// </summary>
        /// <param name="path"> The file path of the file to check. </param>
        /// <returns>
        ///     A <see cref="bool" /> to identify whether the file exists or not.
        /// </returns>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        ///     Checked whether a specific directory exists or not.
        /// </summary>
        /// <param name="path"> The directory path of the directory to check. </param>
        /// <returns>
        ///     A <see cref="bool" /> to identify whether the directory exists or not.
        /// </returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        ///     Creates a directory.
        /// </summary>
        /// <param name="path"> The directory path to create. </param>
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Gets a file.
        /// </summary>
        /// <param name="path"> The file path. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> object that represents the file.
        /// </returns>
        public IFileSystemMember GetFileByPath(string path)
        {
            var file = new FileInfo(path);
            return new FileSystemMember(file);
        }

        /// <summary>
        ///     Gets a directory.
        /// </summary>
        /// <param name="path"> The directory path. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> object that represents the directory.
        /// </returns>
        public IFileSystemMember GetDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            return new FileSystemMember(directory);
        }

        /// <summary>
        ///     Gets the directory file structure that contains all the directories, sub-directories, and files within it.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> array containing all directories and files.
        /// </returns>
        public FileSystemInfo[] GetDirectoryFileStructure(string target)
        {
            var directories = GetDirectoryTree(target);
            var files = GetFileTree(target);
            var structure = directories.Union(files).OrderBy(fileOrDirectory => fileOrDirectory.FullName);

            return structure.ToArray();
        }

        /// <summary>
        ///     Gets all the directories in a specified directory.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> array containing all directories within the target directory.
        /// </returns>
        public FileSystemInfo[] GetDirectories(string target)
        {
            target = Path.GetFullPath(target);
            var directoryNames = Directory.GetDirectories(target);
            var directories = from directoryName in directoryNames
                              select (FileSystemInfo)new DirectoryInfo(directoryName);

            return directories.ToArray();
        }

        /// <summary>
        ///     Gets all directories and sub-directories within a specified directory.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> array containing all directories abd sub-directories.
        /// </returns>
        public FileSystemInfo[] GetDirectoryTree(string target)
        {
            var directories = GetDirectories(target);
            if (!directories.Any())
            {
                return null;
            }
            var directoryTree = new Collection<FileSystemInfo>();

            foreach (var directory in directories)
            {
                directoryTree.Add(directory);
                directories = GetDirectoryTree(directory.FullName);
                if (directories != null)
                {
                    directories.ForEach(directoryTree.Add);
                }
            }

            return directoryTree.ToArray();
        }

        /// <summary>
        ///     Returns all the files in the specified directory.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> array that represents the files.
        /// </returns>
        public FileSystemInfo[] GetFiles(string target)
        {
            target = Path.GetFullPath(target);
            var filePaths = Directory.GetFiles(target);
            var files = from file in filePaths
                        select (FileSystemInfo)new FileInfo(file);
            return files.ToArray();
        }

        /// <summary>
        ///     Gets all files within the specified directory and it's sub directories.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> array that represents the files.
        /// </returns>
        public FileSystemInfo[] GetFileTree(string target)
        {
            var directoryTree = GetDirectoryTree(target);
            var files = from directory in directoryTree
                        from file in GetFiles(directory.FullName)
                        select file;

            return files.ToArray();
        }

        #endregion
    }
}