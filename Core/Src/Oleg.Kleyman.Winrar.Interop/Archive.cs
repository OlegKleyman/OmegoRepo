using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    public class Archive
    {
        [DllImport(@"C:\test\unrar.dll")]
        private static extern int RARCloseArchive(IntPtr hArcData);

        [DllImport(@"C:\test\unrar.dll")]
        private static extern RarStatus RARReadHeaderEx(IntPtr hArcData, ref RARHeaderDataEx headerData);

        private IntPtr _handle;

        internal Archive(IntPtr handle)
        {
            _handle = handle;
        }

        /// <summary>
        /// Get filename list
        /// </summary>
        /// <returns>A collection of file names in the archive.</returns>
        public ICollection<string> GetFiles()
        {
            // Throw exception if archive not open
            if (_handle == IntPtr.Zero)
            {
                const string archiveIsNotOpen = "Archive is not open.";
                throw new IOException(archiveIsNotOpen);
            }




            var files = new Collection<string>();
            return null;
        }

        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int DosDateTimeToFileTime(ushort dateValue, ushort timeValue, out UInt64 fileTime);

        public bool ReadHeader()
        {
            var header = new RARHeaderDataEx();
            header.Initialize();

            var result = RARReadHeaderEx(_handle, ref header);

            if (result == RarStatus.EndOfArchive)
            {
                return false;
            }
            if (result == RarStatus.BadData)
            {
                const string archiveDataIsCorrupt = "Archive data is corrupt.";
                throw new IOException(archiveDataIsCorrupt);
            }

            if ((header.Flags & 0x01) == 0)
            {
                var file = new RARFileInfo();

                file.FileName = header.FileNameW;

                if ((header.Flags & 0x02) != 0)
                {
                    file.ContinuedOnNext = true;
                }
                if (header.PackSizeHigh != 0)
                {
                    file.PackedSize = (header.PackSizeHigh * 0x100000000) + header.PackSize;
                }
                else
                {
                    file.PackedSize = header.PackSize;
                }

                if (header.UnpSizeHigh != 0)
                {
                    file.UnpackedSize = (header.UnpSizeHigh * 0x100000000) + header.UnpSize;
                }
                else
                {
                    file.UnpackedSize = header.UnpSize;
                }

                file.HostOS = (int)header.HostOS;
                file.FileCRC = header.FileCRC;
                var dateValue = (ushort)((header.FileTime & 0xFFFF0000) >> 16);
                var timeValue = (ushort)(header.FileTime & 0xFFFF);
                ulong fileTime;
                DosDateTimeToFileTime(dateValue, timeValue, out fileTime);
                file.FileTime = DateTime.FromFileTime((long) fileTime);
                file.VersionToUnpack = (int)header.UnpVer;
                file.Method = (int)header.Method;
                file.FileAttributes = (int)header.FileAttr;
                file.BytesExtracted = 0;
                if ((header.Flags & 0xE0) == 0xE0)
                {
                    file.IsDirectory = true;
                }
            }

            return false;
        }
        /// <summary>
        /// Close Archive
        /// </summary>
        public void Close()
        {
            RARCloseArchive(_handle);
        }
    }
}