using System.Collections.Generic;
using System.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core
{
    public abstract class ConfigurationElementBase : ConfigurationElement
    {
        protected ConfigurationElementBase(IEnumerable<KeyValuePair<string, object>> values) : this()
        {
            CreatePropertiesWithValue(values);
        }

        protected ConfigurationElementBase()
        {
        }

        /// <summary>
        /// Creates a range of properties.
        /// </summary>
        /// <param name="values">The property names and values to create.</param>
        protected void CreatePropertiesWithValue(IEnumerable<KeyValuePair<string, object>> values)
        {
            if(values == null)
            {
                return;
            }

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