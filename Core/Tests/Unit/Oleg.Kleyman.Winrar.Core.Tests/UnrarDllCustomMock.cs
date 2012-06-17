using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    public class UnrarDllCustomMock : IUnrar
    {
        public RAROpenArchiveDataEx OpenData { get; set; }
        public uint ReturnUintValue { get; set; }
        public IntPtr ReturnIntPtrValue { get; set; }
        public RARHeaderDataEx HeaderData { get; set; }

        #region Implementation of IUnrar

        public IntPtr RAROpenArchiveEx(ref RAROpenArchiveDataEx openArchiveData)
        {
            openArchiveData = OpenData;
            return ReturnIntPtrValue;
        }

        public uint RARCloseArchive(IntPtr hArcData)
        {
            return ReturnUintValue;
        }

        public uint RARReadHeaderEx(IntPtr handle, out RARHeaderDataEx headerData)
        {
            headerData = HeaderData;
            return ReturnUintValue;
        }

        public uint RARProcessFileW(IntPtr handle, int operation, string destPath, string destName)
        {
            return ReturnUintValue;
        }

        public void RARSetCallback(IntPtr hArcData, CallbackProc callback, IntPtr userData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
