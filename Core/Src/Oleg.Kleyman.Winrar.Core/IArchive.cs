using System.Collections.ObjectModel;
using System.IO;

namespace Oleg.Kleyman.Winrar.Core
{
    public interface IArchive
    {
        /// <summary>
        /// The <see cref="IUnrarHandle"/> to use for Archive operations.
        /// </summary>
        IUnrarHandle Handle { get; }

        /// <summary>
        /// Files contained in the Archive.
        /// </summary>
        ReadOnlyCollection<ArchiveMember> Files { get; }

        /// <summary>
        /// The file path to the Archive.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Extracts the contents of the archive.
        /// </summary>
        /// <param name="destination">The destination file path for extraction.</param>
        /// <returns>The destination file path.</returns>
        FileSystemInfo Extract(string destination);
    }
}