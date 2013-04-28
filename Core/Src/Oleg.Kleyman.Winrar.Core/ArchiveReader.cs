using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class ArchiveReader : IArchiveReader
    {
        public IUnrarWrapper Wrapper { get; private set; }

        /// <summary>
        /// Initializes a <see cref="ArchiveReader"/> object.
        /// </summary>
        /// <param name="wrapper">The <see cref="IUnrarWrapper"/> to use for operations.</param>
        private ArchiveReader(IUnrarWrapper wrapper)
        {
            Wrapper = wrapper;
        }

        #region Implementation of IArchiveReader

        /// <summary>
        ///     Reads the next file in the archive.
        /// </summary>
        /// <returns> The next Archive Member. </returns>
        /// <exception cref="UnrarException">Thrown when the header data of the archive is unable to be read.</exception>
        public ArchiveMember Read()
        {
            var member = Wrapper.GetNextMember(IntPtr.Zero);
            return member;
        }

        /// <summary>
        ///     Gets the status of the Archive.
        /// </summary>
        public RarStatus Status { get; private set; }

        #endregion
        
        /// <summary>
        /// Retrieves a <see cref="IArchiveReader"/> object.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns>A <see cref="IArchiveReader"/> object.</returns>
        public static IArchiveReader Execute(IUnrarWrapper wrapper)
        {
            if (wrapper == null)
            {
                const string wrapperParamName = "wrapper";
                throw new ArgumentNullException(wrapperParamName);
            }
            return new ArchiveReader(wrapper);
        }
    }
}