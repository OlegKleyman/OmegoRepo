using System.Configuration;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class FilterConfigurationElementCollection : ConfigurationElementCollection
    {
        private FilterConfigurationElementCollection() { }
        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            throw new System.NotImplementedException();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}