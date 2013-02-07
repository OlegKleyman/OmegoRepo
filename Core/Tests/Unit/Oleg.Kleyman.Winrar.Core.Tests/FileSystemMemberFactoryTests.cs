using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class FileSystemMemberFactoryTests : TestsBase
    {
        private Mock<IFileSystem> MockFileSystem { get; set; }
        private Mock<IFileSystemMember> MockFileMember { get; set; }

        public override void Setup()
        {
            MockFileSystem = new Mock<IFileSystem>();
            MockFileMember = new Mock<IFileSystemMember>();
            MockFileSystem.Setup(x => x.GetDirectory(@"C:\testPath\test")).Returns(MockFileMember.Object);
            MockFileSystem.Setup(x => x.GetFileByPath(@"C:\testPath\test.txt")).Returns(MockFileMember.Object);
        }

        [Test]
        public void GetFileShouldReturnTheCorrectFileSystemPathToDirectory()
        {
            MockFileMember.SetupGet(x => x.Attributes).Returns(FileAttributes.Directory);
            MockFileMember.SetupGet(x => x.Exists).Returns(true);
            MockFileMember.SetupGet(x => x.FullName).Returns(@"C:\testPath\test");

            var headerData = new RARHeaderDataEx();
            headerData.FileNameW = "test";
            headerData.Flags = 0xE0;

            var archiveMember = (ArchiveMember)headerData;
            var factory = new FileSystemMemberFactory(MockFileSystem.Object);
            var systemMember = factory.GetFileMember(archiveMember, @"C:\testPath");

            Assert.That(systemMember.Attributes, Is.EqualTo(FileAttributes.Directory));
            Assert.That(systemMember.FullName, Is.EqualTo(@"C:\testPath\test"));
            Assert.That(systemMember.Exists, Is.True);
        }

        [Test]
        public void GetFileShouldReturnTheCorrectFileSystemPathToFile()
        {
            MockFileMember.SetupGet(x => x.Attributes).Returns(FileAttributes.Normal);
            MockFileMember.SetupGet(x => x.Exists).Returns(true);
            MockFileMember.SetupGet(x => x.FullName).Returns(@"C:\testPath\test.txt");

            var headerData = new RARHeaderDataEx();
            headerData.FileNameW = "test.txt";

            var archiveMember = (ArchiveMember)headerData;
            var factory = new FileSystemMemberFactory(MockFileSystem.Object);
            var systemMember = factory.GetFileMember(archiveMember, @"C:\testPath");

            Assert.That(systemMember.Attributes, Is.EqualTo(FileAttributes.Normal));
            Assert.That(systemMember.FullName, Is.EqualTo(@"C:\testPath\test.txt"));
            Assert.That(systemMember.Exists, Is.True);
        }

        [Test]
        public void GetFileShouldShouldThrowArgumentNullExceptionWhenDestinationIsNull()
        {
            var factory = new FileSystemMemberFactory(MockFileSystem.Object);
            var ex = Assert.Throws<ArgumentNullException>(() => factory.GetFileMember(new ArchiveMember(), null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: destinationPath"));
        }

        [Test]
        public void GetFileShouldShouldThrowArgumentNullExceptionWhenArchiveArgumentIsNull()
        {
            var factory = new FileSystemMemberFactory(MockFileSystem.Object);
            var ex = Assert.Throws<ArgumentNullException>(() => factory.GetFileMember(null, string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: archiveMember"));
        }
    }
}
