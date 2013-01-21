using System.Configuration;

namespace Oleg.Kleyman.Core.Configuration
{
    /// <summary>
    ///     Provides property that returns the unique key for a ConfigurationElement.
    /// </summary>
    public interface IConfigurationElement
    {
        /// <summary>
        ///     Gets the unique key of a ConfigurationElement.
        /// </summary>
        [ConfigurationProperty("key", IsDefaultCollection = false, IsKey = true, IsRequired = true)]
        string Key { get; }
    }
}