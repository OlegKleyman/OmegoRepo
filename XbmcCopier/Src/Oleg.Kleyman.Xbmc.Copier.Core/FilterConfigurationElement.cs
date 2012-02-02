using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    /// <summary>
    /// Represents XBMC Copier onfiguration element
    /// </summary>
    public class FilterConfigurationElement : ConfigurationElement
    {
        private const string FILTER_CONFIG_ATTRIBUTE_NAME = "filter";

        /// <summary>
        /// Creates filter element based on keys and values of an IDictionary
        /// </summary>
        /// <param name="values">The property names and values to set</param>
        public FilterConfigurationElement(IEnumerable<KeyValuePair<string, object>> values) : this()
        {
            AddProperties(values);
        }

        internal FilterConfigurationElement() { }

        /// <summary>
        /// Gets the value of filter
        /// </summary>
        [ConfigurationProperty(FILTER_CONFIG_ATTRIBUTE_NAME, IsDefaultCollection = false, IsKey = false, IsRequired = false)]
        public string Value { get { return (string)base[FILTER_CONFIG_ATTRIBUTE_NAME]; } }

        private void AddProperties(IEnumerable<KeyValuePair<string, object>> values)
        {
            foreach(var keyValue in values)
            {
                var property = new ConfigurationProperty(keyValue.Key, keyValue.Value.GetType());
                Properties.Add(property);
                SetPropertyValue(property, keyValue.Value, false);
            }
        }
    }
}