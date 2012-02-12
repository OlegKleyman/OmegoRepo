using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public sealed class XbmcCopierConfigurationSection : ConfigurationSection, ISettingsProvider
    {
        private const string UNRAR_PATH_PROPERTY_NAME = "unrarPath";
        private const string TV_PATH_PROPERTY_NAME = "tvPath";
        private const string MOVIE_PATH_PROPERTY_NAME = "moviePath";
        private const string MOVIE_FILTERS_PROPERTY_NAME = "movieFilters";
        private const string TV_FILTERS_PROPERTY_NAME = "tvFilters";
        private const string FILTER_PROPERTY_NAME = "filter";
        private const string CONFIGURATION_SECTION_NAME = "XbmcCopierConfiguration";
        private static readonly object __syncRoot;
        private static XbmcCopierConfigurationSection __configurationInstance;
        private readonly object _syncRoot;
        private SingleValueConfigurationElementCollection<SingleValueConfigurationElement> _movieFilterElements;
        private Regex[] _movieFilters;
        private SingleValueConfigurationElementCollection<SingleValueConfigurationElement> _tvFilterElements;
        private Regex[] _tvFilters;

        static XbmcCopierConfigurationSection()
        {
            __syncRoot = new object();
        }

        private XbmcCopierConfigurationSection()
        {
            _syncRoot = new object();
        }

        [ConfigurationProperty(MOVIE_FILTERS_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        [ConfigurationCollection(typeof(SingleValueConfigurationElementCollection<SingleValueConfigurationElement>),
            AddItemName = FILTER_PROPERTY_NAME)]
        private SingleValueConfigurationElementCollection<SingleValueConfigurationElement> MovieFilterElements
        {
            get
            {
                lock (_syncRoot)
                {
                    EnsureFilterElementsAreNotNull(ref _movieFilterElements, MOVIE_FILTERS_PROPERTY_NAME);
                }
                return _movieFilterElements;
            }
        }

        [ConfigurationProperty(TV_FILTERS_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        [ConfigurationCollection(typeof(SingleValueConfigurationElementCollection<SingleValueConfigurationElement>),
            AddItemName = FILTER_PROPERTY_NAME)]
        private SingleValueConfigurationElementCollection<SingleValueConfigurationElement> TvFilterElements
        {
            get
            {
                lock (_syncRoot)
                {
                    EnsureFilterElementsAreNotNull(ref _tvFilterElements, TV_FILTERS_PROPERTY_NAME);
                }
                return _tvFilterElements;
            }
        }

        /// <summary>
        ///   Gets the default <see cref="ISettingsProvider" /> for the Xbmx configuration from the config file.
        /// </summary>
        public static ISettingsProvider DefaultSettings
        {
            get
            {
                lock (__syncRoot)
                {
                    if (__configurationInstance == null)
                    {
                        __configurationInstance = GetConfigurationnInstance();
                    }

                    return __configurationInstance;
                }
            }
        }

        #region ISettingsProvider Members

        /// <summary>
        ///   Gets the unrar.exe location from the config.
        /// </summary>
        [ConfigurationProperty(UNRAR_PATH_PROPERTY_NAME, IsRequired = true)]
        string ISettingsProvider.UnrarPath
        {
            get { return base[UNRAR_PATH_PROPERTY_NAME] as string; }
        }

        /// <summary>
        ///   Gets the TV path location from the config.
        /// </summary>
        [ConfigurationProperty(TV_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        string ISettingsProvider.TvPath
        {
            get { return base[TV_PATH_PROPERTY_NAME] as string; }
        }

        /// <summary>
        ///   Gets the movie path location from the config.
        /// </summary>
        [ConfigurationProperty(MOVIE_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        string ISettingsProvider.MoviesPath
        {
            get { return base[MOVIE_PATH_PROPERTY_NAME] as string; }
        }

        /// <summary>
        ///   Gets the movie filters to use from the config file.
        /// </summary>
        Regex[] ISettingsProvider.MovieFilters
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_movieFilters == null)
                    {
                        _movieFilters = GetFilters(ReleaseType.Movie);
                    }
                }

                return _movieFilters;
            }
        }

        /// <summary>
        ///   Gets the tv filters to use from the config file.
        /// </summary>
        Regex[] ISettingsProvider.TvFilters
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_tvFilters == null)
                    {
                        _tvFilters = GetFilters(ReleaseType.Tv);
                    }
                }

                return _tvFilters;
            }
        }

        #endregion

        private void EnsureFilterElementsAreNotNull(
            ref SingleValueConfigurationElementCollection<SingleValueConfigurationElement> filterElements,
            string propertyName)
        {
            if (filterElements == null)
            {
                filterElements =
                    (SingleValueConfigurationElementCollection<SingleValueConfigurationElement>)
                    base[propertyName];
                if (filterElements == null)
                {
                    //It should never get to this point
                    throw new ConfigurationErrorsException("Internal configuration error: filterElements is null");
                }
            }
        }

        private Regex[] GetFilters(ReleaseType releaseType)
        {
            var filterElements = releaseType == ReleaseType.Movie ? MovieFilterElements : TvFilterElements;
            var filters = new Regex[filterElements.Count];
            const RegexOptions regexOptions =
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            for (var index = 0; index < filterElements.Count; index++)
            {
                filters[index] = new Regex(filterElements[index].Value, regexOptions);
            }
            return filters;
        }

        private static XbmcCopierConfigurationSection GetConfigurationnInstance()
        {
            var configurationSection =
                (XbmcCopierConfigurationSection)ConfigurationManager.GetSection(CONFIGURATION_SECTION_NAME);
            return configurationSection;
        }

        /// <summary>
        ///   Gets an <see cref="ISettingsProvider" /> settings by configuration file path.
        /// </summary>
        /// <param name="configurationFilePath"> UNC path to the configuration file </param>
        /// <returns> An <see cref="ISettingsProvider" /> object. </returns>
        public static ISettingsProvider GetSettingsByConfigurationFile(string configurationFilePath)
        {
            ValidatePath(configurationFilePath);

            var fileMap = new ExeConfigurationFileMap
                              {
                                  ExeConfigFilename = configurationFilePath
                              };

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                                                                                          ConfigurationUserLevel.None);
            return GetSettingsByConfiguration(configuration);
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
        ///   Gets an <see cref="ISettingsProvider" /> settings by <see cref="Configuration" /> .
        /// </summary>
        /// <param name="configuration"> The <see cref="Configuration" /> object containing XBMC settings </param>
        /// <returns> An <see cref="ISettingsProvider" /> object. </returns>
        public static ISettingsProvider GetSettingsByConfiguration(Configuration configuration)
        {
            var section = (ISettingsProvider)configuration.GetSection(CONFIGURATION_SECTION_NAME);
            if (section == null)
            {
                const string xbmcCopierConfigurationSectionNotFoundMessage =
                    "XbmcCopierConfiguration configuration section not found.";
                throw new ConfigurationErrorsException(xbmcCopierConfigurationSectionNotFoundMessage);
            }

            return section;
        }
    }
}