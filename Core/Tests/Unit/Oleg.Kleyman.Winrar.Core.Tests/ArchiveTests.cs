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
    public class ArchiveTests : TestsBase
    {
        private const string FILE_PATH_TO_VALID_RAR = @"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar";
        private const string FILE_PATH_TO_EXTRACTION_FOLDER = @"C:\GitRepos\MainDefault\Common\Test\";
        private Mock<IUnrarHandle> MockUnrarHandle { get; set; }
        private Mock<IUnrar> MockUnrar { get; set; }
        private Mock<IArchiveReader> MockArchiveReader { get; set; }

        #region Overrides of TestsBase

        public override void Setup()
        {
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

            MockArchiveReader.SetupGet(x => x.Handle).Returns(MockUnrarHandle.Object);
            SetupUnrar();
        }

        private void SetupUnrar()
        {
            MockUnrar.Setup(x => x.ExecuteReader()).Returns(MockArchiveReader.Object);
            MockUnrar.SetupGet(x => x.Handle).Returns(MockUnrarHandle.Object);
            MockUnrar.Setup(x => x.Extract(FILE_PATH_TO_EXTRACTION_FOLDER)).Returns(new MockDirectory(FILE_PATH_TO_EXTRACTION_FOLDER));
        }

        private void SetupUnrarHandle()
        {
            MockUnrarHandle.SetupGet(x => x.RarFilePath).Returns(FILE_PATH_TO_VALID_RAR);
            MockUnrarHandle.Setup(x => x.Open()).Callback(() => MockUnrarHandle.SetupGet(y => y.IsOpen).Returns(true));
            MockUnrarHandle.Setup(x => x.Close()).Callback(() => MockUnrarHandle.SetupGet(y => y.IsOpen).Returns(false));
            MockUnrarHandle.Setup(x => x.Dispose()).Callback(() => MockUnrarHandle.SetupGet(y => y.IsOpen).Returns(false));
        }

        #endregion

        [SetUp]
        public void SetupTest()
        {
            MockArchiveReader.SetupGet(x => x.Status).Returns(RarStatus.Success);
            MockUnrarHandle.Object.Open();
        }

        [Test]
        public void OpenUnrarTest()
        {
            var archive = Archive.Open(null, MockUnrar.Object);
            Assert.AreEqual(2, archive.Files.Count);

            Assert.AreEqual(MockUnrarHandle.Object, archive.Handle);
            Assert.AreEqual(MockUnrarHandle.Object, MockArchiveReader.Object.Handle);
            Assert.AreEqual(@"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar", archive.FilePath);
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
            Assert.IsTrue(archive.Handle.IsOpen);
        }

        [Test]
        public void OpenReaderTest()
        {
            var archive = Archive.Open(null, MockArchiveReader.Object);
            Assert.AreEqual(2, archive.Files.Count);

            Assert.AreEqual(MockUnrarHandle.Object, archive.Handle);
            Assert.AreEqual(MockUnrarHandle.Object, MockArchiveReader.Object.Handle);
            Assert.AreEqual(@"C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar", archive.FilePath);
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
            Assert.IsTrue(archive.Handle.IsOpen);
        }

        [Test]
        public void ExtractTest()
        {
            var archive = Archive.Open(null, MockUnrar.Object);

            var containingFolder = archive.Extract(@"C:\GitRepos\MainDefault\Common\Test\");
            
            Assert.AreEqual(FILE_PATH_TO_EXTRACTION_FOLDER, containingFolder.FullName);
        }
    }
}
