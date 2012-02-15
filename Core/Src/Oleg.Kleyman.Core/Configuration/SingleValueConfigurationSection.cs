using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core.Configuration
{
    /// <summary>
    ///   Represents a configuration section with only one attribute.
    /// </summary>
    public class SingleValueConfigurationSection : ConfigurationSectionBase
    {
        private const string VALUE_CONFIG_ATTRIBUTE_NAME = "value";

        /// <summary>
        ///   Creates configuration section based on keys and values of an IDictionary.
        /// </summary>
        /// <param name="values"> The property names and values to set. </param>
        /// <example>
        ///   var propertyNameValues = new Dictionary&lt;string, object&gt; { {"value", "test"}, {"key", "someKey"} }; var element = new SingleValueConfigurationSection(propertyNameValues); //use object
        /// </example>
        public SingleValueConfigurationSection(IEnumerable<KeyValuePair<string, object>> values) : base(values)
        {
        }

        /// <summary>
        ///   Default constructor
        /// </summary>
        /// <remarks>
        ///   Needed for runtime to initialize configuration.
        /// </remarks>
// ReSharper disable UnusedMember.Local
        private SingleValueConfigurationSection()
// ReSharper restore UnusedMember.Local
        {
        }

        /// <summary>
        ///   Gets the value the value attribute.
        /// </summary>
        [ConfigurationProperty(VALUE_CONFIG_ATTRIBUTE_NAME, IsDefaultCollection = false, IsKey = false,
            IsRequired = false)]
        public virtual string Value
        {
            get { return (string) base[VALUE_CONFIG_ATTRIBUTE_NAME]; }
        }
    }
}