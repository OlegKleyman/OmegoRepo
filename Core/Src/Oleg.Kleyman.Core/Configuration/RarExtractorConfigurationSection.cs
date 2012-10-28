using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core.Configuration
{
    /// <summary>
    ///   Represents a rar extractor configuration section.
    /// </summary>
    public sealed class RarExtractorConfigurationSection : SingleValueConfigurationSection, IRarExtractorSettings
    {
        private const string UNRAR_PATH_PROPERTY_NAME = "unrarPath";
        private static IRarExtractorSettings __defaultSettings;
        private static readonly object __syncLock;

        static RarExtractorConfigurationSection()
        {
            __syncLock = new object();
        }

// ReSharper disable UnusedMember.Local
        /// <summary>
        ///   Instantiates an instance of the <see cref="RarExtractorConfigurationSection" /> class.
        /// </summary>
        /// <remarks>
        ///   This constructor is needed to create an object dynamically.
        /// </remarks>
        private RarExtractorConfigurationSection()
// ReSharper restore UnusedMember.Local
        {
        }

        /// <summary>
        ///   Creates configuration section based on keys and values of an IDictionary.
        /// </summary>
        /// <param name="values"> The property names and values to set. </param>
        /// <example>
        ///   var propertyNameValues = new Dictionary&lt;string, object&gt; { {"value", "test"}, {"key", "someKey"} }; var element = new SingleValueConfigurationSection(propertyNameValues); //use object
        /// </example>
        public RarExtractorConfigurationSection(IEnumerable<KeyValuePair<string, object>> values)
            : base(values)
        {
        }

        /// <summary>
        ///   Gets the default extractor settings.
        /// </summary>
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
            get { return base[UNRAR_PATH_PROPERTY_NAME] as string; }
        }

        #region Implementation of IRarExtractorSettings

        /// <summary>
        ///   Gets the path for unrar file.
        /// </summary>
        string IRarExtractorSettings.UnrarPath
        {
            get { return Value; }
        }

        #endregion

        private static IRarExtractorSettings GetExtractor()
        {
            var factory = new ConfigurationSectionFactory<RarExtractorConfigurationSection>();
            const string sectionName = "rarExtractorConfiguration";
            var settings = factory.GetConfigurationBySectionName(sectionName);
            return settings;
        }
    }
}