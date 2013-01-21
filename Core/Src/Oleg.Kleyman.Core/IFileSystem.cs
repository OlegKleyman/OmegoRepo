using System.Collections.Generic;
using System.IO;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents a file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        ///     Copies a file.
        /// </summary>
        /// <param name="sourceFilePath"> The source file to copy. </param>
        /// <param name="destinationFilePath"> The destination path to copy the file too. </param>
        /// <returns>
        ///     A <see cref="FileInfo" /> object of the copied file.
        /// </returns>
        FileInfo CopyFile(string sourceFilePath, string destinationFilePath);

        /// <summary>
        ///     Gets files from a directory with a specific extension.
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="extension"> The extension of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="FileInfo" /> array.
        /// </returns>
        FileInfo[] GetFilesByExtension(string targetDirectory, string extension);

        /// <summary>
        ///     Gets files from a directory by a list of extensions..
        /// </summary>
        /// <param name="targetDirectory"> The directory to investigate files in. </param>
        /// <param name="extensions"> The extensions of the files to be retrieved. </param>
        /// <returns>
        ///     A <see cref="FileInfo" /> array.
        /// </returns>
        FileInfo[] GetFilesByExtensions(string targetDirectory, IEnumerable<string> extensions);

        /// <summary>
        ///     Checked whether a specific file exists or not.
        /// </summary>
        /// <param name="path"> The file path of the file to check. </param>
        /// <returns>
        ///     A <see cref="bool" /> to identify whether the file exists or not.
        /// </returns>
        bool FileExists(string path);

        /// <summary>
        ///     Checked whether a specific directory exists or not.
        /// </summary>
        /// <param name="path"> The directory path of the directory to check. </param>
        /// <returns>
        ///     A <see cref="bool" /> to identify whether the directory exists or not.
        /// </returns>
        bool DirectoryExists(string path);

        /// <summary>
        ///     Creates a directory.
        /// </summary>
        /// <param name="path"> The directory path to create. </param>
        void CreateDirectory(string path);

        /// <summary>
        ///     Gets a file.
        /// </summary>
        /// <param name="path"> The file path. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> object that represents the file.
        /// </returns>
        IFileSystemMember GetFileByPath(string path);

        /// <summary>
        ///     Gets a directory.
        /// </summary>
        /// <param name="path"> The directory path. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> object that represents the directory.
        /// </returns>
        IFileSystemMember GetDirectory(string path);

        /// <summary>
        ///     Gets the directory file structure that contains all the directories, sub-directories, and files within it.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> array containing all directories and files.
        /// </returns>
        FileSystemInfo[] GetDirectoryFileStructure(string target);

        /// <summary>
        ///     Gets all the directories in a specified directory.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> array containing all directories within the target directory.
        /// </returns>
        FileSystemInfo[] GetDirectories(string target);

        /// <summary>
        ///     Gets all directories and sub-directories within a specified directory.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> array containing all directories abd sub-directories.
        /// </returns>
        FileSystemInfo[] GetDirectoryTree(string target);

        /// <summary>
        ///     Returns all the files in the specified directory.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> array that represents the files.
        /// </returns>
        FileSystemInfo[] GetFiles(string target);

        /// <summary>
        ///     Gets all files within the specified directory and it's sub directories.
        /// </summary>
        /// <param name="target"> The directory path to investigate in. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> array that represents the files.
        /// </returns>
        FileSystemInfo[] GetFileTree(string target);
    }
}