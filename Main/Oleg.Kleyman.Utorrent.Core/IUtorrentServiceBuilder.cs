namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents a utorrent service builder.
    /// </summary>
    public interface IUtorrentServiceBuilder
    {
        /// <summary>
        /// Gets the utorrent service.
        /// </summary>
        /// <returns>A <see cref="IUtorrentService"/> object.</returns>
        IUtorrentService GetService();
    }
}