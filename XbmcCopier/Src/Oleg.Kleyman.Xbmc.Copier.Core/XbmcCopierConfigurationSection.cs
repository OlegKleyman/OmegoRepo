using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using Oleg.Kleyman.Core.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class XbmcCopierConfigurationSection : ConfigurationSection, ISettingsProvider
    {
        private readonly object _syncRoot;
        private ICollection<string> _filters;
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

        [ConfigurationProperty(UNRAR_PATH_PROPERTY_NAME)]
        string ISettingsProvider.UnrarPath
        {
            get { return base[UNRAR_PATH_PROPERTY_NAME] as string; }
        }

        [ConfigurationProperty(TV_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        string ISettingsProvider.TvPath
        {
            get { return base[TV_PATH_PROPERTY_NAME] as string; }
        }

        [ConfigurationProperty(MOVIE_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false)]
        string ISettingsProvider.MoviesPath
        {
            get { return base[MOVIE_PATH_PROPERTY_NAME] as string; }
        }

        ICollection<string> ISettingsProvider.Filters
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
            }
        }

        private void AddFilters()
        {
            EnsureFilterElementsAreNotNull();
            _filters = new Collection<string>();
            _filterElements.ForEach(element => _filters.Add(((SingleValueConfigurationElement)element).Value));
        }

        static XbmcCopierConfigurationSection()
        {
            __configurationInstance = GetConfigurationnInstance();
        }

        private static XbmcCopierConfigurationSection GetConfigurationnInstance()
        {
            const string configurationSectionName = "XbmcCopierConfiguration";
            var configurationSection = (XbmcCopierConfigurationSection)ConfigurationManager.GetSection(configurationSectionName);
            return configurationSection;
        }

        public static ISettingsProvider Settings
        {
            get { return __configurationInstance; }
        }
    }
}