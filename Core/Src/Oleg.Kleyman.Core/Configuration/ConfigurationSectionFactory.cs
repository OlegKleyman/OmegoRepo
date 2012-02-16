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
            ValidatePath(configurationFilePath);


            if (string.IsNullOrEmpty(sectionName))
            {
                const string sectionNameParamName = "sectionName";
                throw new ArgumentNullException(sectionNameParamName);
            }
            
            var fileMap = new ExeConfigurationFileMap
                              {
                                  ExeConfigFilename = configurationFilePath
                              };

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return GetSettingsByConfiguration(configuration, sectionName);
        }

        private static void ValidatePath(string configurationFilePath)
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
        public T GetSettingsByConfiguration(System.Configuration.Configuration configuration, string sectionName)
        {
            if (configuration == null)
            {
                const string sectionNameParamName = "configuration";
                throw new ArgumentNullException(sectionNameParamName);
            }

            if (string.IsNullOrEmpty(sectionName))
            {
                const string sectionNameParamName = "sectionName";
                throw new ArgumentNullException(sectionNameParamName);
            }

            var section = configuration.GetSection(sectionName);

            if (section == null)
            {
                var xbmcCopierConfigurationSectionNotFoundMessage = string.Format("{0} configuration section not found.", sectionName);
                throw new ConfigurationErrorsException(xbmcCopierConfigurationSectionNotFoundMessage);
            }

            return (T)section;
        }

        /// <summary>
        ///   Gets a <see cref="System.Configuration.ConfigurationSection" /> by <see cref="Oleg.Kleyman.Core.Configuration" /> .
        /// </summary>
        /// <param name="sectionName">The name of the configuration section to load.</param>
        /// <returns> A <see cref="System.Configuration.ConfigurationSection" /> object. </returns>
        public T GetConfigurationBySectionName(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                const string sectionNameParamName = "sectionName";
                throw new ArgumentNullException(sectionNameParamName);
            }

            var section = ConfigurationManager.GetSection(sectionName);
            if (section == null)
            {
                var xbmcCopierConfigurationSectionNotFoundMessage = string.Format("{0} configuration section not found.", sectionName);
                throw new ConfigurationErrorsException(xbmcCopierConfigurationSectionNotFoundMessage);
            }

            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}