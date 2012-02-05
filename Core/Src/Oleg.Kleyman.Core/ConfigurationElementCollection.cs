using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents a collection that holds ConfigurationElements.
    /// </summary>
    /// <typeparam name="T">The type of the ConfigurationElement.</typeparam>
    public class ConfigurationElementCollection<T> : ConfigurationElementCollectionBase<T> where T : ConfigurationElement, IConfigurationElement
    {
        /// <summary>
        /// Constructions ConfigurationElementCollection with a range of ConfigurationElements.
        /// </summary>
        /// <param name="elements">ConfigurationElements to create the ConfigurationElementCollection object with.</param>
        /// <example>
        /// var propertyNameValues = new Dictionary&lt;string, object&gt;
        ///                              {
        ///                                 {"value", "test"},
        ///                                 {"key", "someKey"}
        ///                              };
        /// var element = new SingleValueConfigurationElement(propertyNameValues);
        /// var collection = new ConfigurationElementCollection&lt;SingleValueConfigurationElement&gt;(new[] { element });
        /// //use object
        /// </example>
        public ConfigurationElementCollection(IEnumerable<T> elements) : base(elements) { }
        
        /// <summary>
        /// Gets the key of the specified element.
        /// </summary>
        /// <param name="element">The element to get the key of.</param>
        /// <returns>Returns the key of the element specified.</returns>
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