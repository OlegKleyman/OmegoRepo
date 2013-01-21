using System;
using System.IO;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Tests.Core
{
    [Serializable]
    public sealed class MockDirectory : FileSystemInfo
    {
        [NonSerialized] private string _name;

        private MockDirectory(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MockDirectory(string path)
        {
            FullPath = path;
        }

        #region Overrides of FileSystemInfo

        public override string Name
        {
            get { return _name ?? (_name = GetDirectoryName()); }
        }

        public override bool Exists
        {
            get { return true; }
        }

        public override void Delete()
        {
        }

        private string GetDirectoryName()
        {
            var candidates = FullPath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
            return candidates[candidates.GetUpperBound(0)];
        }

        #endregion
    }
}