using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class Archive
    {
        public IUnrarHandle Handle { get; internal set; }

        public Collection<UnpackedFile> Files { get; private set; }

        public string FilePath { get; internal set; }

        internal Archive(IUnrarHandle handle)
        {
            Handle = handle;
            Files = new Collection<UnpackedFile>();
        }


        public static Archive Open(IUnrar unrarDll, string filePath, OpenMode openMode)
        {
            //var archive = new Archive(unrarDll);
            return null;
        }
    }
}