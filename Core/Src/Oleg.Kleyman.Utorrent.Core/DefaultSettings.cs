using System;
using Oleg.Kleyman.Utorrent.Core.Properties;

namespace Oleg.Kleyman.Utorrent.Core
{
    public class DefaultSettings : ISettingsProvider
    {
        #region Implementation of ISettingsProvider

        public Uri Url
        {
            get { return new Uri(Settings.Default.Url); }
        }

        public string Username
        {
            get { return Settings.Default.Username; }
        }

        public string Password
        {
            get { return Settings.Default.Password; }
        }

        #endregion
    }
}