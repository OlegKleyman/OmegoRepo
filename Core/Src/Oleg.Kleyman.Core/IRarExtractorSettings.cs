namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents the settings for a <see cref="RarExtractor"/> object.
    /// </summary>
    public interface IRarExtractorSettings
    {
        /// <summary>
        /// Gets the path for unrar file.
        /// </summary>
        string UnrarPath { get; }
    }
}