using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    /// <summary>
    ///   Represents the unmanaged RAROpenArchiveDataEx struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable InconsistentNaming
    public struct RAROpenArchiveDataEx
        // ReSharper restore InconsistentNaming
        : IDisposable
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
        private IntPtr _userData;
        public IntPtr UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint[] Reserved;

        public void Dispose()
        {
            _userData = IntPtr.Zero;
        }
    }
}