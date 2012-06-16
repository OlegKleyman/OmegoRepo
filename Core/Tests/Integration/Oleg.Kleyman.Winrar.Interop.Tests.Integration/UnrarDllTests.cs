using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Interop.Tests.Integration
{
    [TestFixture]
    public class UnrarDllTests : TestsBase
    {
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        [Test]
        public void RAROpenArchiveExTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            unrar.RARCloseArchive(handle);
        }

        [Test]
        public void RARCloseArchiveTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RARReadHeaderExTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();
            var status = unrar.RARReadHeaderEx(handle, out headerData);
            Assert.AreEqual(0, status);

            Assert.AreEqual("㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴整瑳楆敬爮牡", headerData.ArcName);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\testFile.rar", headerData.ArcNameW);
            Assert.IsNull(headerData.CmtBuf);
            Assert.AreEqual(0, headerData.CmtBufSize);
            Assert.AreEqual(0, headerData.CmtSize);
            Assert.AreEqual(0, headerData.CmtState);
            Assert.AreEqual(32, headerData.FileAttr);
            Assert.AreEqual(0, headerData.FileCRC);
            Assert.AreEqual("整瑳楆敬琮瑸", headerData.FileName);
            Assert.AreEqual("testFile.txt", headerData.FileNameW);
            Assert.AreEqual(1062684718, headerData.FileTime);
            Assert.AreEqual(36896, headerData.Flags);
            Assert.AreEqual(2, headerData.HostOS);
            Assert.AreEqual(48, headerData.Method);
            Assert.AreEqual(0, headerData.PackSize);
            Assert.AreEqual(0, headerData.PackSizeHigh);
            Assert.IsTrue(headerData.Reserved.All(target => target == 0));
            Assert.AreEqual(0, headerData.UnpSize);
            Assert.AreEqual(0, headerData.UnpSizeHigh);
            Assert.AreEqual(29, headerData.UnpVer);

            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RARProcessFileWTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            openData.OpenMode = 1;
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);

            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void MultiVolumeTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            
            var headerData = new RARHeaderDataEx();

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);

            extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile2.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);

            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        private bool _callBackSuccess;
        [Test]
        public void RARSetCallbackTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            unrar.RARSetCallback(handle, RarCallBack, Marshal.StringToCoTaskMemUni("test data"));
            var headerData = new RARHeaderDataEx();
            
            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);
            Assert.IsTrue(_callBackSuccess);
            _callBackSuccess = false;

            extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile2.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);
            Assert.IsTrue(_callBackSuccess);

            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        private void ExtractAndVerify(IUnrar unrar, IntPtr handle, string extractPath, RARHeaderDataEx headerData)
        {
            var status = unrar.RARReadHeaderEx(handle, out headerData);
            Assert.AreEqual(0, status);
            var extractStatus = unrar.RARProcessFileW(handle, 2, string.Empty, extractPath);
            Assert.AreEqual(0, extractStatus);
            Assert.AreEqual(0, status);
            Assert.IsTrue(File.Exists(extractPath), "file doesn't exist");
            File.Delete(extractPath);
        }

        [Test]
        public void RAROpenArchiveDataExCallBackTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            openData.Callback = RarCallBack;

            openData.UserData = Marshal.StringToCoTaskMemUni("test data"); ;
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);
            Assert.IsTrue(_callBackSuccess);
            _callBackSuccess = false;

            extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile2.txt.test");
            ExtractAndVerify(unrar, handle, extractPath, headerData);
            Assert.IsTrue(_callBackSuccess);

            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        private int RarCallBack(uint message, IntPtr userData, IntPtr p1, int p2)
        {
            Assert.AreEqual("test data", Marshal.PtrToStringUni(userData));
            _callBackSuccess = true;
            return 1;
        }
    }
}
