using System;

namespace Oleg.Kleyman.Winrar.Interop
{
    public interface IUnrar
    {
// ReSharper disable InconsistentNaming
        IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);
        RarStatus RARCloseArchive(IntPtr hArcData);
        RarStatus RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData);
        int RARProcessFile(IntPtr hArcData, int operation, string destPath, string destName);
        RarStatus RARProcessFileW(IntPtr handle, int operation, string destPath, string destName);
        void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData);
// ReSharper restore InconsistentNaming
    }
}