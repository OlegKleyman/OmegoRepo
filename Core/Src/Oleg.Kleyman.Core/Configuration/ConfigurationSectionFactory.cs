using System;
using System.Configuration;
using System.IO;

namespace Oleg.Kleyman.Core.Configuration
{
    public class ConfigurationSectionFactory<T> where T : ConfigurationSection
    {
        /// <summary>
        ///   Gets a <see cref="System.Configuration.ConfigurationSection" /> by file path.
        /// </summary>
        /// <param name="configurationFilePath"> UNC path to the configuration file </param>
        /// <param name="sectionName">The name of the configuration section to load.</param>
        /// <returns> A <see cref="System.Configuration.ConfigurationSection" /> object. </returns>
        public T GetSettingsByConfigurationFile(string configurationFilePath, string sectionName)
        {
            ThrowExceptionOnInvalidPathArgument(configurationFilePath);
            
            ThrowExceptionOnInvalidArguments(sectionName);
            
            var fileMap = new ExeConfigurationFileMap
                              {
                                  ExeConfigFilename = configurationFilePath
                              };

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return GetConfigurationSectionByConfiguration(configuration, sectionName);
        }

        private static void ThrowExceptionOnInvalidPathArgument(string configurationFilePath)
        {
            if (string.IsNullOrEmpty(configurationFilePath))
            {
                const string configurationFilePathParamName = "configurationFilePath";
                throw new ArgumentNullException(configurationFilePathParamName);
            }

            if (!File.Exists(configurationFilePath))
            {
                const string configurationFileNotFoundMessage = "Configuration file not found";
                throw new ConfigurationErrorsException(configurationFileNotFoundMessage, configurationFilePath, 0);
            }
        }

        /// <summary>
        ///   Gets a <see cref="System.Configuration.ConfigurationSection" /> by <see cref="Oleg.Kleyman.Core.Configuration" /> .
        /// </summary>
        /// <param name="configuration"> The <see cref="Oleg.Kleyman.Core.Configuration" /> object containing the <see cref="System.Configuration.ConfigurationSection"/> to load.</param>
        /// <param name="sectionName">The name of the configuration section to load.</param>
        /// <returns> A <see cref="System.Configuration.ConfigurationSection" /> object. </returns>
        public T GetConfigurationSectionByConfiguration(System.Configuration.Configuration configuration, string sectionName)
        {
            ThrowExceptionOnInvalidArguments(configuration, sectionName);

            var section = configuration.GetSection(sectionName);

            ThrowExceptionOnConfigurationSectionNull(sectionName, section);

            return (T)section;
        }

        private static void ThrowExceptionOnInvalidArguments(System.Configuration.Configuration configuration, string sectionName)
        {
            if (configuration == null)
            {
                const string configurationParamName = "configuration";
                throw new ArgumentNullException(configurationParamName);
            }

            ThrowExceptionOnInvalidArguments(sectionName);
        }

        private static void ThrowExceptionOnInvalidArguments(string sectionName)
        {
            const string sectionNameParamName = "sectionName";

            if (sectionName == null)
            {
                throw new ArgumentNullException(sectionNameParamName);
            }

            if (sectionName == string.Empty)
            {
                const string argumentCannotBeAnEmptyStringMessage = "Cannot be an empty string.";
                throw new ArgumentException(argumentCannotBeAnEmptyStringMessage, sectionNameParamName);
            }
        }

        private static void ThrowExceptionOnConfigurationSectionNull(string sectionName, ConfigurationSection section)
        {
            if (section == null)
            {
                var xbmcCopierConfigurationSectionNotFoundMessage = string.Format("{0} configuration section not found.",
                                                                                  sectionName);
                throw new ConfigurationErrorsException(xbmcCopierConfigurationSectionNotFoundMessage);
            }
        }

        /// <summary>
        ///   Gets a <see cref="System.Configuration.ConfigurationSection" /> by <see cref="Oleg.Kleyman.Core.Configuration" /> .
        /// </summary>
        /// <param name="sectionName">The name of the configuration section to load.</param>
        /// <returns> A <see cref="System.Configuration.ConfigurationSection" /> object. </returns>
        public T GetConfigurationBySectionName(string sectionName)
        {
            ThrowExceptionOnInvalidArguments(sectionName);

            var section = ConfigurationManager.GetSection(sectionName);
            ThrowExceptionOnConfigurationSectionNull(sectionName, section as ConfigurationSection);

            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}