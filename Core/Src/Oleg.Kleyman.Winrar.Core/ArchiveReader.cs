using System;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class ArchiveReader : IArchiveReader
    {
        private RARHeaderDataEx _headerData;

        internal ArchiveReader(IUnrarHandle handle)
        {
            Handle = handle;
        }

        #region Implementation of IArchiveReader

        /// <summary>
        ///   Reads the next file in the archive.
        /// </summary>
        /// <returns> The next Archive Member. </returns>
        /// <exception cref="UnrarException">Thrown when the header data of the archive is unable to be read.</exception>
        public ArchiveMember Read()
        {
            Status = SetHeaderDataAndProcessFile();
            var member = Status == RarStatus.EndOfArchive ? null : (ArchiveMember) _headerData;
            return member;
        }

        /// <summary>
        ///   Gets the status of the Archive.
        /// </summary>
        public RarStatus Status { get; private set; }

        private RarStatus SetHeaderDataAndProcessFile()
        {
            var status = Handle.UnrarDll.RARReadHeaderEx(Handle.Handle, out _headerData);
            ValidateRarStatus((RarStatus) status);

            Handle.UnrarDll.RARProcessFileW(Handle.Handle, (int) ArchiveMemberOperation.Skip, null, null);

            return (RarStatus) status;
        }

        private void ValidateRarStatus(RarStatus status)
        {
            if (status != RarStatus.Success && status != RarStatus.EndOfArchive)
            {
                const string unableToReadHeaderDataMessage = "Unable to read header data.";
                throw new UnrarException(unableToReadHeaderDataMessage, status);
            }
        }

        #endregion

        /// <summary>
        ///   Gets the <see cref="IUnrarHandle" /> that's used for operations.
        /// </summary>
        public IUnrarHandle Handle { get; private set; }

        internal void ValidateHandle()
        {
            ValidatePrerequisites();
        }

        private void ValidatePrerequisites()
        {
            if (!Handle.IsOpen)
            {
                const string handleIsNotOpenMessage = "Handle must be open.";
                throw new InvalidOperationException(handleIsNotOpenMessage);
            }

            if (Handle.Mode == OpenMode.Extract)
            {
                const string modeMustBeListMessage = "Handle mode must be OpenMode.List.";
                throw new InvalidOperationException(modeMustBeListMessage);
            }
        }
    }
}