using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Oleg.Kleyman.Core.Linq
{
    /// <summary>
    ///     <see cref="SecureString" /> extension methods.
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        ///     Converts a <see cref="SecureString" /> object to a managed <see cref="string" />.
        /// </summary>
        /// <param name="secureString"></param>
        /// <returns></returns>
        public static string ToUnsecureString(this SecureString secureString)
        {
            if (secureString == null)
            {
                const string secureStringParamName = "secureString";
                throw new ArgumentNullException(secureStringParamName);
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}