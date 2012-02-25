using System.Diagnostics;
using System.IO;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class RarExtractorTests : TestsBase
    {
        private Mock<IFileSystem> _fileSystem;
        private Mock<IProcessManager> _processManager;
        private Mock<IProcess> _process;
        private Mock<IFile> _file;
        private Mock<IRarExtractorSettings> MockSettings { get; set; }
        private string TestDirectory { get; set; }

        public override void Setup()
        {
            MockSettings = new Mock<IRarExtractorSettings>();
            MockSettings.SetupGet(settings => settings.UnrarPath).Returns(@"C:\Program Files\WinRAR\unrar.exe");
            _fileSystem = new Mock<IFileSystem>();
            _process = new Mock<IProcess>();
            _processManager = new Mock<IProcessManager>();
            TestDirectory = new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\").FullName;
            _file = new Mock<IFile>();

            #region ExtractTest

            _file.SetupGet(x => x.FullName).Returns("c:\\gitrepos\\maindefault\\Common\\Test\\testFile.Rar");
            _fileSystem.Setup(x => x.GetFileByPath("..\\..\\..\\..\\..\\..\\Common\\Test\\testFile.Rar")).Returns(_file.Object);
            _fileSystem.Setup(x => x.DirectoryExists(@"..\..\..\..\..\..\Common\Test\testUnrar\")).Returns(false);
            _fileSystem.Setup(x => x.FileExists(@"C:\Program Files\WinRAR\unrar.exe")).Returns(true);
            _fileSystem.Setup(x => x.GetDirectory(@"..\..\..\..\..\..\Common\Test\testUnrar\")).Returns(new MockDirectory(@"C:\repo\Common\Test\testUnrar"));
            _process.SetupGet(x => x.HasExited).Returns(false);
            _processManager.Setup(x => x.Start(It.IsAny<ProcessStartInfo>())).Returns(_process.Object);

            #endregion

            #region ExtractUnrarFileNotFoundTest

            _fileSystem.Setup(x => x.FileExists(@"C:\Program Files\WinRAR\InvalidFile.exe")).Returns(false);

            #endregion

        }

        [Test]
        public void ExtractTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\unrar.exe", _fileSystem.Object, _processManager.Object);
            var destination = @"..\..\..\..\..\..\Common\Test\testUnrar\";
            var info = extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            Assert.AreEqual(@"C:\repo\Common\Test\testUnrar", info.FullName);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FileNotFoundException),
            ExpectedExceptionName = "System.IO.FileNotFoundException",
            ExpectedMessage = @"Unable to find unrar file at location C:\Program Files\WinRAR\InvalidFile.exe",
            MatchType = MessageMatch.Exact)]
        public void ExtractUnrarFileNotFoundTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\InvalidFile.exe", _fileSystem.Object, _processManager.Object);
            const string destination = @"..\..\..\..\..\..\Common\Test\testUnrar\";

            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
        }

        [Test]
        public void RarExtractorConstructorTest()
        {
            var settings = MockSettings.Object;
            var extractor = new RarExtractor(settings, _fileSystem.Object, _processManager.Object);
            Assert.IsNotNull(extractor);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", extractor.Settings.UnrarPath);
        }
    }
}