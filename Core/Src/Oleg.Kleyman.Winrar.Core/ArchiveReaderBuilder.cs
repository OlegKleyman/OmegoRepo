using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents an a builder for <see cref="IArchiveReader"/>
    /// </summary>
    internal class ArchiveReaderBuilder
    {
        /// <summary>
        /// Gets the archive handle.
        /// </summary>
        public IUnrarHandle Handle { get; private set; }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="handle">The <see cref="IUnrarHandle"/> object to use for operations.</param>
        public ArchiveReaderBuilder(IUnrarHandle handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Gets the archive reader.
        /// </summary>
        /// <returns>A <see cref="IArchiveReader"/> object.</returns>
        public IArchiveReader GetReader()
        {
            
            var reader = ArchiveReader.Execute(new UnrarWrapper(new NativeMethods(), new DestinationPathBuilder(new PathBuilder())));
            return reader;
        }
    }
}