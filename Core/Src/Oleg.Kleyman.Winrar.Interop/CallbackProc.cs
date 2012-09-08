using System;

namespace Oleg.Kleyman.Winrar.Interop
{
    /// <summary>
    ///   A delegate for the unrar callback.
    /// </summary>
    /// <param name="msg"> The callback message. </param>
    /// <param name="userData"> The user data associated with the callback. </param>
    /// <param name="p1"> The first parameter of the callback. </param>
    /// <param name="p2"> The second parameter of the callback. </param>
    /// <returns> </returns>
    public delegate int CallbackProc(uint msg, IntPtr userData, IntPtr p1, int p2);
}