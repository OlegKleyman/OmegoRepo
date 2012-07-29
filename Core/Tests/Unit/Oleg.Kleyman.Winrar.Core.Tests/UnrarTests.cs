using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class UnrarTests : TestsBase
    {
        private Mock<IUnrarHandle> UnrarHandleMock { get; set; }
        private Mock<IUnrarDll> UnrarDllMock { get; set; }
        private Mock<IFileSystem> MockFileSystem { get; set; }

        private RARHeaderDataEx _test2TxtFileHeaderData;
        private RARHeaderDataEx _test1TxtFileHeaderData;
        private const string FILE_PATH_TO_INVALID_RAR = "C:\\GitRepos\\MainDefault\\Common\\Test\\test.txt";
        private const string FILE_PATH_TO_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";
        private const string FILE_PATH_TO_BROKEN_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";
        private const string FILE_PATH_TO_EXTRACTION_FOLDER = @"C:\GitRepos\MainDefault\Common\Test\";

        #region Overrides of TestsBase

        public override void Setup()
        {
            SetupMocks();
        }

        #endregion

        [SetUp]
        public void SetupTest()
        {
            UnrarHandleMock.SetupGet(x => x.IsOpen).Returns(true);
        }

        private void SetupMocks()
        {
            UnrarDllMock = new Mock<IUnrarDll>();
            UnrarHandleMock = new Mock<IUnrarHandle>();
            MockFileSystem = new Mock<IFileSystem>();

            SetupFileSystem();

            UnrarHandleMock.SetupGet(x => x.UnrarDll).Returns(UnrarDllMock.Object);
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
        }

        private void SetupFileSystem()
        {
            MockFileSystem.Setup(x => x.GetDirectory(FILE_PATH_TO_EXTRACTION_FOLDER)).Returns(new MockDirectory(FILE_PATH_TO_EXTRACTION_FOLDER));
        }

        [Test]
        public void ExecuteReaderTest()
        {
            var unrar = new Unrar(UnrarHandleMock.Object, null);

            var reader = unrar.ExecuteReader();

            Assert.AreEqual(UnrarHandleMock.Object, reader.Handle);
            Assert.AreEqual(RarStatus.Success, reader.Status);
        }

        [Test]
        public void ExtractTest()
        {
            var unrar = new Unrar(UnrarHandleMock.Object, MockFileSystem.Object);
            var extractionFolder = unrar.Extract(@"C:\GitRepos\MainDefault\Common\Test\");
            Assert.AreEqual(FILE_PATH_TO_EXTRACTION_FOLDER, extractionFolder.FullName);
        }
    }
}
