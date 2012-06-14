using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Oleg.Kleyman.Core.Linq;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarHandle : IUnrarHandle
    {
        private IUnrar _unrarDll;
        private RARHeaderDataEx _headerData;

        /// <summary>
        /// Gets or Sets the UnrarDll for unrar operations.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        public IUnrar UnrarDll
        {
            get { return _unrarDll; }
            set
            {
                if (IsOpen)
                {
                    const string unrarDllCannotBeChangedMessage = "UnrarDll cannot be changed if the unrar handle is still open.";
                    throw new InvalidOperationException(unrarDllCannotBeChangedMessage);
                }
                _unrarDll = value;
            }
        }

        /// <summary>
        /// Gets or sets the RarFilePath
        /// </summary>
        public string RarFilePath { get; set; }

        /// <summary>
        /// Gets whether the handle to the rar archive is open.
        /// </summary>
        public bool IsOpen { get; private set; }

        private IntPtr Handle { get; set; }

        /// <summary>
        /// Creates the UnrarHandle object.
        /// </summary>
        /// <param name="unrarDll">The <see cref="IUnrar"/> object to interface commands to.</param>
        public UnrarHandle(IUnrar unrarDll)
        {
            UnrarDll = unrarDll;
            _headerData = new RARHeaderDataEx();
            IsOpen = false;
        }

        /// <summary>
        /// Creates the UnrarHandle object.
        /// </summary>
        /// <param name="unrarDll">The <see cref="IUnrar"/> object to interface commands to.</param>
        /// <param name="rarFilePath">The path to the rar archive to use.</param>
        public UnrarHandle(IUnrar unrarDll, string rarFilePath)
            : this(unrarDll)
        {
            RarFilePath = rarFilePath;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Disposes the UnrarHandle object.
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

        ~UnrarHandle()
        {
            Dispose();
        }

        /// <summary>
        /// Opens handle to the rar Archive.
        /// </summary>
        /// <returns>The opened archive.</returns>
        public Archive OpenArchive()
        {
            ValidatePrerequisites();
            var openData = new RAROpenArchiveDataEx
                               {
                                   ArcName = RarFilePath,
                                   OpenMode = OpenMode.List
                               };
            Handle = UnrarDll.RAROpenArchiveEx(ref openData);
            GC.ReRegisterForFinalize(this);
            if (Handle == default(IntPtr))
            {
                throw new ApplicationException("Unable to open archive.");
            }

            IsOpen = true;
            var archive = GetArchive();
            return archive;
        }

        private Archive GetArchive()
        {
            var archive = new Archive(this)
                              {
                                  FilePath = RarFilePath
                              };
            FillArchive(archive);
            return archive;
        }

        private void FillArchive(Archive archive)
        {
            var status = SetHeaderDataAndProcessFile();

            switch (status)
            {
                case RarStatus.Success:
                    SetArchiveData(archive);
                    FillArchive(archive);
                    break;
                case RarStatus.EndOfArchive:
                    break;
                default:
                    //TODO: throw custom exception
                    break;
            }
        }

        private void SetArchiveData(Archive archive)
        {
            var unpackedFile = GetUnpackedFile();
            OnFileProcessed(unpackedFile);
            archive.Files.Add(unpackedFile);
        }

        protected void OnFileProcessed(UnpackedFile unpackedFile)
        {
            if(FileProcessed != null)
            {
                FileProcessed(this, new UnrarFileProcessedEventArgs(unpackedFile));
            }
        }

        private UnpackedFile GetUnpackedFile()
        {
            var modifiedDate = GetLastModifiedDate();
            
            var file = new UnpackedFile
                           {
                               Name = _headerData.FileNameW,
                               //Volume = _headerData.ArcNameW,
                               //LastModificationDate = modifiedDate,

                           };
            return file;
        }

        private DateTime GetLastModifiedDate()
        {
            var date = _headerData.FileTime.ToDate();
            date = date.AddHours(4); //This is needed because the time comes back 4 hours behind what it should be.
            return date;
        }

        private RarStatus SetHeaderDataAndProcessFile()
        {
            var status = UnrarDll.RARReadHeaderEx(Handle, out _headerData);
            UnrarDll.RARProcessFileW(Handle, 0, null, null);
            return status;
        }

        private void ValidatePrerequisites()
        {
            if (UnrarDll == null)
            {
                const string unrarDllMustBeSetMessage = "UnrarDll must be set.";
                throw new InvalidOperationException(unrarDllMustBeSetMessage);
            }

            if (RarFilePath == null)
            {
                const string rarFilePathMustBeSetMessage = "RarFilePath must be set.";
                throw new InvalidOperationException(rarFilePathMustBeSetMessage);
            }
        }

        /// <summary>
        /// Closes the handle to the rar archive.
        /// </summary>
        public void Close()
        {
            if (IsOpen)
            {
                var status = UnrarDll.RARCloseArchive(Handle);
                if (status != RarStatus.Success)
                {
                    //TODO: throw rar exception
                }
                IsOpen = false;
            }
            else
            {
                const string unrarHandleIsNotOpenMessage = "Unrar handle is not open.";
                throw new InvalidOperationException(unrarHandleIsNotOpenMessage);
            }
        }

        public event EventHandler<UnrarFileProcessedEventArgs> FileProcessed;
    }
}