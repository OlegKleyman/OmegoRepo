using System;
using System.Collections.Generic;
using System.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core
{
    public class SingleValueConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement
    {
        /// <summary>
        /// Constructions ConfigurationElementCollection with a range of ConfigurationElements.
        /// </summary>
        /// <param name="elements">ConfigurationElements to create the ConfigurationElementCollection object with.</param>
        public SingleValueConfigurationElementCollection(IEnumerable<T> elements) : this()
        {
            if(elements == null)
            {
                const string elementsParamName = "elements";
                throw new ArgumentNullException(elementsParamName);
            }
            AddElements(elements);
            
        }

        private void AddElements(IEnumerable<T> elements)
        {
            elements.ForEach(BaseAdd);
        }

        private SingleValueConfigurationElementCollection() { }

        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement)Activator.CreateInstance(typeof(T), true);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Singleton.Instance;
        }

        #endregion

        public T this[int index]
        {
            get { return (T)BaseGet(index); }
        }
    }
}