using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public sealed class XbmcCopierConfigurationSection : ConfigurationSectionBase, ISettingsProvider
    {
        private const string UNRAR_PATH_PROPERTY_NAME = "unrarPath";
        private const string TV_PATH_PROPERTY_NAME = "tvPath";
        private const string MOVIE_PATH_PROPERTY_NAME = "moviePath";
        private const string MOVIE_FILTERS_PROPERTY_NAME = "movieFilters";
        private const string TV_FILTERS_PROPERTY_NAME = "tvFilters";
        private const string FILTER_PROPERTY_NAME = "filter";
        private const string CONFIGURATION_SECTION_NAME = "XbmcCopierConfiguration";
        private static readonly object __syncRoot;
        private static ISettingsProvider __singletonSettingsInstance;
        private readonly object _syncRoot;
        private SingleValueConfigurationElementCollection<SingleValueConfigurationSection> _movieFilterElements;
        private Regex[] _movieFilters;
        private SingleValueConfigurationElementCollection<SingleValueConfigurationSection> _tvFilterElements;
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
        [ConfigurationCollection(typeof(SingleValueConfigurationElementCollection<SingleValueConfigurationSection>),
            AddItemName = FILTER_PROPERTY_NAME)]
        private SingleValueConfigurationElementCollection<SingleValueConfigurationSection> MovieFilterElements
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
        [ConfigurationCollection(typeof(SingleValueConfigurationElementCollection<SingleValueConfigurationSection>),
            AddItemName = FILTER_PROPERTY_NAME)]
        private SingleValueConfigurationElementCollection<SingleValueConfigurationSection> TvFilterElements
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
                    if (__singletonSettingsInstance == null)
                    {
                        __singletonSettingsInstance = GetConfigurationSection();
                    }

                    return __singletonSettingsInstance;
                }
            }
        }

        private static ISettingsProvider GetConfigurationSection()
        {
            var factory = new ConfigurationSectionFactory<XbmcCopierConfigurationSection>();
            return factory.GetConfigurationBySectionName(CONFIGURATION_SECTION_NAME);
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
            ref SingleValueConfigurationElementCollection<SingleValueConfigurationSection> filterElements,
            string propertyName)
        {
            if (filterElements == null)
            {
                filterElements =
                    (SingleValueConfigurationElementCollection<SingleValueConfigurationSection>)
                    base[propertyName];
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
    }
}