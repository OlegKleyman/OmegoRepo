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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>Needed for runtime to initialize configuration.</remarks>
        private SingleValueConfigurationElementCollection() : base()
        {
            
        }

        #region Overrides of ConfigurationElementCollection

        /// <summary>
        /// Gets the key of the specified element.
        /// </summary>
        /// <param name="element">The element to get the key of.</param>
        /// <returns>Returns the same singleton object irrelevent of the argument used.</returns>
        /// <remarks>
        /// Since the class is for single value collections then a key is irrelevant as such this method will always 
        /// return the same object of type Singleton.
        /// </remarks>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Gets whether or not an exception is thrown if multiple ConfigurationElements have the same key.
        /// </summary>
        /// <remarks>
        /// Returns false.
        /// </remarks>
        protected override bool ThrowOnDuplicate
        {
            get { return false; }
        }
    }
}