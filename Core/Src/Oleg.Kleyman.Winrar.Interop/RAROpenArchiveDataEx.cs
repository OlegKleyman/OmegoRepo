using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Winrar.Interop
{
    /// <summary>
    ///     Represents the unmanaged RAROpenArchiveDataEx struct.
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
        
        /// <summary>
        /// Represents the UserData member of the unrar.dll. Must be private to limit pointer manipulation.
        /// </summary>
        private IntPtr _userData;

        /// <summary>
        /// Gets or sets user data.
        /// </summary>
        /// Cannot use auto property because there must be a known data member to marshal to unmanaged code.
        public IntPtr UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] 
        public uint[] Reserved;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _userData = IntPtr.Zero; //releases the pointer handle
        }
    }
}