using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core.Tests
{
    public class MockConfigurationElement : ConfigurationElementBase, IConfigurationElement
    {
        private const string VALUE_CONFIG_ATTRIBUTE_NAME = "value";
        private const string KEY_CONFIG_ATTRIBUTE_NAME = "key";

        public MockConfigurationElement(IEnumerable<KeyValuePair<string, object>> values) : base(values)
        {
        }

        [ConfigurationProperty(VALUE_CONFIG_ATTRIBUTE_NAME, IsDefaultCollection = false, IsKey = false, IsRequired = true)]
        public string Value { get { return (string)base[VALUE_CONFIG_ATTRIBUTE_NAME]; } }

        #region Implementation of IConfigurationElement

        public string Key
        {
            get { return (string)base[KEY_CONFIG_ATTRIBUTE_NAME]; }
        }

        #endregion
    }
}