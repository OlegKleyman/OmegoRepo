using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    public class UnrarDll : IUnrar
    {
        [DllImport(@"C:\test\unrar.dll")]
        private static extern IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern uint RARCloseArchive(IntPtr hArcData);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern uint RARReadHeaderEx(IntPtr hArcData, out RARHeaderDataEx headerData);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern uint RARProcessFileW(IntPtr hArcData, int operation,
            [MarshalAs(UnmanagedType.LPWStr)] string destPath,
            [MarshalAs(UnmanagedType.LPWStr)]string destName);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData);

        void IUnrar.RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData)
        {
            RARSetCallback(hArcData, callback, userData);
        }

        uint IUnrar.RARProcessFileW(IntPtr handle, int operation, string destPath, string destName)
        {
            return RARProcessFileW(handle, operation, destPath, destName);
        }

        uint IUnrar.RARCloseArchive(IntPtr hArcData)
        {
            return RARCloseArchive(hArcData);
        }

        IntPtr IUnrar.RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData)
        {
            return RAROpenArchiveEx(ref archiveData);
        }

        uint IUnrar.RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData)
        {
            return RARReadHeaderEx(handle, out headerData);
        }
    }
}