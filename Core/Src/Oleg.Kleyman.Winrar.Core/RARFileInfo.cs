using System;

namespace Oleg.Kleyman.Winrar.Core
{
    public class RARFileInfo
    {
        public string FileName;
        public bool ContinuedFromPrevious;
        public bool ContinuedOnNext;
        public bool IsDirectory;
        public long PackedSize;
        public long UnpackedSize;
        public int HostOS;
        public long FileCRC;
        public DateTime FileTime;
        public int VersionToUnpack;
        public int Method;
        public int FileAttributes;
        public long BytesExtracted;

        public double PercentComplete
        {
            get
            {
                if (UnpackedSize != 0)
                {
                    return ((BytesExtracted / (double)UnpackedSize) * 100.0);
                }

                return 0;
            }
        }
    }
}