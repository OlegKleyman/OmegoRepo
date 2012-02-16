using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Core.Configuration;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration
{
    [TestFixture]
    public class XbmcCopierConfigurationSectionTests : TestsBase
    {
        private const string CONFIGURATION_SECTION_NAME = "XbmcCopierConfiguration";
        private string XbmcCopierValidNoFiltersConfigFilePath { get; set; }
        
        public override void Setup()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            const string xbmcCopierValidNoFiltersConfigFileName = @"XbmcCopierValidNoFilters.config";
            const string filePath = @"TestConfigs";

            XbmcCopierValidNoFiltersConfigFilePath = Path.Combine(currentDirectory, filePath,
                                                                  xbmcCopierValidNoFiltersConfigFileName);
        }

        [Test]
        public void DefaultSettingsTests()
        {
            var settings = XbmcCopierConfigurationSection.DefaultSettings;
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.AreEqual(2, settings.MovieFilters.Length);
            Assert.AreEqual(@"\.720P\.|\.1080P\.|\.DVDRIP\.|\.PAL\.DVDR\.|\.NTSC\.DVDR\.|\.XVID\.",
                            settings.MovieFilters[0].ToString());
            Assert.AreEqual("testing", settings.MovieFilters[1].ToString());
            Assert.AreEqual(1, settings.TvFilters.Length);
            Assert.AreEqual(@"\.S\d{2}E\d{2}\.", settings.TvFilters[0].ToString());
        }

        [Test]
        public void GetSettingsByConfigurationFileNoFiltersInConfigTest()
        {
            var factory = new ConfigurationSectionFactory<XbmcCopierConfigurationSection>();
            var settings = (ISettingsProvider) factory.GetSettingsByConfigurationFile(XbmcCopierValidNoFiltersConfigFilePath, CONFIGURATION_SECTION_NAME);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.AreEqual(0, settings.MovieFilters.Length);
            Assert.AreEqual(0, settings.TvFilters.Length);
        }
    }
}