using System;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class ArchiveReaderTests : TestsBase
    {
        protected Mock<IUnrarWrapper> MockWrapper { get; set; }
        private Mock<IMemberExtractor> MockMemberExtractor { get; set; }

        private RARHeaderDataEx _test2TxtFileHeaderData;
        private RARHeaderDataEx _test1TxtFileHeaderData;

        public override void Setup()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            MockMemberExtractor = new Mock<IMemberExtractor>();
            MockWrapper = new Mock<IUnrarWrapper>();
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

        [Test]
        public void ReadTest()
        {
            var reader = GetReader();

            MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember)_test1TxtFileHeaderData);

            var member = reader.Read();

            Assert.AreEqual(RarStatus.Success, reader.Status);

            Assert.AreEqual("test2.txt", member.Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, member.HighFlags);
            Assert.AreEqual(new DateTime(634751294720000000), member.LastModificationDate);
            Assert.AreEqual(3145642, member.PackedSize);
            Assert.AreEqual(5293080, member.UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part1.rar", member.Volume);
            Assert.AreEqual(LowMemberFlags.FileContinuedOnNextVolume, member.LowFlags);

            MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember)_test2TxtFileHeaderData);

            member = reader.Read();

            Assert.AreEqual(RarStatus.Success, reader.Status);

            Assert.AreEqual("test.txt", member.Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, member.HighFlags);
            Assert.AreEqual(new DateTime(634751294360000000), member.LastModificationDate);
            Assert.AreEqual(297540, member.PackedSize);
            Assert.AreEqual(297540, member.UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part2.rar", member.Volume);
            Assert.AreEqual(LowMemberFlags.None, member.LowFlags);

            MockMemberExtractor.Setup(x => x.Extract(null)).Returns(RarStatus.EndOfArchive);
            MockMemberExtractor.SetupGet(x => x.CurrentMember).Returns((ArchiveMember)null);

            reader.Read();

            Assert.AreEqual(RarStatus.EndOfArchive, reader.Status);
        }

        private IArchiveReader GetReader()
        {
            return ArchiveReader.Execute(MockWrapper.Object);
        }

        [Test]
        public void ExecuteShouldReturnArchiveReaderObject()
        {
            var reader = GetReader();
            Assert.That(reader, Is.InstanceOf<ArchiveReader>());
        }

        [Test]
        public void ExecuteShouldThrowExceptionWhenExtractorIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArchiveReader.Execute(MockWrapper.Object));
            Assert.That(ex.ParamName, Is.EqualTo("extractor"));
        }
    }
}