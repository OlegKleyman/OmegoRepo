using System;
using System.Collections.Generic;

namespace Oleg.Kleyman.Winrar.Core
{
    public interface IUnrarWrapper
    {
        IntPtr Open(string archivePath, OpenMode mode);
        RarStatus Close(IntPtr handle);
        IEnumerable<ArchiveMember> GetFiles(IntPtr handle);
    }
}