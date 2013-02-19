using System;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class RarFileProcessorTests : TestsBase
    {
        private Mock<IUnrarHandle> MockUnrarHandle { get; set; }
        private Mock<IUnrarDll> MockUnrarDll { get; set; }
        private Mock<IFileSystem> MockFileSystem { get; set; }

        private RARHeaderDataEx MockHeaderData { get; set; }

        public override void Setup()
        {
            MockUnrarHandle = new Mock<IUnrarHandle>();
            MockUnrarDll = new Mock<IUnrarDll>();
            MockFileSystem = new Mock<IFileSystem>();
            MockUnrarHandle.SetupGet(x => x.UnrarDll).Returns(MockUnrarDll.Object);
            MockUnrarHandle.SetupGet(x => x.Handle).Returns(new IntPtr(1337));
            MockHeaderData = new RARHeaderDataEx
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

            MockUnrarDll.Setup(x => x.RARProcessFileW(new IntPtr(1337), 2, @"C:\invalidExtractPath", null))
                        .Returns((int)RarStatus.BadData);
        }

        [SetUp]
        public void TestSetup()
        {
            MockUnrarHandle.SetupGet(x => x.IsOpen).Returns(true);
        }
        private RarFileProcessor GetFileProcessor()
        {
            return new RarFileProcessor(MockUnrarHandle.Object);
        }

        [Test]
        public void ProcessFileShouldCompleteSuccessfully()
        {
            var fileProcessor = GetFileProcessor();
            fileProcessor.ProcessFile(@"C:\validExtractPath");
            Assert.Pass("If it reached this point then the test completed successfully.");
        }

        [Test]
        public void ProcessFileShouldFailBecauseOfBadData()
        {
            var fileProcessor = GetFileProcessor();
            var ex = Assert.Throws<UnrarException>(() => fileProcessor.ProcessFile(@"C:\invalidExtractPath"));
            Assert.That(ex.Status, Is.EqualTo(RarStatus.BadData));
            Assert.That(ex.Message, Is.EqualTo("Unable to extract file."));
        }

        [Test]
        public void ProcessFileShouldFailBecauseHandleIsClosed()
        {
            var fileProcessor = GetFileProcessor();
            MockUnrarHandle.SetupGet(x => x.IsOpen).Returns(false);
            var ex = Assert.Throws<InvalidOperationException>(() => fileProcessor.ProcessFile(null));
            Assert.That(ex.Message, Is.EqualTo("Unrar handle must be open to complete this operation."));
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenTheHandleArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new RarFileProcessor(null));
            Assert.That(ex.ParamName, Is.EqualTo("handle"));
        }
    }
}
