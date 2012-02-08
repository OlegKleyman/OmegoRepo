using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration
{
    [TestFixture]
    public class XbmcCopierConfigurationSectionTests
    {
        private string XbmcCopierValidConfigFilePath { get; set; }
        private string XbmcCopierValidNoFiltersConfigFilePath { get; set; }
        private string XbmcCopierInvalidNoConfigurationSectionFilePath { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            const string xbmcCopierValidConfigFileName = @"XbmcCopierValid.config";
            const string xbmcCopierValidNoFiltersConfigFileName = @"XbmcCopierValidNoFilters.config";
            const string xbmcCopierInvalidNoConfigurationSectionFileName = @"XbmcCopierInvalidNoConfigurationSection.config";
            const string filePath = @"{0}\TestConfigs\{1}";
            XbmcCopierValidConfigFilePath = string.Format(filePath, currentDirectory, xbmcCopierValidConfigFileName);
            XbmcCopierValidNoFiltersConfigFilePath = string.Format(filePath, currentDirectory, xbmcCopierValidNoFiltersConfigFileName);
            XbmcCopierInvalidNoConfigurationSectionFilePath = string.Format(filePath, currentDirectory, xbmcCopierInvalidNoConfigurationSectionFileName);
        }

        [Test]
        public void GetByConfigurationFileTest()
        {
            var settings = XbmcCopierConfigurationSection.GetSettingsByConfigurationFile(XbmcCopierValidConfigFilePath);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.AreEqual(2, settings.Filters.Length);
            Assert.AreEqual("testing", settings.Filters[0]);
            Assert.AreEqual("second test", settings.Filters[1]);
        }

        [Test]
        public void GetByConfigurationTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = XbmcCopierValidConfigFilePath;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var settings = XbmcCopierConfigurationSection.GetSettingsByConfiguration(configuration);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.AreEqual(2, settings.Filters.Length);
            Assert.AreEqual("testing", settings.Filters[0]);
            Assert.AreEqual("second test", settings.Filters[1]);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentNullException),
                           ExpectedExceptionName = "System.ArgumentNullException",
                           ExpectedMessage = "Value cannot be null.\r\nParameter name: configurationFilePath",
                           MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFilePathIsNullTest()
        {
            XbmcCopierConfigurationSection.GetSettingsByConfigurationFile(null);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ConfigurationErrorsException),
                          ExpectedExceptionName = "System.Configuration.ConfigurationErrorsException",
                          ExpectedMessage = "Configuration file not found (testFile.config)",
                          MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFileFileDoesNotExistTest()
        {
            XbmcCopierConfigurationSection.GetSettingsByConfigurationFile("testFile.config");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ConfigurationErrorsException),
                          ExpectedExceptionName = "System.Configuration.ConfigurationErrorsException",
                          ExpectedMessage = "XbmcCopierConfiguration configuration section not found.",
                          MatchType = MessageMatch.Exact)]
        public void GetByConfigurationConfigurationSectionNotFoundTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = XbmcCopierInvalidNoConfigurationSectionFilePath;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            XbmcCopierConfigurationSection.GetSettingsByConfiguration(configuration);
        }

        [Test]
        public void DefaultSettingsNoFiltersInConfigTest()
        {
            var settings = XbmcCopierConfigurationSection.GetSettingsByConfigurationFile(XbmcCopierValidNoFiltersConfigFilePath);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.AreEqual(0, settings.Filters.Length);
        }
    }
}
