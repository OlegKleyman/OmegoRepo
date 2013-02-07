using System;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class RarMemberExtractorTests : TestsBase
    {
        private Mock<IUnrarHandle> MockUnrarHandle { get; set; }
        private Mock<IUnrarDll> MockUnrarDll { get; set; }
        private Mock<IFileSystem> MockFileSystem { get; set; }

        public override void Setup()
        {
            MockUnrarHandle = new Mock<IUnrarHandle>();
            MockUnrarDll = new Mock<IUnrarDll>();
            MockFileSystem = new Mock<IFileSystem>();
            MockUnrarHandle.SetupGet(x => x.UnrarDll).Returns(MockUnrarDll.Object);
            MockUnrarHandle.SetupGet(x => x.Handle).Returns(new IntPtr(1337));
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

            MockUnrarDll.Setup(x => x.RARReadHeaderEx(new IntPtr(1337), out test1TxtFileHeaderData));
        }

        [Test]
        public void ExtractShouldCompleteSuccessfully()
        {
            var extractor = new RarMemberExtractor(MockUnrarHandle.Object, MockFileSystem.Object);
            var result = extractor.Extract(@"C:\extractPath");
            Assert.That(result, Is.EqualTo(RarStatus.Success));
        }

        [Test]
        public void ExtractShouldSetTheCurrentMember()
        {
            var extractor = new RarMemberExtractor(MockUnrarHandle.Object, MockFileSystem.Object);
            extractor.Extract(@"C:\extractPath");
            Assert.That(extractor.CurrentMember.HighFlags, Is.EqualTo(HighMemberFlags.DictionarySize4096K));
            Assert.That(extractor.CurrentMember.LastModificationDate, Is.EqualTo(new DateTime(634751294720000000)));
            Assert.That(extractor.CurrentMember.LowFlags, Is.EqualTo(LowMemberFlags.FileContinuedOnNextVolume));
            Assert.That(extractor.CurrentMember.Name, Is.EqualTo("test2.txt"));
            Assert.That(extractor.CurrentMember.PackedSize, Is.EqualTo(3145642));
            Assert.That(extractor.CurrentMember.UnpackedSize, Is.EqualTo(5293080));
            Assert.That(extractor.CurrentMember.Volume, Is.EqualTo("C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar"));
        }
    }
}
