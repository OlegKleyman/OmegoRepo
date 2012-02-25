using System;
using System.IO;

namespace Oleg.Kleyman.Tests.Core
{
    public class MockDirectory : FileSystemInfo
    {
        private string _name;

        public MockDirectory(string path)
        {
            FullPath = path;
        }

        #region Overrides of FileSystemInfo

        public override void Delete()
        {

        }

        public override string Name
        {
            get { return _name ?? (_name = GetDirectoryName()); }
        }

        private string GetDirectoryName()
        {
            var candidates = FullPath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
            return candidates[candidates.GetUpperBound(0)];
        }

        public override bool Exists
        {
            get { return true; }
        }

        #endregion

    }
}