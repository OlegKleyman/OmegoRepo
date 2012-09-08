using System;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class UnrarHandleTests : TestsBase
    {
        #region Setup/Teardown

        [SetUp]
        public void TestCaseSetup()
        {
            MockOpenArchiveTest();
        }

        #endregion

        private Mock<IUnrarDll> UnrarDllMock { get; set; }
        private RAROpenArchiveDataEx _openData;
        private RARHeaderDataEx _test1TxtFileHeaderData;
        private RAROpenArchiveDataEx _invalidFileOpenData;
        private const string FILE_PATH_TO_INVALID_RAR = "C:\\GitRepos\\MainDefault\\Common\\Test\\test.txt";
        private const string FILE_PATH_TO_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";

        public override void Setup()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            UnrarDllMock = new Mock<IUnrarDll>();

            _openData = new RAROpenArchiveDataEx
                            {
                                ArcName = FILE_PATH_TO_VALID_RAR,
                                OpenMode = (uint) OpenMode.List
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

            _invalidFileOpenData = new RAROpenArchiveDataEx
                                       {
                                           ArcName = FILE_PATH_TO_INVALID_RAR,
                                           OpenMode = (uint) OpenMode.List,
                                           OpenResult = 13
                                       };
        }

        private void MockOpenArchiveTest()
        {
            UnrarDllMock.Setup(x => x.RAROpenArchiveEx(ref _openData)).Returns(new IntPtr(1111));
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test1TxtFileHeaderData)).Returns(0);
            UnrarDllMock.Setup(x => x.RARCloseArchive(new IntPtr(1111))).Returns(0);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Unrar handle is not open.",
            MatchType = MessageMatch.Exact)]
        public void CloseConnectionNotOpenTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Close();
        }

        [Test]
        public void CloseTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Open();

            unrarHandle.Close(); //No exception assumes success
        }

        [Test]
        [ExpectedException(typeof (UnrarException),
            ExpectedMessage = "Unable to close archive. Possibly because it's already closed.",
            MatchType = MessageMatch.Exact)]
        public void CloseUnableToCloseTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);

            unrarHandle.Open();
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
        public void DisposeTest()
        {
            using (var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR))
            {
                unrarHandle.Open();
            }

            //If it gets to here then assume success
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "RarFilePath must be set.",
            MatchType = MessageMatch.Exact)]
        public void GetArchiveHandleMustBeOpenExceptionTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, null);
            unrarHandle.Open();
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException),
            ExpectedMessage = "Object is open and must be closed to open again.", MatchType = MessageMatch.Exact)]
        public void OpenAlreadyOpenTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Open();
            unrarHandle.Open();
        }

        [Test]
        [ExpectedException(typeof (UnrarException), ExpectedMessage = "Unable to open archive.",
            MatchType = MessageMatch.Exact)]
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
                unrarHandle.Open();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadArchive, ex.Status);
                throw;
            }
        }

        [Test]
        public void OpenTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Open();
            Assert.IsTrue(unrarHandle.IsOpen);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "UnrarDll must be set.",
            MatchType = MessageMatch.Exact)]
        public void OpenUnrarDllIsNullTest()
        {
            var unrarHandle = new UnrarHandle(null, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Open();
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException),
            ExpectedMessage = "Connection must be closed to change the mode.", MatchType = MessageMatch.Exact)]
        public void SetModeConnectionMustBeClosedExceptionTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Open();
            unrarHandle.Mode = OpenMode.Extract;
        }

        [Test]
        public void SetModeTest()
        {
            var unrarHandle = new UnrarHandle(null, null);
            unrarHandle.Mode = OpenMode.List;
            Assert.AreEqual(OpenMode.List, unrarHandle.Mode);
        }

        [Test]
        public void UnrarDllGetTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            Assert.AreEqual(unrarHandle.UnrarDll, UnrarDllMock.Object);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException),
            ExpectedMessage = "UnrarDll cannot be changed if the unrar handle is still open.",
            MatchType = MessageMatch.Exact)]
        public void UnrarDllSetCannotChangeTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.Open();
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
    }
}