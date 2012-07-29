using System;
using Oleg.Kleyman.Core.Linq;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class ArchiveMember
    {
        /// <summary>
        /// Returns archive member flags.
        /// </summary>
        public ArchiveMemberFlags Flags { get; internal set; }

        /// <summary>
        /// Gets the unpacked size of the file in bytes.
        /// </summary>
        public long UnpackedSize { get; internal set; }

        /// <summary>
        /// Gets the packed size of the file in bytes.
        /// </summary>
        public long PackedSize { get; internal set; }

        /// <summary>
        /// Gets the last modification date of the file.
        /// </summary>
        public DateTime LastModificationDate { get; internal set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the archive volume name that the file is in.
        /// </summary>
        public string Volume { get; internal set; }

        public static explicit operator ArchiveMember(RARHeaderDataEx headerData)
        {
            var modifiedDate = GetLastModifiedDate(headerData);
            var unpackedSize = JoinSizes(headerData.UnpSize, headerData.UnpSizeHigh);
            var packedSize = JoinSizes(headerData.PackSize, headerData.PackSizeHigh);
            var archiveMemberFlags = GetArchiveMemberFlags(headerData.Flags);

            var archiveFile = new ArchiveMember();
            archiveFile.Name = headerData.FileNameW;
            archiveFile.Volume = headerData.ArcNameW;
            archiveFile.LastModificationDate = modifiedDate;
            archiveFile.UnpackedSize = unpackedSize;
            archiveFile.PackedSize = packedSize;
            archiveFile.Flags = archiveMemberFlags;

            return archiveFile;
        }

        private static ArchiveMemberFlags GetArchiveMemberFlags(uint flags)
        {
            ArchiveMemberFlags flagsResult;
            if (flags == 0 || flags > 0x10)
            {
                const int largeValueFlagMask = 0xE0;
                flagsResult = (ArchiveMemberFlags)(flags & largeValueFlagMask); //Larger values need to be masked.
            }
            else
            {
                flagsResult = (ArchiveMemberFlags) flags;
            }

            return flagsResult;
        }


        private static DateTime GetLastModifiedDate(RARHeaderDataEx headerData)
        {
            var date = headerData.FileTime.ToDate();
            date = date.AddHours(4); //This is needed because the time comes back 4 hours behind what it should be.
            return date;
        }

        private static long JoinSizes(uint low, uint high)
        {
            long unpackedSize = high != default(uint) ? low.JoinWithLeft(high) : low;

            return unpackedSize;
        }
    }
}