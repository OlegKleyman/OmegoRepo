namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents an archive reader.
    /// </summary>
    public interface IArchiveReader
    {
        /// <summary>
        /// Reads the next archive member in the Archive.
        /// </summary>
        /// <returns>An ArchiveMember.</returns>
        ArchiveMember Read();

        /// <summary>
        /// Gets the status of the Archive.
        /// </summary>
        RarStatus Status { get; }
    }
}