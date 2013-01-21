using System;
using System.IO;
using System.Linq;
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
    public class ArchiveTests : TestsBase
    {
        [SetUp]
        public void SetupTest()
        {
            MockArchiveReader.SetupGet(x => x.Status).Returns(RarStatus.Success);
            MockUnrarHandle.Object.Open();
        }

        private const string FILE_PATH_TO_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";
        private const string FILE_PATH_TO_EXTRACTION_FOLDER = @"C:\GitRepos\MainDefault\Common\Test\";
        private Mock<IUnrarHandle> MockUnrarHandle { get; set; }
        private Mock<IUnrar> MockUnrar { get; set; }
        private Mock<IArchiveReader> MockArchiveReader { get; set; }

        public override void Setup()
        {
            AttributesToAvoidReplicating.Add(typeof (FileIOPermissionAttribute));
            MockUnrarHandle = new Mock<IUnrarHandle>();
            MockUnrar = new Mock<IUnrar>();
            MockArchiveReader = new Mock<IArchiveReader>();

            SetupUnrarHandle();

            var test1TxtFileHeaderData = new RARHeaderDataEx
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

            var test2TxtFileHeaderData = new RARHeaderDataEx
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

            var members = new[]
                {
                    (ArchiveMember) test1TxtFileHeaderData,
                    (ArchiveMember) test2TxtFileHeaderData
                };

            var readCallCounter = 1;
            MockArchiveReader.Setup(x => x.Read()).Returns(() =>
                {
                    switch (readCallCounter)
                    {
                        case 1:
                            readCallCounter++;
                            return members[0];
                        case 2:
                            MockArchiveReader.SetupGet(x => x.Status).
                                              Returns(RarStatus.EndOfArchive);
                            readCallCounter = 1;
                            return members[1];
                    }

                    throw new ApplicationException(
                        string.Format(
                            "readCallCounter was at an unknown value {0}",
                            readCallCounter));
                });


            SetupUnrar();
        }

        private void SetupUnrar()
        {
            MockUnrar.Setup(x => x.ExecuteReader()).Returns(MockArchiveReader.Object);
            MockUnrar.SetupGet(x => x.Handle).Returns(MockUnrarHandle.Object);
            var mockMembers = new[]
                {
                    new Mock<IFileSystemMember>(),
                    new Mock<IFileSystemMember>(),
                    new Mock<IFileSystemMember>(),
                    new Mock<IFileSystemMember>()
                };
            mockMembers[0].SetupGet(x => x.FullName).Returns(
                @"C:\GitRepos\MainDefault\Common\Test\TestFolder\testFile.txt");
            mockMembers[1].SetupGet(x => x.FullName).Returns(@"C:\GitRepos\MainDefault\Common\Test\test.txt");
            mockMembers[2].SetupGet(x => x.FullName).Returns(
                @"C:\GitRepos\MainDefault\Common\Test\TestFolder\InnerTestFolder");
            mockMembers[2].SetupGet(x => x.Attributes).Returns(FileAttributes.Directory);
            mockMembers[3].SetupGet(x => x.FullName).Returns(@"C:\GitRepos\MainDefault\Common\Test\TestFolder");
            mockMembers[3].SetupGet(x => x.Attributes).Returns(FileAttributes.Directory);

            MockUnrar.Setup(x => x.Extract(FILE_PATH_TO_EXTRACTION_FOLDER)).Returns(
                mockMembers.Select(x => x.Object).ToArray());
        }

        private void SetupUnrarHandle()
        {
            MockUnrarHandle.SetupGet(x => x.RarFilePath).Returns(FILE_PATH_TO_VALID_RAR);
            MockUnrarHandle.Setup(x => x.Open()).Callback(() => MockUnrarHandle.SetupGet(y => y.IsOpen).Returns(true));
            MockUnrarHandle.Setup(x => x.Close()).Callback(() => MockUnrarHandle.SetupGet(y => y.IsOpen).Returns(false));
            MockUnrarHandle.Setup(x => x.Dispose()).Callback(
                () => MockUnrarHandle.SetupGet(y => y.IsOpen).Returns(false));
        }

        [Test]
        public void ExtractTest()
        {
            var archive = Archive.Open(MockUnrar.Object);

            var extractedContents = archive.Extract(@"C:\GitRepos\MainDefault\Common\Test\");

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
        public void OpenUnrarTest()
        {
            var archive = Archive.Open(MockUnrar.Object);
            Assert.AreEqual(2, archive.Files.Count);


            Assert.AreEqual(@"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar", archive.FilePath);
            Assert.AreEqual(2, archive.Files.Count);
            Assert.AreEqual("test2.txt", archive.Files[0].Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, archive.Files[0].HighFlags);
            Assert.AreEqual(new DateTime(634751294720000000), archive.Files[0].LastModificationDate);
            Assert.AreEqual(3145642, archive.Files[0].PackedSize);
            Assert.AreEqual(5293080, archive.Files[0].UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part1.rar", archive.Files[0].Volume);
            Assert.AreEqual(LowMemberFlags.FileContinuedOnNextVolume, archive.Files[0].LowFlags);
            Assert.AreEqual("test.txt", archive.Files[1].Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, archive.Files[1].HighFlags);
            Assert.AreEqual(new DateTime(634751294360000000), archive.Files[1].LastModificationDate);
            Assert.AreEqual(297540, archive.Files[1].PackedSize);
            Assert.AreEqual(297540, archive.Files[1].UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part2.rar", archive.Files[1].Volume);
            Assert.AreEqual(LowMemberFlags.None, archive.Files[1].LowFlags);
        }
    }
}