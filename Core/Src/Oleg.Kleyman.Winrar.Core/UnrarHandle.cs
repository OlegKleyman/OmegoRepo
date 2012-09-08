using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarHandle : IUnrarHandle
    {
        private OpenMode _mode;
        private IUnrarDll _unrarDll;

        /// <summary>
        ///   Creates the UnrarHandle object.
        /// </summary>
        /// <param name="unrarDll"> The <see cref="IUnrarDll" /> object to interface commands to. </param>
        public UnrarHandle(IUnrarDll unrarDll)
        {
            UnrarDll = unrarDll;
            IsOpen = false;
        }

        /// <summary>
        ///   Creates the UnrarHandle object.
        /// </summary>
        /// <param name="unrarDll"> The <see cref="IUnrarDll" /> object to interface commands to. </param>
        /// <param name="rarFilePath"> The path to the rar archive to use. </param>
        public UnrarHandle(IUnrarDll unrarDll, string rarFilePath)
            : this(unrarDll)
        {
            RarFilePath = rarFilePath;
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Disposes the UnrarHandle object.
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
        ///   Gets or sets the open mode for the handle.
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
        ///   Gets or Sets the UnrarDll for unrarDll operations.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the handle is still open.</exception>
        public IUnrarDll UnrarDll
        {
            get { return _unrarDll; }
            set
            {
                if (IsOpen)
                {
                    const string unrarDllCannotBeChangedMessage =
                        "UnrarDll cannot be changed if the unrar handle is still open.";
                    throw new InvalidOperationException(unrarDllCannotBeChangedMessage);
                }
                _unrarDll = value;
            }
        }

        /// <summary>
        ///   Gets or sets the RarFilePath
        /// </summary>
        public string RarFilePath { get; set; }

        /// <summary>
        ///   Gets whether the handle to the rar archive is open.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        ///   Closes the handle to the rar archive.
        /// </summary>
        /// <exception cref="UnrarException">Thrown when the handle cannot be closed.</exception>
        public void Close()
        {
            if (IsOpen)
            {
                var status = UnrarDll.RARCloseArchive(Handle);
                if (status != (uint) RarStatus.Success)
                {
                    const string unableToCloseArchiveMessage =
                        "Unable to close archive. Possibly because it's already closed.";
                    throw new UnrarException(unableToCloseArchiveMessage, (RarStatus) status);
                }
                IsOpen = false;
            }
            else
            {
                const string unrarHandleIsNotOpenMessage = "Unrar handle is not open.";
                throw new InvalidOperationException(unrarHandleIsNotOpenMessage);
            }
        }

        /// <summary>
        ///   Opens the handle to the archive
        /// </summary>
        /// <exception cref="UnrarException">Thrown when the archive was unable to be opened.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the handle is already open, the UnrarDll or RarFilePath properties are null or an empty string.</exception>
        public void Open()
        {
            ValidatePrerequisites();
            var openData = new RAROpenArchiveDataEx
                               {
                                   ArcName = RarFilePath,
                                   OpenMode = (uint) Mode
                               };

            Handle = UnrarDll.RAROpenArchiveEx(ref openData);

            GC.ReRegisterForFinalize(this);

            if (Handle == default(IntPtr))
            {
                const string unableToOpenArchiveMessage = "Unable to open archive.";
                throw new UnrarException(unableToOpenArchiveMessage, (RarStatus) openData.OpenResult);
            }

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
            if (UnrarDll == null)
            {
                const string unrarDllMustBeSetMessage = "UnrarDll must be set.";
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