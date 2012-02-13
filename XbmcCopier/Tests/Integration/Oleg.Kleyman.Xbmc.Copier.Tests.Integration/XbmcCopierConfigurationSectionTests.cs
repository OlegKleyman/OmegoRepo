using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration
{
    [TestFixture]
    public class XbmcCopierConfigurationSectionTests : TestsBase
    {
        private string XbmcCopierValidConfigFilePath { get; set; }
        private string XbmcCopierValidNoFiltersConfigFilePath { get; set; }
        private string XbmcCopierInvalidNoConfigurationSectionFilePath { get; set; }
        
        public override void Setup()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            const string xbmcCopierValidConfigFileName = @"XbmcCopierValid.config";
            const string xbmcCopierValidNoFiltersConfigFileName = @"XbmcCopierValidNoFilters.config";
            const string xbmcCopierInvalidNoConfigurationSectionFileName =
                @"XbmcCopierInvalidNoConfigurationSection.config";
            const string filePath = @"TestConfigs";

            XbmcCopierValidConfigFilePath = Path.Combine(currentDirectory, filePath, xbmcCopierValidConfigFileName);
            XbmcCopierValidNoFiltersConfigFilePath = Path.Combine(currentDirectory, filePath,
                                                                  xbmcCopierValidNoFiltersConfigFileName);
            XbmcCopierInvalidNoConfigurationSectionFilePath = Path.Combine(currentDirectory, filePath,
                                                                           xbmcCopierInvalidNoConfigurationSectionFileName);
        }

        [Test]
        public void DefaultSettingsTests()
        {
            var settings = XbmcCopierConfigurationSection.DefaultSettings;
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.IsInstanceOf(typeof (FileSystem), settings.FileSystem);
            Assert.AreEqual(2, settings.MovieFilters.Length);
            Assert.AreEqual(@"\.720P\.|\.1080P\.|\.DVDRIP\.|\.PAL\.DVDR\.|\.NTSC\.DVDR\.|\.XVID\.",
                            settings.MovieFilters[0].ToString());
            Assert.AreEqual("testing", settings.MovieFilters[1].ToString());
            Assert.AreEqual(1, settings.TvFilters.Length);
            Assert.AreEqual(@"\.S\d{2}E\d{2}\.", settings.TvFilters[0].ToString());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ConfigurationErrorsException),
            ExpectedExceptionName = "System.Configuration.ConfigurationErrorsException",
            ExpectedMessage = "XbmcCopierConfiguration configuration section not found.",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationConfigurationSectionNotFoundTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = XbmcCopierInvalidNoConfigurationSectionFilePath;

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                                                                                          ConfigurationUserLevel.None);

            XbmcCopierConfigurationSection.GetSettingsByConfiguration(configuration);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ConfigurationErrorsException),
            ExpectedExceptionName = "System.Configuration.ConfigurationErrorsException",
            ExpectedMessage = "Configuration file not found (testFile.config)",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFileFileDoesNotExistTest()
        {
            XbmcCopierConfigurationSection.GetSettingsByConfigurationFile("testFile.config");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentNullException),
            ExpectedExceptionName = "System.ArgumentNullException",
            ExpectedMessage = "Value cannot be null.\r\nParameter name: configurationFilePath",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFilePathIsNullTest()
        {
            XbmcCopierConfigurationSection.GetSettingsByConfigurationFile(null);
        }

        [Test]
        public void GetByConfigurationFileTest()
        {
            ISettingsProvider settings =
                XbmcCopierConfigurationSection.GetSettingsByConfigurationFile(XbmcCopierValidConfigFilePath);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.IsInstanceOf(typeof(FileSystem), settings.FileSystem);
            Assert.AreEqual(2, settings.MovieFilters.Length);
            Assert.AreEqual(@"\.720P\.|\.1080P\.|\.DVDRIP\.|\.PAL\.DVDR\.|\.NTSC\.DVDR\.|\.XVID\.",
                            settings.MovieFilters[0].ToString());
            Assert.AreEqual("testing", settings.MovieFilters[1].ToString());
            Assert.AreEqual(1, settings.TvFilters.Length);
            Assert.AreEqual(@"\.S\d{2}E\d{2}\.", settings.TvFilters[0].ToString());
        }

        [Test]
        public void GetByConfigurationTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = XbmcCopierValidConfigFilePath;

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                                                                                          ConfigurationUserLevel.None);

            ISettingsProvider settings = XbmcCopierConfigurationSection.GetSettingsByConfiguration(configuration);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.IsInstanceOf(typeof(FileSystem), settings.FileSystem);
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
            ISettingsProvider settings =
                XbmcCopierConfigurationSection.GetSettingsByConfigurationFile(XbmcCopierValidNoFiltersConfigFilePath);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.IsInstanceOf(typeof(FileSystem), settings.FileSystem);
            Assert.AreEqual(0, settings.MovieFilters.Length);
            Assert.AreEqual(0, settings.TvFilters.Length);
        }
    }
}