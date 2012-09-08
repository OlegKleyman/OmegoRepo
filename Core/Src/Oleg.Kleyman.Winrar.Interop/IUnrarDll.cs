using System;

namespace Oleg.Kleyman.Winrar.Interop
{
    /// <summary>
    ///   An interface to the unrar unmanaged dll.
    /// </summary>
    public interface IUnrarDll
    {
// ReSharper disable InconsistentNaming
        /// <summary>
        ///   Opens a handle to the target archive.
        /// </summary>
        /// <param name="openArchiveData"> The information data of the target archive to open. </param>
        /// <returns> A <see cref="IntPtr" /> handle to the archive. </returns>
        IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx openArchiveData);

        /// <summary>
        ///   Closes the handle to the archive. Archive handle must be open to use this method. If it's closed then it fails hard without a managed exception.
        /// </summary>
        /// <param name="hArcData"> The archive handle. </param>
        /// <returns> The success status of the handle closing. </returns>
        /// <remarks>
        ///   0 if success. If failed returns 17.
        /// </remarks>
        uint RARCloseArchive(IntPtr hArcData);

        /// <summary>
        ///   Reads the next header of the archive.
        /// </summary>
        /// <param name="handle"> An open handle to the target archive. </param>
        /// <param name="headerData"> The header data to be populated on sucess. </param>
        /// <returns> The status of the header read operation. </returns>
        uint RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData);

        /// <summary>
        ///   Processes an archive member.
        /// </summary>
        /// <param name="handle"> An open handle to the target archive. </param>
        /// <param name="operation"> The operation to use when processing the member. </param>
        /// <param name="destPath"> The destination path of the directory to extract to. If null then the current directory will be used. </param>
        /// <param name="destName"> The full path to where the member will be extracted. </param>
        /// <returns> The sucess or error identifier of the operation. </returns>
        /// <remarks>
        ///   If destName parameter is defined the it overrides the destPath parameter.
        /// </remarks>
        uint RARProcessFileW(IntPtr handle, int operation, string destPath, string destName);

        /// <summary>
        ///   Sets the callback method of unrar operations.
        /// </summary>
        /// <param name="hArcData"> The handle of the archive. </param>
        /// <param name="callback"> The callback method. </param>
        /// <param name="userData"> The user defined value which is passed into the callback method </param>
        void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData);

// ReSharper restore InconsistentNaming
    }
}