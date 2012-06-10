using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RAROpenArchiveDataEx
    {
        public void Initialize()
        {
            CmtBuf = new string((char)0, 65536);
            CmtBufSize = 65536;
            Reserved = new uint[32];
        }

        [MarshalAs(UnmanagedType.LPStr)]
        public string ArcName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ArcNameW;
        public uint OpenMode;
        public uint OpenResult;
        [MarshalAs(UnmanagedType.LPStr)]
        public string CmtBuf;
        public uint CmtBufSize;
        public uint CmtSize;
        public uint CmtState;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint[] Reserved;
    }
}