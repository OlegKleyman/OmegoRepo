namespace Oleg.Kleyman.Winrar.Core
{
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

        /// <summary>
        /// Gets the <see cref="IUnrarHandle"/> used by the reader.
        /// </summary>
        IUnrarHandle Handle { get; }
    }
}