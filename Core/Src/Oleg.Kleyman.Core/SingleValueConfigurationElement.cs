using System;
using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents XBMC Copier onfiguration element
    /// </summary>
    public class SingleValueConfigurationElement : ConfigurationElement
    {
        private long Salt { get; set; }
        private const string VALUE_CONFIG_ATTRIBUTE_NAME = "value";

        /// <summary>
        /// Creates filter element based on keys and values of an IDictionary
        /// </summary>
        /// <param name="values">The property names and values to set</param>
        /// <param name="salt"> </param>
        public SingleValueConfigurationElement(IEnumerable<KeyValuePair<string, object>> values) : this()
        {
            AddProperties(values);
        }

        internal SingleValueConfigurationElement()
        {
        }

        /// <summary>
        /// Gets the value of filter
        /// </summary>
        [ConfigurationProperty(VALUE_CONFIG_ATTRIBUTE_NAME, IsDefaultCollection = false, IsKey = false, IsRequired = false)]
        public string Value { get { return (string)base[VALUE_CONFIG_ATTRIBUTE_NAME]; } }

        private void AddProperties(IEnumerable<KeyValuePair<string, object>> values)
        {
            if(values == null)
            {
                return;
            }

            foreach(var keyValue in values)
            {
                var property = new ConfigurationProperty(keyValue.Key, keyValue.Value.GetType());
                Properties.Add(property);
                SetPropertyValue(property, keyValue.Value, false);
            }
        }
    }
}