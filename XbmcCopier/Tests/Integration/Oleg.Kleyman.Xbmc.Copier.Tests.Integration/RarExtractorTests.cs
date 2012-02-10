using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration
{
    [TestFixture]
    public class RarExtractorTests
    {
        protected ISettingsProvider ConfigSettings { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            ConfigSettings = XbmcCopierConfigurationSection.DefaultSettings;
        }

        [Test]
        public void ExtractTest()
        {
            Extractor extractor = new RarExtractor(ConfigSettings);
            string destination = @"C:\testUnrar\";
            Directory.CreateDirectory(destination);
            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            Assert.IsTrue(File.Exists(destination + "testFile.txt"));
            Directory.Delete(destination, true);
        }
    }
}