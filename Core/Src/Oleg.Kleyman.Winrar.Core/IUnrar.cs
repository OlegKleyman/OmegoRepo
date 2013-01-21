using System;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///     Represents an unrar interface.
    /// </summary>
    public interface IUnrar
    {
        /// <summary>
        ///     The <see cref="IUnrarHandle" /> to use for operations.
        /// </summary>
        IUnrarHandle Handle { get; set; }

        /// <summary>
        ///     Executes the archive reader.
        /// </summary>
        /// <returns>
        ///     An <see cref="IArchiveReader" /> .
        /// </returns>
        IArchiveReader ExecuteReader();

        /// <summary>
        ///     Extracts the archive.
        /// </summary>
        /// <param name="destinationPath"> The destination folder to extract to. If it does not exist then it will be created. </param>
        /// <returns>
        ///     A <see cref="IFileSystemMember" /> array containing the extracted members.
        /// </returns>
        IFileSystemMember[] Extract(string destinationPath);

        /// <summary>
        ///     Invoked when a compressed member is extracted.
        /// </summary>
        event EventHandler<UnrarEventArgs> MemberExtracted;
    }
}