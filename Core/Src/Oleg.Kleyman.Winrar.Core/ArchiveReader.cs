using System;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class ArchiveReader : IArchiveReader
    {
        public IUnrarHandle Handle { get; private set; }
        private RARHeaderDataEx _headerData;

        internal ArchiveReader(IUnrarHandle handle)
        {
            Handle = handle;
        }

        #region Implementation of IArchiveReader

        /// <summary>
        /// Reads the next file in the archive.
        /// </summary>
        /// <returns>The next Archive Member.</returns>
        public ArchiveMember Read()
        {
            Status = SetHeaderDataAndProcessFile();
            return (ArchiveMember)_headerData;
        }

        private RarStatus SetHeaderDataAndProcessFile()
        {
            var status = Handle.UnrarDll.RARReadHeaderEx(Handle.Handle, out _headerData);
            ValidateRarStatus((RarStatus)status);

            Handle.UnrarDll.RARProcessFileW(Handle.Handle, 0, null, null);

            return (RarStatus)status;
        }

        private void ValidateRarStatus(RarStatus status)
        {
            if(status != RarStatus.Success && status != RarStatus.EndOfArchive)
            {
                const string unableToReadHeaderDataMessage = "Unable to read header data.";
                throw new UnrarException(unableToReadHeaderDataMessage, status);
            }
        }

        public RarStatus Status { get; private set; }

        #endregion

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