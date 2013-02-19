using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class ArchiveReader : IArchiveReader
    {
        /// <summary>
        /// Initializes a <see cref="ArchiveReader"/> object.
        /// </summary>
        /// <param name="extractor">The <see cref="IMemberExtractor"/> used to read the archive.</param>
        private ArchiveReader(IMemberExtractor extractor)
        {
            Extractor = extractor;
        }

        #region Implementation of IArchiveReader

        /// <summary>
        ///     Reads the next file in the archive.
        /// </summary>
        /// <returns> The next Archive Member. </returns>
        /// <exception cref="UnrarException">Thrown when the header data of the archive is unable to be read.</exception>
        public ArchiveMember Read()
        {
            Status = Extractor.Extract(null);
            
            var member = Status == RarStatus.EndOfArchive ? null : Extractor.CurrentMember;
            return member;
        }

        /// <summary>
        ///     Gets the status of the Archive.
        /// </summary>
        public RarStatus Status { get; private set; }

        #endregion

        public IMemberExtractor Extractor { get; set; }

        //private void ValidatePrerequisites()
        //{
        //    if (!Handle.IsOpen)
        //    {
        //        const string handleIsNotOpenMessage = "Handle must be open.";
        //        throw new InvalidOperationException(handleIsNotOpenMessage);
        //    }

        //    if (Handle.Mode == OpenMode.Extract)
        //    {
        //        const string modeMustBeListMessage = "Handle mode must be OpenMode.List.";
        //        throw new InvalidOperationException(modeMustBeListMessage);
        //    }
        //}

        /// <summary>
        /// Retrieves a <see cref="IArchiveReader"/> object.
        /// </summary>
        /// <param name="extractor">A <see cref="IMemberExtractor"/> to use for operations.</param>
        /// <returns>A <see cref="IArchiveReader"/> object.</returns>
        public static IArchiveReader Execute(IMemberExtractor extractor)
        {
            if (extractor == null)
            {
                const string extractorParamName = "extractor";
                throw new ArgumentNullException(extractorParamName);
            }
            return new ArchiveReader(extractor);
        }
    }
}