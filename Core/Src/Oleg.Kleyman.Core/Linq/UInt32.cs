using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Oleg.Kleyman.Core.Linq
{
    public static class UInt32
    {
        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int DosDateTimeToFileTime(ushort dateValue, ushort timeValue, out long fileTime);

        public static DateTime ToDate(this uint source)
        {
            var dateValue = (ushort)((source & 0xFFFF0000) >> 16);
            var timeValue = (ushort)(source & 0xFFFF);
            long fileTime;
            DosDateTimeToFileTime(dateValue, timeValue, out fileTime);
            var date = DateTime.FromFileTime(fileTime);
            
            return date;
        }

        public static long JoinWithLeft(this uint source, uint left)
        {
            long leftMoved = (long)left << 32;
            var joinedResult = leftMoved | source;
            return joinedResult;
        }

        public static long JoinWithRight(this uint source, uint right)
        {
            long leftMoved = (long)source << 32;
            var joinedResult = leftMoved | right;
            return joinedResult;
        }
    }
}
