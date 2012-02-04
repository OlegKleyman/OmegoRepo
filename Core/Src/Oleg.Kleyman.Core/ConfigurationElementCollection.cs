using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core
{
    public class ConfigurationElementCollection<T> : SingleValueConfigurationElementCollection<T> where T : ConfigurationElement, IConfigurationElement
    {
        /// <summary>
        /// Constructions ConfigurationElementCollection with a range of ConfigurationElements.
        /// </summary>
        /// <param name="elements">ConfigurationElements to create the ConfigurationElementCollection object with.</param>
        public ConfigurationElementCollection(IEnumerable<T> elements) : base(elements) { }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).Key;
        }

        /// <summary>
        /// Gets ConfigurationElement in the collection by key.
        /// </summary>
        /// <param name="key">The key of the ConfigurationElement</param>
        /// <returns>Returns ConfigurationElement with the key specified in the argument.</returns>
        public new T this[string key]
        {
            get
            {
                return (T)BaseGet(key);
            }
        }
    }
}