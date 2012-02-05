using System.Configuration;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class XbmcCopierConfigurationSection : ConfigurationSection
    {
        public static ISettingsProvider Settings
        {
            get { return null; }
        }
    }
}