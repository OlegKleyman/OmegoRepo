using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core.Configuration
{
    public class RarExtractorConfigurationSection : SingleValueConfigurationSection, IRarExtractorSettings
    {
        private const string UNRAR_PATH_PROPERTY_NAME = "unrarPath";
        private static readonly IEnumerable<KeyValuePair<string, object>> __defaultProperties;
        private static IRarExtractorSettings __defaultSettings;
        private static object __syncLock;

        static RarExtractorConfigurationSection()
        {
            __defaultProperties = new KeyValuePair<string, object>[] { };
            __syncLock = new object();
        }

        private static IRarExtractorSettings GetExtractor()
        {
            var factory = new ConfigurationSectionFactory<RarExtractorConfigurationSection>();
            const string sectionName = "rarExtractorConfiguration";
            var settings = factory.GetConfigurationBySectionName(sectionName);
            return settings;
        }

        public RarExtractorConfigurationSection()
            : base()
        {

        }

        public RarExtractorConfigurationSection(IEnumerable<KeyValuePair<string, object>> values)
            : base(values)
        {

        }

        public static IRarExtractorSettings Default
        {
            get
            {
                lock (__syncLock)
                {

                    return __defaultSettings ?? (__defaultSettings = GetExtractor());
                }
            }
        }

        [ConfigurationProperty(UNRAR_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false, IsRequired = true)]
        public override string Value
        {
            get
            {
                return base[UNRAR_PATH_PROPERTY_NAME] as string;
            }
        }

        #region Implementation of IRarExtractorSettings

        string IRarExtractorSettings.UnrarPath
        {
            get { return Value; }
        }

        #endregion
    }
}