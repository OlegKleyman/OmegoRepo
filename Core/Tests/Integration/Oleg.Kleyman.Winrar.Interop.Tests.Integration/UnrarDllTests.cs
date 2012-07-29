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
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            unrarDll.RARCloseArchive(handle);
        }

        [Test]
        public void RARCloseArchiveTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RARReadHeaderExTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();
            var status = unrarDll.RARReadHeaderEx(handle, out headerData);
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

            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RARProcessFileWTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            openData.OpenMode = 1;
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            
            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);

            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void MultiVolumeTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);
            extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile2.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);

            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        private bool _callBackSuccess;
        [Test]
        public void RARSetCallbackTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            unrarDll.RARSetCallback(handle, RarCallBack, Marshal.StringToCoTaskMemUni("test data"));
            
            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);
            Assert.IsTrue(_callBackSuccess);
            _callBackSuccess = false;

            extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile2.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);
            Assert.IsTrue(_callBackSuccess);

            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        private void ExtractAndVerify(IUnrarDll unrarDll, IntPtr handle, string directory, string extractPath)
        {
            RARHeaderDataEx headerData;
            var status = unrarDll.RARReadHeaderEx(handle, out headerData);
            Assert.AreEqual(0, status);
            var extractStatus = unrarDll.RARProcessFileW(handle, 2, directory, extractPath);
            Assert.AreEqual(0, extractStatus);
            Assert.AreEqual(0, status);
            if(string.IsNullOrEmpty(extractPath))
            {
                Assert.IsTrue(Directory.Exists(directory));
                var files = Directory.GetFiles(directory);
                var extractedFiles = from file in files
                                     where Path.GetFileName(file) == headerData.FileNameW
                                     select file;
                Assert.IsTrue(extractedFiles.Any());
            }
            else
            {
                Assert.IsTrue(File.Exists(extractPath), "file doesn't exist");
                File.Delete(extractPath);
            }
        }

        [Test]
        public void ExtractToDirectoryTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\test");
            ExtractAndVerify(unrarDll, handle, extractPath, null);
            ExtractAndVerify(unrarDll, handle, extractPath, null);

            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);

            Directory.Delete(extractPath, true);
        }
        [Test]
        public void RAROpenArchiveDataExCallBackTest()
        {
            IUnrarDll unrarDll = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            openData.Callback = RarCallBack;

            openData.UserData = Marshal.StringToCoTaskMemUni("test data"); ;
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);
            Assert.IsTrue(_callBackSuccess);
            _callBackSuccess = false;

            extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile2.txt.test");
            ExtractAndVerify(unrarDll, handle, null, extractPath);
            Assert.IsTrue(_callBackSuccess);

            var result = unrarDll.RARCloseArchive(handle);
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
