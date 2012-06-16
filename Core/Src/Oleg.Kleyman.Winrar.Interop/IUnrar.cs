using System;

namespace Oleg.Kleyman.Winrar.Interop
{
    public interface IUnrar
    {
// ReSharper disable InconsistentNaming
        IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);
        uint RARCloseArchive(IntPtr hArcData);
        uint RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData);
        uint RARProcessFileW(IntPtr handle, int operation, string destPath, string destName);
        void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData);
// ReSharper restore InconsistentNaming
    }
}