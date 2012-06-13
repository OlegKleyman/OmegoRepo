using System;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public interface IUnrarHandle : IDisposable
    {
        /// <summary>
        /// Gets or Sets the UnrarDll for unrar operations.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        IUnrar UnrarDll { get; set; }

        /// <summary>
        /// Gets or sets the RarFilePath
        /// </summary>
        string RarFilePath { get; set; }

        /// <summary>
        /// Gets whether the handle to the rar archive is open.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Opens handle to the rar Archive.
        /// </summary>
        /// <returns>The opened archive.</returns>
        Archive OpenArchive();

        /// <summary>
        /// Closes the handle to the rar Archive.
        /// </summary>
        void Close();
    }
}