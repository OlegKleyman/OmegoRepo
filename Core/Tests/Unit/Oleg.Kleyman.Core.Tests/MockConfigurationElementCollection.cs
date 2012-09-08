using System;
using System.Configuration;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core.Tests
{
    internal class MockConfigurationElementCollection<T> : ConfigurationElementCollectionBase<T>
        where T : ConfigurationElement
    {
        internal ConfigurationElement CallCreateNewElement()
        {
            return CreateNewElement();
        }

        #region Overrides of ConfigurationElementCollection

        protected override object GetElementKey(ConfigurationElement element)
        {
            throw new NotImplementedException(
                "this class is a mock implementation for testing and this method is not implemented");
        }

        #endregion
    }
}