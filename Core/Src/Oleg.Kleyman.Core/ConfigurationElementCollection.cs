using System;
using System.Collections.Generic;
using System.Configuration;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core
{
    public class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement
    {
        public ConfigurationElementCollection(IEnumerable<T> elements) : this()
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

        private ConfigurationElementCollection() { }

        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement)Activator.CreateInstance(typeof(T), true);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetHashCode();
        }

        #endregion

        public T this[int index]
        {
            get { return (T)BaseGet(index); }
        }
    }
}