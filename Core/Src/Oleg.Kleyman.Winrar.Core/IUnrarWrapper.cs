using System;
using System.Collections.Generic;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents a wrapper for the Unrar Dll
    /// </summary>
    public interface IUnrarWrapper
    {
        /// <summary>
        /// Opens an archive for operations.
        /// </summary>
        /// <param name="archivePath">The path to the archive.</param>
        /// <param name="mode">The mode to open the archive under.</param>
        /// <returns>A handle to the archive.</returns>
        IntPtr Open(string archivePath, OpenMode mode);

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle">The handle to the archive.</param>
        /// <returns>The status of the close operation.</returns>
        RarStatus Close(IntPtr handle);

        /// <summary>
        /// Gets the files and folders inside the archive.
        /// </summary>
        /// <param name="handle">The handle to the archive.</param>
        /// <returns>The archive members.</returns>
        IEnumerable<ArchiveMember> GetFiles(IntPtr handle);

        /// <summary>
        /// Etract the files in the archive.
        /// </summary>
        /// <param name="factory">The <see cref="IFileSystemMemberFactory"/> to get the extracted files from.</param>
        /// <param name="handle">The handle to the archive.</param>
        /// <param name="destinationPath">The destination folder to extract to.</param>
        /// <returns>The extracted files.</returns>
        IEnumerable<IFileSystemMember> ExtractAll(IFileSystemMemberFactory factory, IntPtr handle, string destinationPath);
    }
}