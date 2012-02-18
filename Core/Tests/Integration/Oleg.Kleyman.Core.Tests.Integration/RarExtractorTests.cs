using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class RarExtractorTests
    {
        private Mock<IRarExtractorSettings> MockSettings { get; set; }
        private string TestDirectory{get;set;}

        [TestFixtureSetUp]
        public void Setup()
        {
            MockSettings = new Mock<IRarExtractorSettings>();
            MockSettings.SetupGet(settings => settings.UnrarPath).Returns(@"..\..\..\..\..\..\Common\Test");

            TestDirectory = new DirectoryInfo(@"..\..\..\..\..\..\Common\Test\").FullName;
        }

        [Test]
        public void ExtractTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\unrar.exe");
            var destination = @"..\..\..\..\..\..\Common\Test\testUnrar\";
            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            Assert.IsTrue(File.Exists(Path.Combine(destination, "testFile.txt")));
            Directory.Delete(destination, true);
        }

        [Test]
        public void DefaultTest()
        {
            var extractor = RarExtractor.Default;
            Assert.IsInstanceOf<RarExtractor>(extractor);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", extractor.Settings.UnrarPath);
        }

        [Test]
        public void ExtractorConstructorTest()
        {
            var settings = RarExtractorConfigurationSection.Default;
            var extractor = new RarExtractor(settings);
            Assert.IsInstanceOf<RarExtractor>(extractor);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", extractor.Settings.UnrarPath);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FileNotFoundException),
            ExpectedExceptionName = "System.IO.FileNotFoundException",
            ExpectedMessage = @"Unable to find unrar file at location C:\Program Files\WinRAR\InvalidFile.exe",
            MatchType = MessageMatch.Exact)]
        public void ExtractUnrarFileNotFoundTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\InvalidFile.exe");
            const string destination = @"..\..\..\..\..\..\Common\Test\testUnrar\";
            try
            {
                extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            }
            finally
            {
                Directory.Delete(destination, true);
            }
        }
    }
}