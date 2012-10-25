using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Oleg.Kleyman.Core.Linq
{
    public static class SecureStringExtensions
    {
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
