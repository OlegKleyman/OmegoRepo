using System;
using System.IO;
using System.Security.Permissions;
using Castle.DynamicProxy.Generators;
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
        [SetUp]
        public void SetupTest()
        {
            UnrarHandleMock.SetupGet(x => x.IsOpen).Returns(true);
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test1FileHeaderData)).Returns(0);
            UnrarDllMock.Setup(x => x.RARProcessFileW(new IntPtr(1111), 2, FILE_PATH_TO_EXTRACTION_FOLDER, null)).
                         Returns(0);
        }

        private RARHeaderDataEx _test2FileHeaderData;
        private RARHeaderDataEx _test1FileHeaderData;
        private RARHeaderDataEx _test3FileHeaderData;
        private RARHeaderDataEx _test4FileHeaderData;
        private Mock<IUnrarHandle> UnrarHandleMock { get; set; }
        private Mock<IUnrarDll> UnrarDllMock { get; set; }
        private Mock<IMemberExtractor> MockMemberExtractor { get; set; }
        private Mock<IFileSystemMemberFactory> MockFileSystemMemberFactory { get; set; }

        private const string FILE_PATH_TO_EXTRACTION_FOLDER = @"C:\GitRepos\MainDefault\Common\Test\";

        public override void Setup()
        {
            AttributesToAvoidReplicating.Add(typeof (FileIOPermissionAttribute));
            SetupMocks();
        }

        private void SetupMocks()
        {
            UnrarDllMock = new Mock<IUnrarDll>();
            UnrarHandleMock = new Mock<IUnrarHandle>();
            MockMemberExtractor = new Mock<IMemberExtractor>();
            MockFileSystemMemberFactory = new Mock<IFileSystemMemberFactory>();

            SetupFileSystem();
            SetupUnrarHandle();
            SetupUnrarDll();
            SetupMemberExtractor();
            SetupFileFactory();
        }

        private void SetupFileFactory()
        {
            var mockTestFileTxtFileMember = new Mock<IFileSystemMember>();
            mockTestFileTxtFileMember.SetupGet(x => x.FullName)
                                     .Returns("C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder\\testFile.txt");
            MockFileSystemMemberFactory.Setup(
                x => x.GetFileMember(It.Is((ArchiveMember y) => y.Name == "TestFolder\\testFile.txt"), @"C:\GitRepos\MainDefault\Common\Test\")).Returns(mockTestFileTxtFileMember.Object);
        }

        private void SetupUnrarDll()
        {
            _test1FileHeaderData = new RARHeaderDataEx
                {
                    ArcName = "㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴敔瑳潆摬牥爮牡",
                    ArcNameW = "C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder.rar",
                    CmtBuf = null,
                    CmtBufSize = 0,
                    CmtSize = 0,
                    CmtState = 0,
                    FileAttr = 32,
                    FileCRC = 0,
                    FileName = "敔瑳潆摬牥瑜獥䙴汩⹥硴t",
                    FileNameW = "TestFolder\\testFile.txt",
                    FileTime = 1090800113,
                    Flags = 36960,
                    HostOS = 2,
                    Method = 48,
                    PackSize = 0,
                    PackSizeHigh = 0,
                    Reserved = new uint[1024],
                    UnpSize = 0,
                    UnpSizeHigh = 0,
                    UnpVer = 29
                };

            _test2FileHeaderData = new RARHeaderDataEx
                {
                    ArcName = "㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴敔瑳潆摬牥爮牡",
                    ArcNameW = "C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder.rar",
                    CmtBuf = null,
                    CmtBufSize = 0,
                    CmtSize = 0,
                    CmtState = 0,
                    FileAttr = 32,
                    FileCRC = 2462479057,
                    FileName = "整瑳琮瑸",
                    FileNameW = "test.txt",
                    FileTime = 1087221789,
                    Flags = 36960,
                    HostOS = 2,
                    Method = 53,
                    PackSize = 41,
                    PackSizeHigh = 0,
                    Reserved = new uint[1024],
                    UnpSize = 297541,
                    UnpSizeHigh = 0,
                    UnpVer = 29
                };

            _test3FileHeaderData = new RARHeaderDataEx
                {
                    ArcName = "㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴敔瑳潆摬牥爮牡",
                    ArcNameW = "C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder.rar",
                    CmtBuf = null,
                    CmtBufSize = 0,
                    CmtSize = 0,
                    CmtState = 0,
                    FileAttr = 16,
                    FileCRC = 0,
                    FileName = "敔瑳潆摬牥䥜湮牥敔瑳潆摬牥",
                    FileNameW = "TestFolder\\InnerTestFolder",
                    FileTime = 1090800107,
                    Flags = 37088,
                    HostOS = 2,
                    Method = 48,
                    PackSize = 0,
                    PackSizeHigh = 0,
                    Reserved = new uint[1024],
                    UnpSize = 0,
                    UnpSizeHigh = 0,
                    UnpVer = 20
                };

            _test4FileHeaderData = new RARHeaderDataEx
                {
                    ArcName = "㩃䝜瑩敒潰屳慍湩敄慦汵屴潃浭湯呜獥屴敔瑳潆摬牥爮牡",
                    ArcNameW = "C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder.rar",
                    CmtBuf = null,
                    CmtBufSize = 0,
                    CmtSize = 0,
                    CmtState = 0,
                    FileAttr = 16,
                    FileCRC = 0,
                    FileName = "敔瑳潆摬牥",
                    FileNameW = "TestFolder",
                    FileTime = 1090800115,
                    Flags = 37088,
                    HostOS = 2,
                    Method = 48,
                    PackSize = 0,
                    PackSizeHigh = 0,
                    Reserved = new uint[1024],
                    UnpSize = 0,
                    UnpSizeHigh = 0,
                    UnpVer = 20
                };
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test1FileHeaderData)).Returns(0);
        }

        private void SetupMemberExtractor()
        {
            MockMemberExtractor.Setup(x => x.Extract(@"C:\GitRepos\MainDefault\Common\Test\UnableToReadHeaderData"))
                               .Throws(new UnrarException("Unable to read header data.", RarStatus.BadData));

            MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember)_test1FileHeaderData);
            MockMemberExtractor.Setup(x => x.Extract(@"C:\GitRepos\MainDefault\Common\Test\"))
                               .Returns(RarStatus.Success);
        }

        private void SetupUnrarHandle()
        {
            UnrarHandleMock.SetupGet(x => x.UnrarDll).Returns(UnrarDllMock.Object);
            UnrarHandleMock.Setup(x => x.Handle).Returns(new IntPtr(1111));
        }

        private void SetupFileSystem()
        {
            var mockFileSystemInfo1 = new Mock<IFileSystemMember>();
            var mockFileSystemInfo2 = new Mock<IFileSystemMember>();
            var mockFileSystemInfo3 = new Mock<IFileSystemMember>();
            var mockFileSystemInfo4 = new Mock<IFileSystemMember>();

            mockFileSystemInfo1.SetupGet(x => x.FullName).Returns(
                @"C:\GitRepos\MainDefault\Common\Test\TestFolder\testFile.txt");
            mockFileSystemInfo2.SetupGet(x => x.FullName).Returns(@"C:\GitRepos\MainDefault\Common\Test\test.txt");
            mockFileSystemInfo3.SetupGet(x => x.FullName).Returns(
                @"C:\GitRepos\MainDefault\Common\Test\TestFolder\InnerTestFolder");
            mockFileSystemInfo3.SetupGet(x => x.Attributes).Returns(FileAttributes.Directory);
            mockFileSystemInfo4.SetupGet(x => x.FullName).Returns(@"C:\GitRepos\MainDefault\Common\Test\TestFolder");
            mockFileSystemInfo4.SetupGet(x => x.Attributes).Returns(FileAttributes.Directory);

            MockFileSystemMemberFactory.Setup(
                x => x.GetFileMember(It.IsAny<ArchiveMember>(), "C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder\\testFile.txt")).Returns(
                    mockFileSystemInfo1.Object);
            MockFileSystemMemberFactory.Setup(x => x.GetFileMember(It.IsAny<ArchiveMember>(), "C:\\GitRepos\\MainDefault\\Common\\Test\\test.txt")).Returns(
                mockFileSystemInfo2.Object);
            MockFileSystemMemberFactory.Setup(
                x => x.GetFileMember(It.IsAny<ArchiveMember>(), "C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder\\InnerTestFolder")).Returns(
                    mockFileSystemInfo3.Object);
            MockFileSystemMemberFactory.Setup(x => x.GetFileMember(It.IsAny<ArchiveMember>(), @"C:\GitRepos\MainDefault\Common\Test\TestFolder")).Returns(
                mockFileSystemInfo4.Object);
        }

        private Unrar GetUnrarObject()
        {
            return new Unrar(UnrarHandleMock.Object, MockMemberExtractor.Object, MockFileSystemMemberFactory.Object);
        }

        private void ExtractTest_MemberExtracted(object sender, UnrarEventArgs e)
        {
            switch (e.ArchiveMember.Name)
            {
                case "TestFolder\\testFile.txt":
                    MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember) _test2FileHeaderData);
                    break;
                case "test.txt":
                    MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember)_test3FileHeaderData);
                    break;
                case "TestFolder\\InnerTestFolder":
                    MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember)_test4FileHeaderData);
                    break;
                case "TestFolder":
                    MockMemberExtractor.Setup(x => x.Extract(@"C:\GitRepos\MainDefault\Common\Test\"))
                                       .Returns(RarStatus.EndOfArchive);
                    break;
            }
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Handle cannot be null.",
            MatchType = MessageMatch.Exact)]
        public void ExecuteReaderHandleCannotBeNullErrorTest()
        {
            var unrar = new Unrar(null, MockMemberExtractor.Object, MockFileSystemMemberFactory.Object);
            unrar.ExecuteReader();
        }

        [Test]
        public void ExecuteReaderTest()
        {
            var unrar = new Unrar(UnrarHandleMock.Object, MockMemberExtractor.Object, null);

            var reader = unrar.ExecuteReader();


            Assert.AreEqual(RarStatus.Success, reader.Status);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "FileFactory cannot be null.",
            MatchType = MessageMatch.Exact)]
        public void ExtractFileSystemCannotBeNullTest()
        {
            var unrar = new Unrar(UnrarHandleMock.Object, MockMemberExtractor.Object, null);
            unrar.Extract(@"C:\GitRepos\MainDefault\Common\Test\");
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Handle cannot be null.",
            MatchType = MessageMatch.Exact)]
        public void ExtractHandleCannotBeNullErrorTest()
        {
            var unrar = new Unrar(null, MockMemberExtractor.Object, MockFileSystemMemberFactory.Object);
            unrar.Extract(@"C:\GitRepos\MainDefault\Common\Test\");
        }

        [Test]
        [ExpectedException(typeof (UnrarException), ExpectedMessage = "Unable to read header data.",
            MatchType = MessageMatch.Exact)]
        public void ExtractHeaderDataErrorTest()
        {
            var unrar = GetUnrarObject();
            try
            {
                unrar.Extract(@"C:\GitRepos\MainDefault\Common\Test\UnableToReadHeaderData");
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadData, ex.Status);
                throw;
            }
        }

        [Test]
        public void ExtractTest()
        {
            var unrar = GetUnrarObject();
            unrar.MemberExtracted += ExtractTest_MemberExtracted;
            var extractedContents = unrar.Extract(@"C:\GitRepos\MainDefault\Common\Test\");
            Assert.AreEqual(4, extractedContents.Length);
            Assert.AreEqual("C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder\\testFile.txt",
                            extractedContents[0].FullName);
            Assert.AreEqual("C:\\GitRepos\\MainDefault\\Common\\Test\\test.txt", extractedContents[1].FullName);
            Assert.AreEqual("C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder\\InnerTestFolder",
                            extractedContents[2].FullName);
            Assert.AreEqual(FileAttributes.Directory, extractedContents[2].Attributes);
            Assert.AreEqual("C:\\GitRepos\\MainDefault\\Common\\Test\\TestFolder", extractedContents[3].FullName);
            Assert.AreEqual(FileAttributes.Directory, extractedContents[3].Attributes);
        }

        [Test]
        [ExpectedException(typeof (UnrarException), ExpectedMessage = "Unable to extract file.",
            MatchType = MessageMatch.Exact)]
        public void ExtractUnableToProcessFileErrorTest()
        {
            UnrarDllMock.Setup(x => x.RARProcessFileW(new IntPtr(1111), 2, FILE_PATH_TO_EXTRACTION_FOLDER, null)).
                         Returns(12);

            var unrar = GetUnrarObject();
            try
            {
                unrar.Extract(@"C:\GitRepos\MainDefault\Common\Test\");
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadData, ex.Status);
                throw;
            }
        }
    }
}