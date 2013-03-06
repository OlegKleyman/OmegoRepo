using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public sealed class UnrarHandle : IUnrarHandle
    {
        private OpenMode _mode;
        private IUnrarWrapper _wrapper;

        /// <summary>
        ///     Creates the UnrarHandle object.
        /// </summary>
        /// <param name="wrapper"></param>
        public UnrarHandle(IUnrarWrapper wrapper)
        {
            _wrapper = wrapper;
            IsOpen = false;
        }

        /// <summary>
        ///     Creates the UnrarHandle object.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="rarFilePath"> The path to the rar archive to use. </param>
        public UnrarHandle(IUnrarWrapper wrapper, string rarFilePath)
            : this(wrapper)
        {
            RarFilePath = rarFilePath;
        }

        #region Implementation of IDisposable

        /// <summary>
        ///     Disposes the UnrarHandle object.
        /// </summary>
        public void Dispose()
        {
            if (IsOpen)
            {
                Close();
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IUnrarHandle Members

        public IntPtr Handle { get; private set; }

        /// <summary>
        ///     Gets or sets the open mode for the handle.
        /// </summary>
        public OpenMode Mode
        {
            get { return _mode; }
            set
            {
                if (IsOpen)
                {
                    const string connectionIsOpenMessage = "Connection must be closed to change the mode.";
                    throw new InvalidOperationException(connectionIsOpenMessage);
                }
                _mode = value;
            }
        }

        /// <summary>
        ///     Gets or Sets the Wrapper for unrarDll operations.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the handle is still open.</exception>
        public IUnrarWrapper Wrapper
        {
            get { return _wrapper; }
            set
            {
                if (IsOpen)
                {
                    const string unrarWrapperCannotBeChangedMessage =
                        "Wrapper cannot be changed if the unrar handle is still open.";
                    throw new InvalidOperationException(unrarWrapperCannotBeChangedMessage);
                }
                _wrapper = value;
            }
        }

        /// <summary>
        ///     Gets or sets the RarFilePath
        /// </summary>
        public string RarFilePath { get; set; }

        /// <summary>
        ///     Gets whether the handle to the rar archive is open.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        ///     Closes the handle to the rar archive.
        /// </summary>
        /// <exception cref="UnrarException">Thrown when the handle cannot be closed.</exception>
        public void Close()
        {
            if (IsOpen)
            {
                _wrapper.Close(Handle);
                
                IsOpen = false;
            }
            else
            {
                const string unrarHandleIsNotOpenMessage = "Unrar handle is not open.";
                throw new InvalidOperationException(unrarHandleIsNotOpenMessage);
            }
        }

        /// <summary>
        ///     Opens the handle to the archive
        /// </summary>
        /// <exception cref="UnrarException">Thrown when the archive was unable to be opened.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the handle is already open, the Wrapper or RarFilePath properties are null or an empty string.</exception>
        public void Open()
        {
            ValidatePrerequisites();
            
            Handle = _wrapper.Open(RarFilePath, Mode);
            GC.ReRegisterForFinalize(this);

            IsOpen = true;
        }

        #endregion

        [NoCoverage]
        ~UnrarHandle()
        {
            Dispose();
        }

        private void ValidatePrerequisites()
        {
            if (IsOpen)
            {
                const string objectIsOpenMessage = "Object is open and must be closed to open again.";
                throw new InvalidOperationException(objectIsOpenMessage);
            }
            if (Wrapper == null)
            {
                const string unrarDllMustBeSetMessage = "Wrapper must be set.";
                throw new InvalidOperationException(unrarDllMustBeSetMessage);
            }

            if (string.IsNullOrEmpty(RarFilePath))
            {
                const string rarFilePathMustBeSetMessage = "RarFilePath must be set.";
                throw new InvalidOperationException(rarFilePathMustBeSetMessage);
            }
        }
    }
}