using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class FileSystemTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        [Test]
        public void CopyFileTest()
        {
            var copier = new FileSystem();
            var file = copier.CopyFile(@"..\..\..\..\..\..\Common\Test\testFile.Rar", @"..\..\..\..\..\..\Common\Test\testFile1.Rar");
            Assert.IsTrue(File.Exists(@"..\..\..\..\..\..\Common\Test\testFile1.Rar"));
            Assert.AreEqual("testFile1.Rar", file.Name);
            var destinationDirectory = new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\");
            Assert.AreEqual(destinationDirectory.Name, file.Directory.Name);
            File.Delete(file.FullName);
        }

        [Test]
        public void GetFilesByExtensionTest()
        {
            var copier = new FileSystem();
            var files = copier.GetFilesByExtension(@"..\..\..\..\..\..\Common\Test\", "rar");
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("testFile.rar", files[0].Name);
            var destinationDirectory = new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\");
            Assert.AreEqual(destinationDirectory.Name, files[0].Directory.Name);
        }

        [Test]
        public void GetFilesByExtensionsTest()
        {
            var copier = new FileSystem();
            var files = copier.GetFilesByExtensions(@"..\..\..\..\..\..\Common\Test\", new[] { "rar" }); 
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("testFile.rar", files[0].Name);
            var destinationDirectory = new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\");
            Assert.AreEqual(destinationDirectory.Name, files[0].Directory.Name);
        }

    }
}
