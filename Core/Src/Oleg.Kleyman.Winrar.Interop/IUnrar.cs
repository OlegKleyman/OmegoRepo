using System;

namespace Oleg.Kleyman.Winrar.Interop
{
    public interface IUnrar
    {
// ReSharper disable InconsistentNaming
        IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx archiveData);
        int RARCloseArchive(IntPtr hArcData);
        RarStatus RARReadHeaderEx(IntPtr handle, ref RARHeaderDataEx headerData);
        int RARProcessFile(IntPtr hArcData, int operation, string destPath, string destName);
        int RARProcessFileW(IntPtr handle, int operation, string destPath, string destName);
// ReSharper restore InconsistentNaming
    }
}