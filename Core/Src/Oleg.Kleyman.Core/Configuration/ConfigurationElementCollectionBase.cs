using System;
using System.Collections.Generic;
using System.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core.Configuration
{
    /// <summary>
    ///   Represents a collection that holds ConfigurationElements.
    /// </summary>
    /// <typeparam name="T"> The type of the ConfigurationElement. </typeparam>
    public abstract class ConfigurationElementCollectionBase<T> : ConfigurationElementCollection
        where T : ConfigurationElement
    {
        /// <summary>
        ///   Constructions ConfigurationElementCollection with a range of ConfigurationElements.
        /// </summary>
        /// <param name="elements"> ConfigurationElements to create the ConfigurationElementCollection object with. </param>
        /// <exception cref="ArgumentNullException">Thrown when the elements argument is null.</exception>
        protected ConfigurationElementCollectionBase(IEnumerable<T> elements) : this()
        {
            if (elements == null)
            {
                const string elementsParamName = "elements";
                throw new ArgumentNullException(elementsParamName);
            }
            AddElements(elements);
        }

        /// <summary>
        ///   Default constructor
        /// </summary>
        protected ConfigurationElementCollectionBase()
        {
        }

        /// <summary>
        ///   Gets a ConfigurationElement at the desired index.
        /// </summary>
        /// <param name="index"> The index to retrieve the ConfigurationElement at. </param>
        /// <returns> Returns the ConfigurationElement at the desired index. </returns>
        public T this[int index]
        {
            get { return (T) BaseGet(index); }
        }

        private void AddElements(IEnumerable<T> elements)
        {
            elements.ForEach(BaseAdd);
        }

        /// <summary>
        ///   Creates an instance of a configuration element using the default constructor regardless of access level.
        /// </summary>
        /// <remarks>
        ///   Use wisely. This method can be dangerous when misused.
        /// </remarks>
        /// <returns> Returns an instance of a ConfigurationElement. </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement) Activator.CreateInstance(typeof (T), true);
        }
    }
}