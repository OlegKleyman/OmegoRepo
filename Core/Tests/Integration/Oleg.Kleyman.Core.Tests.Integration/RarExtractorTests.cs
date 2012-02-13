using System.IO;
using NUnit.Framework;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class RarExtractorTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {

        }

        [Test]
        public void ExtractTest()
        {
            Extractor extractor = new RarExtractor(@"C:\Program Files\WinRAR\unrar.exe");
            string destination = @"C:\testUnrar\";
            Directory.CreateDirectory(destination);
            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            Assert.IsTrue(File.Exists(Path.Combine(destination, "testFile.txt")));
            Directory.Delete(destination, true);
        }
    }
}