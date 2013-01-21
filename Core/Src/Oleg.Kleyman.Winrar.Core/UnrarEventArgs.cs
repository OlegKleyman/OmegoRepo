using System;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///     Represents the arguments for an unrar event.
    /// </summary>
    public class UnrarEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes the <see cref="UnrarEventArgs" /> object.
        /// </summary>
        /// <param name="archiveMember"> The archive member to initialize the object with. </param>
        public UnrarEventArgs(ArchiveMember archiveMember)
        {
            ArchiveMember = archiveMember;
        }

        /// <summary>
        ///     Gets the <see cref="ArchiveMember" /> associated with the event.
        /// </summary>
        public ArchiveMember ArchiveMember { get; private set; }
    }
}