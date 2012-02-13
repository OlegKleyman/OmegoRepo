using System.Collections.Generic;
using System.IO;

namespace Oleg.Kleyman.Core
{
    public interface IFileSystem
    {
        FileInfo CopyFile(string sourceFilePath, string destinationFilePath);
        FileInfo[] GetFilesByExtension(string targetDirectory, string extension);
        FileInfo[] GetFilesByExtensions(string targetDirectory, IEnumerable<string> extensions);
    }
}