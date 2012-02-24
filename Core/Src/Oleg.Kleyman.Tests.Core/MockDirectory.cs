using System.IO;

namespace Oleg.Kleyman.Tests.Core
{
    public class MockDirectory : FileSystemInfo
    {
        private readonly string _path;

        public MockDirectory(string path)
        {
            _path = path;
        }

        #region Overrides of FileSystemInfo

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }

        public override string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool Exists
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        public override string FullName
        {
            get
            {
                return _path;
            }
        }
    }
}