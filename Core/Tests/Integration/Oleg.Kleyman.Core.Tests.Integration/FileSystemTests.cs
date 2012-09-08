using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class FileSystemTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void CopyFileTest()
        {
            var copier = new FileSystem();
            var file = copier.CopyFile(
                @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile.Rar",
                @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile1.Rar");
            Assert.IsTrue(File.Exists(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile1.Rar"));
            Assert.AreEqual("testFile1.Rar", file.Name);
            var destinationDirectory =
                new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");
            Assert.AreEqual(destinationDirectory.Name, file.Directory.Name);
            File.Delete(file.FullName);
        }

        [Test]
        public void GetDirectoriesTest()
        {
            IFileSystem fileSystem = new FileSystem();

            var directories =
                fileSystem.GetDirectories(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");
            var paths = new string[2];
            paths[0] = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test");
            paths[1] = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1");

            Assert.AreEqual(paths[0], directories[0].FullName);
            Assert.AreEqual(paths[1], directories[1].FullName);
        }

        [Test]
        public void GetDirectoryFileStructureTest()
        {
            IFileSystem fileSystem = new FileSystem();

            var structure =
                fileSystem.GetDirectoryFileStructure(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");

            var paths = new string[9];
            paths[0] = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test");
            paths[1] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test\SecondLevel1");
            paths[2] =
                Path.GetFullPath(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test\SecondLevel1\testFile.txt");
            paths[3] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test\testFile.txt");
            paths[4] = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1");
            paths[5] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel1");
            paths[6] =
                Path.GetFullPath(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel1\testFile.txt");
            paths[7] =
                Path.GetFullPath(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel1\ThirdLevel1");
            paths[8] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel2");

            Assert.AreEqual(paths[0], structure[0].FullName);
            Assert.AreEqual(paths[1], structure[1].FullName);
            Assert.AreEqual(paths[2], structure[2].FullName);
            Assert.AreEqual(paths[3], structure[3].FullName);
            Assert.AreEqual(paths[4], structure[4].FullName);
            Assert.AreEqual(paths[5], structure[5].FullName);
            Assert.AreEqual(paths[6], structure[6].FullName);
            Assert.AreEqual(paths[7], structure[7].FullName);
            Assert.AreEqual(paths[8], structure[8].FullName);
        }

        [Test]
        public void GetDirectoryTreeTest()
        {
            IFileSystem fileSystem = new FileSystem();

            var directories =
                fileSystem.GetDirectoryTree(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");
            var paths = new string[6];
            paths[0] = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test");
            paths[1] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test\SecondLevel1");
            paths[2] = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1");
            paths[3] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel1");
            paths[4] =
                Path.GetFullPath(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel1\ThirdLevel1");
            paths[5] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel2");

            Assert.AreEqual(paths[0], directories[0].FullName);
            Assert.AreEqual(paths[1], directories[1].FullName);
            Assert.AreEqual(paths[2], directories[2].FullName);
            Assert.AreEqual(paths[3], directories[3].FullName);
            Assert.AreEqual(paths[4], directories[4].FullName);
            Assert.AreEqual(paths[5], directories[5].FullName);
        }

        [Test]
        public void GetFileTreeTest()
        {
            IFileSystem fileSystem = new FileSystem();
            var files = fileSystem.GetFileTree(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");

            var paths = new string[3];
            paths[0] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test\testFile.txt");
            paths[1] =
                Path.GetFullPath(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test\SecondLevel1\testFile.txt");
            paths[2] =
                Path.GetFullPath(
                    @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\Test1\SecondLevel1\testFile.txt");

            Assert.AreEqual(paths[0], files[0].FullName);
            Assert.AreEqual(paths[1], files[1].FullName);
            Assert.AreEqual(paths[2], files[2].FullName);
        }

        [Test]
        public void GetFilesByExtensionTest()
        {
            var copier = new FileSystem();
            var files = copier.GetFilesByExtension(
                @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration", "rar");
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("testFile.rar", files[0].Name);
            var destinationDirectory =
                new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");
            Assert.AreEqual(destinationDirectory.Name, files[0].Directory.Name);
        }

        [Test]
        public void GetFilesByExtensionsTest()
        {
            var copier = new FileSystem();
            var files = copier.GetFilesByExtensions(
                @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration", new[] {"rar"});
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("testFile.rar", files[0].Name);
            var destinationDirectory =
                new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");
            Assert.AreEqual(destinationDirectory.Name, files[0].Directory.Name);
        }

        [Test]
        public void GetFilesTest()
        {
            IFileSystem fileSystem = new FileSystem();
            var files = fileSystem.GetFiles(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration");

            var paths = new string[2];
            paths[0] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile.rar");
            paths[1] =
                Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Core.Tests.Integration\testFile.txt");

            Assert.AreEqual(paths[0], files[0].FullName);
            Assert.AreEqual(paths[1], files[1].FullName);
        }
    }
}