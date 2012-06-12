using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Interop.Tests.Integration
{
    [TestFixture]
    public class UnrarDllTests : TestsBase
    {
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
            openData.Initialize();
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
            openData.Initialize();
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
            openData.Initialize();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();
            headerData.Initialize();
            var status = unrar.RARReadHeaderEx(handle, ref headerData);
            Assert.AreEqual(RarStatus.Success, status);

            Assert.AreEqual("㩣杜瑩敲潰屳慍湩敄慦汵屴潃浭湯呜獥屴整瑳楆敬爮牡", headerData.ArcName);
            Assert.AreEqual(@"c:\gitrepos\MainDefault\Common\Test\testFile.rar", headerData.ArcNameW);
            Assert.AreEqual(string.Empty, headerData.CmtBuf);
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
        public void RARProcessFileTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.Initialize();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            openData.OpenMode = OpenMode.Extract;
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();
            headerData.Initialize();
            var status = unrar.RARReadHeaderEx(handle, ref headerData);
            Assert.AreEqual(RarStatus.Success, status);

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            var extractStatus = unrar.RARProcessFile(handle, 2, string.Empty, extractPath);
            Assert.AreEqual(0, extractStatus);
            Assert.IsTrue(File.Exists(extractPath));
            File.Delete(extractPath);
            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RARProcessFileWTest()
        {
            IUnrar unrar = new UnrarDll();
            var openData = new RAROpenArchiveDataEx();
            openData.Initialize();
            openData.ArcName = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFile.rar");
            openData.OpenMode = OpenMode.Extract;
            var handle = unrar.RAROpenArchiveEx(ref openData);
            Assert.AreNotEqual(IntPtr.Zero, handle);

            var headerData = new RARHeaderDataEx();
            headerData.Initialize();
            var status = unrar.RARReadHeaderEx(handle, ref headerData);
            Assert.AreEqual(RarStatus.Success, status);

            var extractPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\TestExtractions\testFile.txt.test");
            var extractStatus = unrar.RARProcessFileW(handle, 2, string.Empty, extractPath);
            Assert.AreEqual(0, extractStatus);
            Assert.IsTrue(File.Exists(extractPath), "file doesn't exist");
            File.Delete(extractPath);
            var result = unrar.RARCloseArchive(handle);
            Assert.AreEqual(0, result);
        }
    }
}
