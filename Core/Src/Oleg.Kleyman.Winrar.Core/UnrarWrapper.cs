using System;
using System.Collections.Generic;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarWrapper : IUnrarWrapper
    {
        private IUnrarDll UnrarDll { get; set; }

        public UnrarWrapper(IUnrarDll unrarDll)
        {
            UnrarDll = unrarDll;
        }

        public IntPtr Open(string archivePath, OpenMode mode)
        {
            var openData = new RAROpenArchiveDataEx
                {
                    ArcName = archivePath,
                    OpenMode = (uint)mode
                };

            var handle = UnrarDll.RAROpenArchiveEx(ref openData);
            return handle;
        }

        public RarStatus Close(IntPtr handle)
        {
            var status = UnrarDll.RARCloseArchive(handle);
            return (RarStatus)status;
        }

        public IEnumerable<ArchiveMember> GetFiles(IntPtr handle)
        {
            RARHeaderDataEx headerData;

            var members = new List<ArchiveMember>();
            
            while ((RarStatus)UnrarDll.RARReadHeaderEx(handle, out headerData) != RarStatus.EndOfArchive)
            {
                UnrarDll.RARProcessFileW(handle, (int)ArchiveMemberOperation.Extract, null, null);
                members.Add((ArchiveMember)headerData);
            }

            return members;
        }
    }
}