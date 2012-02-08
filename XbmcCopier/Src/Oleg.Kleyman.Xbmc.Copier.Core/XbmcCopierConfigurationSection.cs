using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Xml;
using Oleg.Kleyman.Core.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public sealed class XbmcCopierConfigurationSection : ConfigurationSection, ISettingsProvider
    {
        private readonly object _syncRoot;
        private string[] _filters;
        private static readonly XbmcCopierConfigurationSection __configurationInstance;
        private SingleValueConfigurationElementCollection<SingleValueConfigurationElement> _filterElements;

        private XbmcCopierConfigurationSection()
        {
            _syncRoot = new object();
        }

        private const string UNRAR_PATH_PROPERTY_NAME = "unrarPath";
        private const string TV_PATH_PROPERTY_NAME = "tvPath";
        private const string MOVIE_PATH_PROPERTY_NAME = "moviePath";
        private const string FILTERS_PROPERTY_NAME = "filters";
        private const string FILTER_PROPERTY_NAME = "filter";
        private const string CONFIGURATION_SECTION_NAME = "XbmcCopierConfiguration";

        /// <summary>
        /// Gets the unrar.exe location from the config.
        /// </summary>
        [ConfigurationProperty(UNRAR_PATH_PROPERTY_NAME, IsRequired = true)]
        string ISettingsProvider.UnrarPath
        {
            get { return base[UNRAR_PATH_PROPERTY_NAME] as string; }
        }

        /// <summary>
        /// Gets the TV path location from the config.
        /// </summary>
        [ConfigurationProperty(TV_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        string ISettingsProvider.TvPath
        {
            get { return base[TV_PATH_PROPERTY_NAME] as string; }
        }

        /// <summary>
        /// Gets the movie path location from the config.
        /// </summary>
        [ConfigurationProperty(MOVIE_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        string ISettingsProvider.MoviesPath
        {
            get { return base[MOVIE_PATH_PROPERTY_NAME] as string; }
        }

        /// <summary>
        /// Gets the filters to use from the config file.
        /// </summary>
        string[] ISettingsProvider.Filters
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_filters == null)
                    {
                        AddFilters();
                    }
                }

                return _filters;
            }
        }

        [ConfigurationProperty(FILTERS_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        [ConfigurationCollection(typeof(SingleValueConfigurationElementCollection<SingleValueConfigurationElement>), AddItemName = FILTER_PROPERTY_NAME)]
        private SingleValueConfigurationElementCollection<SingleValueConfigurationElement> FilterElements
        {
            get
            {
                lock (_syncRoot)
                {
                    EnsureFilterElementsAreNotNull();
                }
                return _filterElements;
            }
        }

        private void EnsureFilterElementsAreNotNull()
        {
            if (_filterElements == null)
            {
                _filterElements =
                    (SingleValueConfigurationElementCollection<SingleValueConfigurationElement>)
                    base[FILTERS_PROPERTY_NAME];
                if (_filterElements == null)
                {
                    //It should never get to this point
                    throw new ConfigurationErrorsException("Internal configuration error: Filters null");
                }
            }
        }

        private void AddFilters()
        {
            _filters = new string[FilterElements.Count];
            for(int index = 0; index <FilterElements.Count;index++)
            {
                _filters[index] = FilterElements[index].Value;
            }
        }

        static XbmcCopierConfigurationSection()
        {
            __configurationInstance = GetConfigurationnInstance();
        }

        private static XbmcCopierConfigurationSection GetConfigurationnInstance()
        {
            var configurationSection = (XbmcCopierConfigurationSection)ConfigurationManager.GetSection(CONFIGURATION_SECTION_NAME);
            return configurationSection;
        }

        /// <summary>
        /// Gets the settings for the Xbmx configuration from the config file.
        /// </summary>
        public static ISettingsProvider DefaultSettings
        {
            get { return __configurationInstance; }
        }

        public static ISettingsProvider GetSettingsByConfigurationFile(string configurationFilePath)
        {
            if(string.IsNullOrEmpty(configurationFilePath))
            {
                const string configurationFilePathParamName = "configurationFilePath";
                throw new ArgumentNullException(configurationFilePathParamName);
            }

            if (!File.Exists(configurationFilePath))
            {
                
                const string configurationFileNotFoundMessage = "Configuration file not found";
                throw new ConfigurationErrorsException(configurationFileNotFoundMessage, configurationFilePath, 0);
            }
            
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configurationFilePath;
            
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return GetSettingsByConfiguration(configuration);
        }

        public static ISettingsProvider GetSettingsByConfiguration(Configuration configuration)
        {
            var section = (ISettingsProvider) configuration.GetSection(CONFIGURATION_SECTION_NAME);
            if(section == null)
            {
                const string xbmcCopierConfigurationSectionNotFoundMessage = "XbmcCopierConfiguration configuration section not found.";
                throw new ConfigurationErrorsException(xbmcCopierConfigurationSectionNotFoundMessage);
            }

            return section;
        }
    }
}