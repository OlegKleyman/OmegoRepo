using System;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents UTorrent settings.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets the UTorrent URL
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Gets the UTorrent username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets the UTorrent password.
        /// </summary>
        string Password { get; }
    }
}