using System.IO;
using Moq;
using NUnit.Framework;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class RarExtractorTests
    {
        private FileSystem _fileSystem;
        private ProcessManager _processManager;
        private Mock<IRarExtractorSettings> MockSettings { get; set; }
        private string TestDirectory { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            MockSettings = new Mock<IRarExtractorSettings>();
            MockSettings.SetupGet(settings => settings.UnrarPath).Returns(@"..\..\..\..\..\..\Common\Test");
            _fileSystem = new FileSystem();
            _processManager = new ProcessManager();
            TestDirectory = new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\").FullName;
        }

        [Test]
        public void DefaultTest()
        {
            var extractor = RarExtractor.Default;
            Assert.IsInstanceOf<RarExtractor>(extractor);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", extractor.Settings.UnrarPath);
        }

        [Test]
        public void ExtractTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\unrar.exe", _fileSystem, _processManager);
            var destination = @"..\..\..\..\..\..\Common\Test\testUnrar\";
            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            Assert.IsTrue(File.Exists(Path.Combine(destination, "testFile.txt")));
            Directory.Delete(destination, true);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (FileNotFoundException),
            ExpectedExceptionName = "System.IO.FileNotFoundException",
            ExpectedMessage = @"Unable to find unrar file at location C:\Program Files\WinRAR\InvalidFile.exe",
            MatchType = MessageMatch.Exact)]
        public void ExtractUnrarFileNotFoundTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\InvalidFile.exe", _fileSystem,
                                                   _processManager);
            const string destination = @"..\..\..\..\..\..\Common\Test\testUnrar\";

            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
        }
    }
}