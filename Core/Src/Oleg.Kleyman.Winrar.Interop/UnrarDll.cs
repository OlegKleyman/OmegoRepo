using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    public class UnrarDll : IUnrar
    {
        [DllImport(@"C:\test\unrar.dll")]
        private static extern IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern int RARCloseArchive(IntPtr hArcData);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern RarStatus RARReadHeaderEx(IntPtr hArcData, ref RARHeaderDataEx headerData);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern int RARProcessFile(IntPtr hArcData, int operation,
            [MarshalAs(UnmanagedType.LPStr)] string destPath,
            [MarshalAs(UnmanagedType.LPStr)] string destName);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern int RARProcessFileW(IntPtr hArcData, int operation,
            [MarshalAs(UnmanagedType.LPWStr)] string destPath,
            [MarshalAs(UnmanagedType.LPWStr)]string destName);
        [DllImport(@"C:\test\unrar.dll")]
        private static extern void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData);

        void IUnrar.RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData)
        {
            RARSetCallback(hArcData, callback, userData);
        }

        int IUnrar.RARProcessFileW(IntPtr handle, int operation, string destPath, string destName)
        {
            return RARProcessFileW(handle, operation, destPath, destName);
        }

        int IUnrar.RARCloseArchive(IntPtr hArcData)
        {
            return RARCloseArchive(hArcData);
        }

        IntPtr IUnrar.RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData)
        {
            return RAROpenArchiveEx(ref archiveData);
        }

        RarStatus IUnrar.RARReadHeaderEx(IntPtr handle, ref RARHeaderDataEx headerData)
        {
            return RARReadHeaderEx(handle, ref headerData);
        }

        int IUnrar.RARProcessFile(IntPtr hArcData, int operation, string destPath, string destName)
        {
            return RARProcessFile(hArcData, operation, destPath, destName);
        }
    }
}