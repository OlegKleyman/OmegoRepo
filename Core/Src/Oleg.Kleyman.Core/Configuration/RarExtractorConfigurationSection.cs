using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core.Configuration
{
    public class RarExtractorConfigurationSection : SingleValueConfigurationSection, IRarExtractorSettings
    {
        private const string UNRAR_PATH_PROPERTY_NAME = "unrarPath";
        private static readonly IEnumerable<KeyValuePair<string, object>> __defaultProperties;

        static RarExtractorConfigurationSection()
        {
            __defaultProperties = new KeyValuePair<string, object>[] { };
        }

        public RarExtractorConfigurationSection()
            : base(__defaultProperties)
        {

        }

        public RarExtractorConfigurationSection(IEnumerable<KeyValuePair<string, object>> values)
            : base(values)
        {

        }

        public static IRarExtractorSettings Default
        {
            get { throw new System.NotImplementedException(); }
        }

        [ConfigurationProperty(UNRAR_PATH_PROPERTY_NAME, IsDefaultCollection = false, IsKey = false, IsRequired = false)]
        public override string Value
        {
            get
            {
                return (string) base[UNRAR_PATH_PROPERTY_NAME];
            }
        }

        #region Implementation of IRarExtractorSettings

        public string UnrarPath
        {
            get { return Value; }
        }

        #endregion
    }
}