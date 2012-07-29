using System;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public interface IUnrarHandle : IDisposable
    {
        /// <summary>
        /// Gets or Sets the UnrarDll for unrarDll operations.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        IUnrarDll UnrarDll { get; set; }

        /// <summary>
        /// Gets or sets the RarFilePath
        /// </summary>
        string RarFilePath { get; set; }

        /// <summary>
        /// Gets whether the handle to the rar archive is open.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Closes the handle to the rar Archive.
        /// </summary>
        void Close();

        /// <summary>
        /// Opens the archive
        /// </summary>
        void Open();

        /// <summary>
        /// Gets and sets the mode for handle communication.
        /// </summary>
        OpenMode Mode { get; set; }

        /// <summary>
        /// Gets the handle to the rar archive.
        /// </summary>
        IntPtr Handle { get; }
    }
}