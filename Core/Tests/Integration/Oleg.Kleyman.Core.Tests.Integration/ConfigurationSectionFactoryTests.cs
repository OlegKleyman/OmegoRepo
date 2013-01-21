using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class ConfigurationSectionFactoryTests
    {
        private const string SECTION_NAME = "rarExtractorConfiguration";
        private string ValidConfigFilePath { get; set; }
        private string InvalidNoConfigurationSectionFilePath { get; set; }
        private ConfigurationSectionFactory<RarExtractorConfigurationSection> ConfigurationSectionFactory { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            const string xbmcCopierValidConfigFileName = @"valid.config";
            const string xbmcCopierInvalidNoConfigurationSectionFileName =
                @"InvalidNoConfigurationSection.config";
            const string filePath = @"TestConfigs";

            ValidConfigFilePath = Path.Combine(currentDirectory, filePath, xbmcCopierValidConfigFileName);
            InvalidNoConfigurationSectionFilePath = Path.Combine(currentDirectory, filePath,
                                                                 xbmcCopierInvalidNoConfigurationSectionFileName);
            ConfigurationSectionFactory = new ConfigurationSectionFactory<RarExtractorConfigurationSection>();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentException),
            ExpectedExceptionName = "System.ArgumentException",
            ExpectedMessage = "Cannot be an empty string.\r\nParameter name: sectionName",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationConfigurationSectionArgumentEmptyTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = InvalidNoConfigurationSectionFilePath;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            ConfigurationSectionFactory.GetConfigurationSectionByConfiguration(configuration, string.Empty);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentNullException),
            ExpectedExceptionName = "System.ArgumentNullException",
            ExpectedMessage = "Value cannot be null.\r\nParameter name: configuration",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationConfigurationSectionConfigurationNullTest()
        {
            ConfigurationSectionFactory.GetConfigurationSectionByConfiguration(null, SECTION_NAME);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ConfigurationErrorsException),
            ExpectedExceptionName = "System.Configuration.ConfigurationErrorsException",
            ExpectedMessage = "rarExtractorConfiguration configuration section not found.",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationConfigurationSectionNotFoundTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = InvalidNoConfigurationSectionFilePath;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConfigurationSectionFactory.GetConfigurationSectionByConfiguration(configuration, SECTION_NAME);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ConfigurationErrorsException),
            ExpectedExceptionName = "System.Configuration.ConfigurationErrorsException",
            ExpectedMessage = "Configuration file not found (testFile.config)",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFileFileDoesNotExistTest()
        {
            ConfigurationSectionFactory.GetSettingsByConfigurationFile("testFile.config", null);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentException),
            ExpectedExceptionName = "System.ArgumentException",
            ExpectedMessage = "Value cannot be empty.\r\nParameter name: configurationFilePath",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFilePathIsEmptyTest()
        {
            ConfigurationSectionFactory.GetSettingsByConfigurationFile(string.Empty, SECTION_NAME);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentNullException),
            ExpectedExceptionName = "System.ArgumentNullException",
            ExpectedMessage = "Value cannot be null.\r\nParameter name: configurationFilePath",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationFilePathIsNullTest()
        {
            ConfigurationSectionFactory.GetSettingsByConfigurationFile(null, null);
        }

        [Test]
        public void GetByConfigurationFileTest()
        {
            var settings =
                (IRarExtractorSettings)
                ConfigurationSectionFactory.GetSettingsByConfigurationFile(ValidConfigFilePath, SECTION_NAME);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentNullException),
            ExpectedExceptionName = "System.ArgumentNullException",
            ExpectedMessage = "Value cannot be null.\r\nParameter name: sectionName",
            MatchType = MessageMatch.Exact)]
        public void GetByConfigurationSectionNameIsNullTest()
        {
            ConfigurationSectionFactory.GetSettingsByConfigurationFile(InvalidNoConfigurationSectionFilePath, null);
        }

        [Test]
        public void GetByConfigurationTest()
        {
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = ValidConfigFilePath;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var settings =
                (IRarExtractorSettings)
                ConfigurationSectionFactory.GetConfigurationSectionByConfiguration(configuration, SECTION_NAME);
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
        }
    }
}