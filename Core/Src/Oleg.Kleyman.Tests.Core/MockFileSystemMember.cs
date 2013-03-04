using System.IO;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Tests.Core
{
    public class MockFileSystemMember : IFileSystemMember
    {
        public string FullName { get;  set; }
        public FileAttributes Attributes { get; set; }
        public bool Exists { get; set; }
    }
}