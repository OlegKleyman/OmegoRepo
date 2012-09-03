using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    /// <summary>
    /// Represents the unmanaged RARHeaderDataEx struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
// ReSharper disable InconsistentNaming
    public struct RARHeaderDataEx
// ReSharper restore InconsistentNaming
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string ArcName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string ArcNameW;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string FileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string FileNameW;
        public uint Flags;
        public uint PackSize;
        public uint PackSizeHigh;
        public uint UnpSize;
        public uint UnpSizeHigh;
        public uint HostOS;
        public uint FileCRC;
        public uint FileTime;
        public uint UnpVer;
        public uint Method;
        public uint FileAttr;
        [MarshalAs(UnmanagedType.LPStr)]
        public string CmtBuf;
        public uint CmtBufSize;
        public uint CmtSize;
        public uint CmtState;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public uint[] Reserved;
    }
}