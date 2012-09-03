using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    /// <summary>
    /// Represents the unmanaged RAROpenArchiveDataEx struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
// ReSharper disable InconsistentNaming
    public struct RAROpenArchiveDataEx
// ReSharper restore InconsistentNaming
    {
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
        public CallbackProc Callback;
        public IntPtr UserData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint[] Reserved;
    }
}