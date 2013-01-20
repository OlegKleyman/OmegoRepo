using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Core.Linq
{
    /// <summary>
    /// Represents methods in external code.
    /// </summary>
    public static class NativeMethods
    {
        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern int DosDateTimeToFileTime(ushort dateValue, ushort timeValue, out long fileTime);
    }
}