using System.Collections.ObjectModel;
using System.IO;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents an archivee.
    /// </summary>
    public interface IArchive
    {
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
        /// <returns>The extracted members.</returns>
        IFileSystemMember[] Extract(string destination);
    }
}