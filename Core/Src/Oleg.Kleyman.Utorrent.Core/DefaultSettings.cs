using System;
using Oleg.Kleyman.Utorrent.Core.Properties;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents UTorrent settings
    /// </summary>
    public class DefaultSettings : ISettingsProvider
    {
        #region Implementation of ISettingsProvider

        /// <summary>
        /// Gets the UTorrent URL
        /// </summary>
        public Uri Url
        {
            get { return new Uri(Settings.Default.Url); }
        }

        /// <summary>
        /// Gets the UTorrent username.
        /// </summary>
        public string Username
        {
            get { return Settings.Default.Username; }
        }

        /// <summary>
        /// Gets the UTorrent password.
        /// </summary>
        public string Password
        {
            get { return Settings.Default.Password; }
        }

        #endregion
    }
}