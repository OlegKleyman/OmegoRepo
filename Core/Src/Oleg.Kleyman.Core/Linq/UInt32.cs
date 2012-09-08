using System;
using System.Runtime.InteropServices;

namespace Oleg.Kleyman.Core.Linq
{
    public static class UInt32
    {
        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int DosDateTimeToFileTime(ushort dateValue, ushort timeValue, out long fileTime);

        /// <summary>
        ///   Converts a DOS date and time to a managed <see cref="DateTime" /> .
        /// </summary>
        /// <param name="source"> The DOS date and time representation </param>
        /// <returns> A <see cref="DateTime" /> . </returns>
        public static DateTime ToDate(this uint source)
        {
            var dateValue = (ushort) ((source & 0xFFFF0000) >> 16);
            var timeValue = (ushort) (source & 0xFFFF);
            long fileTime;
            DosDateTimeToFileTime(dateValue, timeValue, out fileTime);
            var date = DateTime.FromFileTime(fileTime);

            return date;
        }

        /// <summary>
        ///   Joins two integers together into a long.
        /// </summary>
        /// <param name="source"> The right half of the number. </param>
        /// <param name="left"> The left half of a number </param>
        /// <returns> A <see cref="long" /> of the combined integers. </returns>
        public static long JoinWithLeft(this uint source, uint left)
        {
            var leftMoved = (long) left << 32;
            var joinedResult = leftMoved | source;
            return joinedResult;
        }

        /// <summary>
        ///   Joins two integers together into a long.
        /// </summary>
        /// <param name="source"> The left half of the number. </param>
        /// <param name="right"> The right half of the number. </param>
        /// <returns> A <see cref="long" /> of the combined integers. </returns>
        public static long JoinWithRight(this uint source, uint right)
        {
            var leftMoved = (long) source << 32;
            var joinedResult = leftMoved | right;
            return joinedResult;
        }
    }
}