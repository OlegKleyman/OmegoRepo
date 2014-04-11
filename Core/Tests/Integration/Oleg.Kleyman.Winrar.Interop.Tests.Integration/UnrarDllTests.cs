using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Interop.Tests.Integration
{
    [TestFixture]
    public sealed class UnrarDllTests : TestsBase, IDisposable
    {
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        public override void Setup()
        {
        }

        private bool _callBackSuccess;

        private void ExtractAndVerify(IUnrarDll unrarDll, IntPtr handle, string directory, string extractPath)
        {
            RARHeaderDataEx headerData;
            var status = unrarDll.RARReadHeaderEx(handle, out headerData);
            Assert.AreEqual(0, status);
            var extractStatus = unrarDll.RARProcessFileW(handle, 2, directory, extractPath);
            Assert.AreEqual(0, extractStatus);
            Assert.AreEqual(0, status);
            if (string.IsNullOrEmpty(extractPath))
            {
                if ((headerData.Flags & 0xE0) == 0xE0)
                {
                    Assert.IsTrue(Directory.Exists(Path.Combine(directory, headerData.FileNameW)));
                }
                else
                {
                    Assert.IsTrue(File.Exists(Path.Combine(directory, headerData.FileNameW)));
                }

                Directory.Delete(directory, true);
                Directory.CreateDirectory(directory);
            }
            else
            {
                Assert.IsTrue(File.Exists(extractPath), "file doesn't exist");
                File.Delete(extractPath);
            }
        }

        private int RarCallBack(uint message, IntPtr userData, IntPtr p1, int p2)
        {
            Assert.AreEqual("test data", Marshal.PtrToStringUni(userData));
            _callBackSuccess = true;
            return 1;
        }

        public void Dispose()
        {
            _manualResetEvent.Dispose();
        }

        [Test]
        public void ExtractToDirectoryTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
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
        public void ExtractWithFoldersTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestFolder.rar");
            openData.OpenMode = 1;
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\");
            ExtractAndVerify(unrarDll, handle, extractPath, null);
            ExtractAndVerify(unrarDll, handle, extractPath, null);
            ExtractAndVerify(unrarDll, handle, extractPath, null);
            ExtractAndVerify(unrarDll, handle, extractPath, null);

            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void MultiVolumeTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
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

        [Test]
        public void RARCloseArchiveTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            var result = unrarDll.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RAROpenArchiveDataExCallBackTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            openData.OpenMode = 1;
            openData.Callback = RarCallBack;

            openData.UserData = Marshal.StringToCoTaskMemUni("test data");
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

        [Test]
        public void RAROpenArchiveExTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);
            unrarDll.RARCloseArchive(handle);
        }

        [Test]
        public void RARProcessFileWTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
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
        public void RARReadHeaderExTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
            var openData = new RAROpenArchiveDataEx();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrarDll.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();
            var status = unrarDll.RARReadHeaderEx(handle, out headerData);
            Assert.AreEqual(0, status);
            var expectedPath = Path.GetFullPath(@"..\..\..\..\..\..\");

            Assert.AreEqual(Path.Combine(expectedPath, @"Common\Test\testFile.rar"), headerData.ArcNameW);
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
        public void RARSetCallbackTest()
        {
            IUnrarDll unrarDll = new NativeMethods();
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
    }
}