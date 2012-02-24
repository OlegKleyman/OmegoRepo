using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Oleg.Kleyman.Core
{
    public class FileSystem : IFileSystem
    {
        #region IFileSystem Members

        public FileInfo CopyFile(string sourceFilePath, string destinationFilePath)
        {
            File.Copy(sourceFilePath, destinationFilePath);
            var info = new FileInfo(destinationFilePath);
            return info;
        }

        public FileInfo[] GetFilesByExtension(string targetDirectory, string extension)
        {
            const string extensionFormat = "*.{0}";
            var extensionSearchPattern = string.Format(extensionFormat, extension);
            var files = from file in Directory.GetFiles(targetDirectory, extensionSearchPattern)
                        let filePath = Path.Combine(targetDirectory, file)
                        select new FileInfo(filePath);
            return files.ToArray();
        }

        public FileInfo[] GetFilesByExtensions(string targetDirectory, IEnumerable<string> extensions)
        {
            var files = from extension in extensions
                        from file in GetFilesByExtension(targetDirectory, extension)
                        select file;

            return files.ToArray();
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public IFile GetFileByPath(string path)
        {
            return new FileDetails(path);
        }

        public FileSystemInfo GetDirectory(string path)
        {
            return new DirectoryInfo(path);
        }

        #endregion
    }
}