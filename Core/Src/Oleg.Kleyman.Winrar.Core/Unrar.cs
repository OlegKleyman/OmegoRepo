using System;
using System.Collections.Generic;
using System.IO;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class Unrar : IUnrar
    {
        public IUnrarHandle Handle { get; set; }
        public IFileSystem FileSystem { get; set; }

        public Unrar(IUnrarHandle handle, IFileSystem fileSystem)
        {
            Handle = handle;
            FileSystem = fileSystem;
        }

        public IArchiveReader ExecuteReader()
        {
            var reader = new ArchiveReader(Handle);
            reader.ValidateHandle();
            return reader;
        }

        public FileSystemInfo Extract(string destinationPath)
        {
            RARHeaderDataEx headerData;
            var result = Handle.UnrarDll.RARReadHeaderEx(Handle.Handle, out headerData);
            if (result != 0)
            {
                const string unableToReadHeaderData = "Unable to read header data.";
                throw new UnrarException(unableToReadHeaderData, (RarStatus)result);
            }

            result = Handle.UnrarDll.RARProcessFileW(Handle.Handle, 2, destinationPath, null);
            if (result != 0)
            {
                const string unableToExtractFileMessage = "Unable to extract file.";
                throw new UnrarException(unableToExtractFileMessage, (RarStatus)result);
            }
            return null;
        }
    }
}