using System.IO;

namespace Oleg.Kleyman.Core
{
    public class FileDetails : IFile
    {
        private FileInfo FileInfo { get; set; }

        public FileDetails(string path)
        {
            FileInfo = new FileInfo(path);
        }

        #region Implementation of IFile

        public string FullName
        {
            get { return FileInfo.FullName; }
        }

        #endregion
    }
}