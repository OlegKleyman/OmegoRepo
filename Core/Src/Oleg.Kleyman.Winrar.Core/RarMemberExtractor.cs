using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents a rar archive member extractor
    /// </summary>
    public class RarMemberExtractor : IMemberExtractor
    {
        private IUnrarHandle _handle;
        /// <summary>
        /// Gets or sets the <see cref="IUnrarHandle"/> for this object.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the property value is being set to null.</exception>
        public IUnrarHandle Handle
        {
            get { return _handle; }
            set
            {
                ThrowArgumentNullExceptionWhenPropertyIsBeingSetToNull(value);
                _handle = value;
            }
        }

        private IFileProcessor _processor;
        /// <summary>
        /// Gets or sets the <see cref="IFileProcessor"/> for this object.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the property value is being set to null.</exception>
        public IFileProcessor Processor
        {
            get { return _processor; }
            set
            {
                ThrowArgumentNullExceptionWhenPropertyIsBeingSetToNull(value);
                _processor = value;
            }
        }

        private static void ThrowArgumentNullExceptionWhenPropertyIsBeingSetToNull(object value)
        {
            if (value == null)
            {
                const string valueParamName = "value";
                throw new ArgumentNullException(valueParamName);
            }
        }

        /// <summary>
        /// Gets the current member extracted.
        /// </summary>
        public ArchiveMember CurrentMember { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="RarMemberExtractor"/> object.
        /// </summary>
        /// <param name="handle">The <see cref="IUnrarHandle"/> to use for operations.</param>
        /// <param name="processor">The <see cref="IFileProcessor"/> to use for operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when either the handle or processor argument is null.</exception>
        public RarMemberExtractor(IUnrarHandle handle, IFileProcessor processor)
        {
            if (handle == null)
            {
                const string handleParamName = "handle";
                throw new ArgumentNullException(handleParamName);
            }

            if (processor == null)
            {
                const string processorParamName = "processor";
                throw new ArgumentNullException(processorParamName);
            }

            Handle = handle;
            Processor = processor;
        }

        /// <summary>
        /// Extracts the archive member.
        /// </summary>
        /// <param name="destinationPath">The destination path to extract to.</param>
        /// <returns>The status of the extraction. Either a RarStatus.Success or RarStatus.EndOfArchive.</returns>
        /// <exception cref="UnrarException">Thrown when the member header is unable to be read.</exception>
        public RarStatus Extract(string destinationPath)
        {
            RARHeaderDataEx headerData;
            var result = (RarStatus)Handle.UnrarDll.RARReadHeaderEx(Handle.Handle, out headerData);

            if (result == RarStatus.Success)
            {
                Processor.ProcessFile(destinationPath);
            }
            else if (result != RarStatus.EndOfArchive)
            {
                const string unableToReadHeaderData = "Unable to read header data.";
                throw new UnrarException(unableToReadHeaderData, result);
            }

            CurrentMember = (ArchiveMember)headerData;
            return result;
        }
    }
}