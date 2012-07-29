using System;

namespace Oleg.Kleyman.Winrar.Interop
{
    public interface IUnrarDll
    {
// ReSharper disable InconsistentNaming
        IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx openArchiveData);
        uint RARCloseArchive(IntPtr hArcData);
        uint RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData);
        uint RARProcessFileW(IntPtr handle, int operation, string destPath, string destName);
        void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData);
// ReSharper restore InconsistentNaming
    }
}