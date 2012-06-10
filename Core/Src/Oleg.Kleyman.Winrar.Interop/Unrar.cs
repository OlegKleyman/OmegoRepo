using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    public class Unrar
    {
        [DllImport(@"C:\test\unrar.dll")]
        private static extern IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);

        public static Archive Open(string archivePath)
        {
            var handle = IntPtr.Zero;
            var openData = new RAROpenArchiveDataEx();
            openData.Initialize();
            openData.ArcName = archivePath + "\0";
            openData.OpenMode = (uint)OpenMode.Extract;
            openData.CmtBuf = null;
            openData.CmtBufSize = 0;
            handle = RAROpenArchiveEx(ref openData);

            return new Archive(handle);
        }
    }
}