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
        private IntPtr ValidHandle { get; set; }
        private IntPtr InvalidHandle { get; set; }
        private Mock<IUnrarHandle> MockUnrarHandle { get; set; }
        private Mock<IUnrarDll> MockUnrarDll { get; set; }
        private Mock<IFileProcessor> MockFileProcessor { get; set; }

        public override void Setup()
        {
            MockUnrarHandle = new Mock<IUnrarHandle>();
            MockUnrarDll = new Mock<IUnrarDll>();
            MockFileProcessor = new Mock<IFileProcessor>();

            MockUnrarHandle.SetupGet(x => x.UnrarDll).Returns(MockUnrarDll.Object);
            ValidHandle = new IntPtr(1337);
            InvalidHandle = new IntPtr(7331);
// ReSharper disable RedundantAssignment - Even though this variable is use as an
                                         //output parameter we need to set it's value
                                         //in order to unit test the method through
                                         //mocking
            var test1TxtFileHeaderData = new RARHeaderDataEx
// ReSharper restore RedundantAssignment
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

            MockUnrarDll.Setup(x => x.RARReadHeaderEx(ValidHandle, out test1TxtFileHeaderData)).Returns((int)RarStatus.Success);
            MockUnrarDll.Setup(x => x.RARReadHeaderEx(InvalidHandle, out test1TxtFileHeaderData)).Returns((int) RarStatus.BadData);
        }

        [SetUp]
        public void SetupTest()
        {
            MockUnrarHandle.SetupGet(x => x.Handle).Returns(ValidHandle);
        }

        private RarMemberExtractor GetMemberExtractor()
        {
            return new RarMemberExtractor(MockUnrarHandle.Object, MockFileProcessor.Object);
        }

        [Test]
        public void ExtractShouldCompleteSuccessfully()
        {
            var extractor = GetMemberExtractor();
            var result = extractor.Extract(@"C:\extractPath");
            Assert.That(result, Is.EqualTo(RarStatus.Success));
        }

        [Test]
        public void ExtractShouldSetTheCurrentMember()
        {
            var extractor = GetMemberExtractor();
            extractor.Extract(@"C:\extractPath");
            Assert.That(extractor.CurrentMember.HighFlags, Is.EqualTo(HighMemberFlags.DictionarySize4096K));
            Assert.That(extractor.CurrentMember.LastModificationDate, Is.EqualTo(new DateTime(634751294720000000)));
            Assert.That(extractor.CurrentMember.LowFlags, Is.EqualTo(LowMemberFlags.FileContinuedOnNextVolume));
            Assert.That(extractor.CurrentMember.Name, Is.EqualTo("test2.txt"));
            Assert.That(extractor.CurrentMember.PackedSize, Is.EqualTo(3145642));
            Assert.That(extractor.CurrentMember.UnpackedSize, Is.EqualTo(5293080));
            Assert.That(extractor.CurrentMember.Volume, Is.EqualTo("C:\\GitRepos\\MainDefault\\Common\\Test\\Test.part1.rar"));
        }

        [Test]
        public void ExtractShouldThrowExceptionWhenUnableToReadHeaderData()
        {
            //Setup the unrar handle mock to return a handle which would cause
            //the unrar DLL mock to return an error code
            MockUnrarHandle.SetupGet(x => x.Handle).Returns(InvalidHandle);
            var extractor = GetMemberExtractor();
            var ex = Assert.Throws<UnrarException>(() => extractor.Extract(@"C:\extractPath"));
            Assert.That(ex.Status, Is.EqualTo(RarStatus.BadData));
            Assert.That(ex.Message, Is.EqualTo("Unable to read header data."));
        }

        [Test]
        public void ConstructorShouldThrowExceptionWhenHandleIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new RarMemberExtractor(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("handle"));
        }

        [Test]
        public void ConstructorShouldThrowExceptionWhenProcessorIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new RarMemberExtractor(MockUnrarHandle.Object, null));
            Assert.That(ex.ParamName, Is.EqualTo("processor"));
        }

        [Test]
        public void HandlePropertyShouldThrowExceptionWhenSetToNull()
        {
            var extractor = GetMemberExtractor();
            var ex = Assert.Throws<ArgumentNullException>(() => extractor.Handle = null);
            Assert.That(ex.ParamName, Is.EqualTo("value"));
        }

        [Test]
        public void ProcessorPropertyShouldThrowExceptionWhenSetToNull()
        {
            var extractor = GetMemberExtractor();
            var ex = Assert.Throws<ArgumentNullException>(() => extractor.Processor = null);
            Assert.That(ex.ParamName, Is.EqualTo("value"));
        }

        [Test]
        public void HandlePropertyShouldBeSetSuccessfully()
        {
            var mockHandle = new Mock<IUnrarHandle>();
            var extractor = GetMemberExtractor();
            extractor.Handle = mockHandle.Object;
            Assert.That(extractor.Handle, Is.EqualTo(mockHandle.Object));
        }

        [Test]
        public void ProcessorPropertyShouldBeSetSuccessfully()
        {
            var mockProcessor = new Mock<IFileProcessor>();
            var extractor = GetMemberExtractor();
            extractor.Processor = mockProcessor.Object;
            Assert.That(extractor.Processor, Is.EqualTo(mockProcessor.Object));
        }
    }
}
