using System;
using System.Runtime.InteropServices;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class Unrar
    {
        [DllImport(@"C:\test\unrar.dll")]
        private static extern IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);

        public static Core.Archive Open(string archivePath)
        {
            var handle = IntPtr.Zero;
            var openData = new RAROpenArchiveDataEx();
            openData.Initialize();
            openData.ArcName = archivePath + "\0";
            openData.OpenMode = OpenMode.Extract;
            openData.CmtBuf = null;
            openData.CmtBufSize = 0;
            handle = RAROpenArchiveEx(ref openData);

            return new Core.Archive(handle);
        }
    }
}