using System.Collections.Generic;
using System.Configuration;

namespace Oleg.Kleyman.Core.Configuration
{
    public class SingleValueConfigurationElementCollection<T> : ConfigurationElementCollectionBase<T>
        where T : ConfigurationElement
    {
        /// <summary>
        /// Constructions ConfigurationElementCollection with a range of ConfigurationElements.
        /// </summary>
        /// <param name="elements">ConfigurationElements to create the ConfigurationElementCollection object with.</param>
        public SingleValueConfigurationElementCollection(IEnumerable<T> elements) : base(elements)
        {
        }

        #region Overrides of ConfigurationElementCollection

        /// <summary>
        /// Gets the key of the specified element.
        /// </summary>
        /// <param name="element">The element to get the key of.</param>
        /// <returns>Returns the same singleton object irrelevent of the argument used.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return Singleton.Instance;
        }

        #endregion
    }
}