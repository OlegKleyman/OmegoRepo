using System.Configuration;

namespace Oleg.Kleyman.Core
{
    public interface IConfigurationElement
    {
        [ConfigurationProperty("key", IsDefaultCollection = false, IsKey = true, IsRequired = true)]
        string Key { get; }
    }
}