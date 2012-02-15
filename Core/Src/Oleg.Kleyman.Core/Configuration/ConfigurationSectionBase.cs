using System;
using System.Collections.Generic;
using System.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core.Configuration
{
    /// <summary>
    ///   Represents a configuration section.
    /// </summary>
    public abstract class ConfigurationSectionBase : ConfigurationSection
    {
        /// <summary>
        ///   Creates configuration section based on keys and values of an IDictionary.
        /// </summary>
        /// <param name="values"> The property names and values to set. </param>
        protected ConfigurationSectionBase(IEnumerable<KeyValuePair<string, object>> values) : this()
        {
            if (values == null)
            {
                const string valuesParamName = "values";
                throw new ArgumentNullException(valuesParamName);
            }
            CreatePropertiesWithValue(values);
        }

        /// <summary>
        ///   Default constructor. Does nothing
        /// </summary>
        protected ConfigurationSectionBase()
        {
        }

        /// <summary>
        ///   Creates a range of properties.
        /// </summary>
        /// <param name="values"> The property names and values to create. </param>
        protected void CreatePropertiesWithValue(IEnumerable<KeyValuePair<string, object>> values)
        {
            values.ForEach(AddPropertyWithValue);
        }

        private void AddPropertyWithValue(KeyValuePair<string, object> value)
        {
            var property = new ConfigurationProperty(value.Key, value.Value.GetType());
            Properties.Add(property);
            SetPropertyValue(property, value.Value, false);
        }
    }
}