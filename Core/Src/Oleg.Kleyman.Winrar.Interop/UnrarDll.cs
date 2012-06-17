using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    public class UnrarDll : IUnrar
    {
        [DllImport(@"C:\test\unrar.dll")]
        private static extern IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx openArchiveData);
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

        /// <summary>
        /// Archive handle must be open to use this method. If it's closed then it fails hard without a
        /// managed exception.
        /// </summary>
        /// <param name="hArcData">The address of the handle of the archive to close.</param>
        /// <returns>Returns 0 if success. If failed returns 17.</returns>
        uint IUnrar.RARCloseArchive(IntPtr hArcData)
        {
            return RARCloseArchive(hArcData);
        }

        IntPtr IUnrar.RAROpenArchiveEx(ref RAROpenArchiveDataEx openArchiveData)
        {
            return RAROpenArchiveEx(ref openArchiveData);
        }

        uint IUnrar.RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData)
        {
            return RARReadHeaderEx(handle, out headerData);
        }
    }
}