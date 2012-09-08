using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class FileSystemMemberTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void AttributesDirectoryTest()
        {
            var directoryPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");
            var directory = new DirectoryInfo(directoryPath);
            IFileSystemMember fileWrapper = new FileSystemMember(directory);
            Assert.AreEqual(FileAttributes.Directory, fileWrapper.Attributes);
        }

        [Test]
        public void AttributesTest()
        {
            var filePath =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile.txt");
            var file = new FileInfo(filePath);
            IFileSystemMember fileWrapper = new FileSystemMember(file);
            Assert.AreEqual(FileAttributes.Archive, fileWrapper.Attributes);
        }

        [Test]
        public void ExistsTest()
        {
            var filePath =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile.txt");
            var file = new FileInfo(filePath);
            IFileSystemMember fileWrapper = new FileSystemMember(file);
            Assert.IsTrue(fileWrapper.Exists);
        }

        [Test]
        public void FullNameTest()
        {
            var filePath =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile.txt");
            var file = new FileInfo(filePath);
            IFileSystemMember fileWrapper = new FileSystemMember(file);
            Assert.AreEqual(filePath, fileWrapper.FullName);
        }
    }
}