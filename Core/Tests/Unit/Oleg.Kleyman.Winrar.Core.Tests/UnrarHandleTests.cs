using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class UnrarHandleTests : TestsBase
    {
        private Mock<IUnrar> UnrarDllMock { get; set; }
        private RAROpenArchiveDataEx _openData;
        private RARHeaderDataEx _defaultHeaderData;
        private RARHeaderDataEx _test2TxtFileHeaderData;
        private RARHeaderDataEx _test1TxtFileHeaderData;
        private RAROpenArchiveDataEx _invalidFileOpenData;
        private const string FILE_PATH_TO_INVALID_RAR = "C:\\GitRepos\\MainDefault\\Common\\Test\\test.txt";
        private const string FILE_PATH_TO_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";
        private const string FILE_PATH_TO_BROKEN_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";

        #region Overrides of TestsBase

        public override void Setup()
        {
            SetupMocks();
        }

        [SetUp]
        public void TestCaseSetup()
        {
            MockOpenArchiveTest();
        }

        private void SetupMocks()
        {
            UnrarDllMock = new Mock<IUnrar>();

            _openData = new RAROpenArchiveDataEx
                           {
                               ArcName = FILE_PATH_TO_VALID_RAR,
                               OpenMode = (uint)OpenMode.List
                           };

            _test1TxtFileHeaderData = new RARHeaderDataEx
            {
                ArcName = "㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴敔瑳瀮牡ㅴ爮牡",
                ArcNameW = "C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar",
                CmtBuf = null,
                CmtBufSize = 1,
                CmtSize = 0,
                CmtState = 0,
                FileAttr = 32,
                FileCRC = 2631502099,
                FileName = "整瑳⸲硴t",
                FileNameW = "test2.txt",
                FileTime = 1087152912,
                Flags = 37058,
                HostOS = 2,
                Method = 48,
                PackSize = 3145642,
                PackSizeHigh = 0,
                Reserved = new uint[1024],
                UnpSize = 5293080,
                UnpSizeHigh = 0,
                UnpVer = 20
            };

            _test2TxtFileHeaderData = new RARHeaderDataEx
            {
                ArcName = "㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴敔瑳瀮牡㉴爮牡",
                ArcNameW = "C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part2.rar",
                CmtBuf = null,
                CmtBufSize = 1,
                CmtSize = 0,
                CmtState = 0,
                FileAttr = 32,
                FileCRC = 462830299,
                FileName = "整瑳琮瑸",
                FileNameW = "test.txt",
                FileTime = 1087152892,
                Flags = 37056,
                HostOS = 2,
                Method = 48,
                PackSize = 297540,
                PackSizeHigh = 0,
                Reserved = new uint[1024],
                UnpSize = 297540,
                UnpSizeHigh = 0,
                UnpVer = 20
            };

            _invalidFileOpenData = new RAROpenArchiveDataEx
            {
                ArcName = FILE_PATH_TO_INVALID_RAR,
                OpenMode = (uint)OpenMode.List,
                OpenResult = 13
            };
        }

        private void MockOpenArchiveTest()
        {
            UnrarDllMock.Setup(x => x.RAROpenArchiveEx(ref _openData)).Returns(new IntPtr(1111));
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test1TxtFileHeaderData)).Returns(0);
            UnrarDllMock.Setup(x => x.RARCloseArchive(new IntPtr(1111))).Returns(0);
        }

        #endregion

        [Test]
        public void OpenArchiveTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.FileProcessed += ValidArchiveFileProcessed;
            var archive = unrarHandle.OpenArchive();
            Assert.IsNotNull(archive);
            Assert.AreEqual(FILE_PATH_TO_VALID_RAR, archive.FilePath);
            Assert.AreEqual(2, archive.Files.Count);
            Assert.AreEqual("test2.txt", archive.Files[0].Name);
            Assert.AreEqual(ArchiveMemberFlags.DictionarySize4096K, archive.Files[0].Flags);
            Assert.AreEqual(new DateTime(634751294720000000), archive.Files[0].LastModificationDate);
            Assert.AreEqual(3145642, archive.Files[0].PackedSize);
            Assert.AreEqual(5293080, archive.Files[0].UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part1.rar", archive.Files[0].Volume);
            Assert.AreEqual("test.txt", archive.Files[1].Name);
            Assert.AreEqual(ArchiveMemberFlags.DictionarySize4096K, archive.Files[1].Flags);
            Assert.AreEqual(new DateTime(634751294360000000), archive.Files[1].LastModificationDate);
            Assert.AreEqual(297540, archive.Files[1].PackedSize);
            Assert.AreEqual(297540, archive.Files[1].UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part2.rar", archive.Files[1].Volume);
        }

        private void ValidArchiveFileProcessed(object sender, UnrarFileProcessedEventArgs e)
        {
            if (e.ArchiveMember.Name == "test2.txt")
            {
                UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test2TxtFileHeaderData)).Returns((uint)RarStatus.Success);
            }

            if (e.ArchiveMember.Name == "test.txt")
            {
                UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _defaultHeaderData)).Returns((uint)RarStatus.EndOfArchive);
            }
        }

        [Test]
        public void CloseTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.FileProcessed += ValidArchiveFileProcessed;
            var archive = unrarHandle.OpenArchive();
            Assert.IsNotNull(archive);
            unrarHandle.Close(); //No exception assumes success
        }

        [Test]
        [ExpectedException(typeof(UnrarException), ExpectedMessage = "Unable to read header data.", MatchType = MessageMatch.Exact)]
        public void OpenArchiveCorruptTest()
        {
            SetupInvalidHeaderMock();
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_BROKEN_VALID_RAR);
            
            try
            {
                unrarHandle.OpenArchive();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadData, ex.Status);
                throw;
            }
            finally
            {
                if (unrarHandle.IsOpen)
                {
                    unrarHandle.Close();
                }
            }
        }

        private void SetupInvalidHeaderMock()
        {
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _defaultHeaderData)).Returns(12);
        }

        [Test]
        [ExpectedException(typeof(UnrarException), ExpectedMessage = "Unable to open archive.", MatchType = MessageMatch.Exact)]
        public void OpenArchiveUnknownFormatTest()
        {
            var customMock = new UnrarDllCustomMock
                                 {
                                     OpenData = _invalidFileOpenData,
                                     ReturnIntPtrValue = IntPtr.Zero
                                 };

            var unrarHandle = new UnrarHandle(customMock, FILE_PATH_TO_INVALID_RAR);
            try
            {
                unrarHandle.OpenArchive();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadArchive, ex.Status);
                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "UnrarDll cannot be changed if the unrar handle is still open.", MatchType = MessageMatch.Exact)]
        public void UnrarDllSetCannotChangeTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.FileProcessed += ValidArchiveFileProcessed;
            unrarHandle.OpenArchive();
            unrarHandle.UnrarDll = null;
        }

        [Test]
        public void UnrarDllSetTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            Assert.AreEqual(unrarHandle.UnrarDll, UnrarDllMock.Object);
            unrarHandle.UnrarDll = null;
            Assert.IsNull(unrarHandle.UnrarDll);
        }

        [Test]
        public void UnrarDllGetTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            Assert.AreEqual(unrarHandle.UnrarDll, UnrarDllMock.Object);
        }



        [Test]
        [ExpectedException(typeof(UnrarException), ExpectedMessage = "Unable to close archive. Possibly because it's already closed.", MatchType = MessageMatch.Exact)]
        public void CloseUnableToCloseTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.FileProcessed += ValidArchiveFileProcessed;
            unrarHandle.OpenArchive();
            UnrarDllMock.Setup(x => x.RARCloseArchive(new IntPtr(1111))).Returns(17);
            try
            {
                unrarHandle.Close();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.CloseError, ex.Status);
                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Unrar handle is not open.", MatchType = MessageMatch.Exact)]
        public void CloseConnectionNotOpenTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Close();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Object is open and must be closed to open again.", MatchType = MessageMatch.Exact)]
        public void OpenAlreadyOpenTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.FileProcessed += ValidArchiveFileProcessed;
            unrarHandle.OpenArchive();
            unrarHandle.OpenArchive();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "UnrarDll must be set.", MatchType = MessageMatch.Exact)]
        public void OpenUnrarDllIsNullTest()
        {
            var unrarHandle = new UnrarHandle(null, FILE_PATH_TO_VALID_RAR);
            unrarHandle.OpenArchive();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "RarFilePath must be set.", MatchType = MessageMatch.Exact)]
        public void OpenRarFilePathNotSetTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, null);
            unrarHandle.OpenArchive();
        }

        [Test]
        public void DisposeTest()
        {
            using(var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR))
            {
                unrarHandle.FileProcessed += ValidArchiveFileProcessed;
                var archive = unrarHandle.OpenArchive();
                Assert.IsNotNull(archive);
            }

            //If it gets to here then assume success
        }
    }
}
