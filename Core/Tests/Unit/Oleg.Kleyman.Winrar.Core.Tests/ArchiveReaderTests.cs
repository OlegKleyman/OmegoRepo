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
        #region Setup/Teardown

        [SetUp]
        public void TestSetup()
        {
            UnrarHandleMock.SetupGet(x => x.Mode).Returns(OpenMode.List);
        }

        #endregion

        private Mock<IUnrarHandle> UnrarHandleMock { get; set; }
        private Mock<IUnrarDll> UnrarDllMock { get; set; }
        private RARHeaderDataEx _test2TxtFileHeaderData;
        private RARHeaderDataEx _test1TxtFileHeaderData;
        private RARHeaderDataEx _defaultHeaderData;

        public override void Setup()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            UnrarDllMock = new Mock<IUnrarDll>();
            UnrarHandleMock = new Mock<IUnrarHandle>();
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

        [Test]
        [ExpectedException(typeof (UnrarException), ExpectedMessage = "Unable to read header data.",
            MatchType = MessageMatch.Exact)]
        public void OpenArchiveCorruptTest()
        {
            var unrar = new Unrar(UnrarHandleMock.Object, null);
            UnrarHandleMock.SetupGet(x => x.IsOpen).Returns(true);

            var reader = unrar.ExecuteReader();
            UnrarHandleMock.SetupGet(x => x.Handle).Returns((IntPtr) 1111);
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test1TxtFileHeaderData)).Returns(12);
            reader.Read();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (InvalidOperationException),
            ExpectedMessage = "Handle mode must be OpenMode.List.", MatchType = MessageMatch.Exact)]
        public void ReadExtractModeTest()
        {
            UnrarHandleMock.SetupGet(x => x.IsOpen).Returns(true);
            UnrarHandleMock.SetupGet(x => x.Mode).Returns(OpenMode.Extract);
            var unrar = new Unrar(UnrarHandleMock.Object, null);

            unrar.ExecuteReader();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (InvalidOperationException),
            ExpectedMessage = "Handle must be open.", MatchType = MessageMatch.Exact)]
        public void ReadHandleNotOpenTest()
        {
            UnrarHandleMock.SetupGet(x => x.IsOpen).Returns(false);
            var unrar = new Unrar(UnrarHandleMock.Object, null);

            unrar.ExecuteReader();
        }

        [Test]
        public void ReadTest()
        {
            var unrar = new Unrar(UnrarHandleMock.Object, null);
            UnrarHandleMock.SetupGet(x => x.IsOpen).Returns(true);

            var reader = unrar.ExecuteReader();
            UnrarHandleMock.SetupGet(x => x.Handle).Returns((IntPtr) 1111);
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test1TxtFileHeaderData)).Returns(0);

            var member = reader.Read();

            Assert.AreEqual(RarStatus.Success, reader.Status);

            Assert.AreEqual("test2.txt", member.Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, member.HighFlags);
            Assert.AreEqual(new DateTime(634751294720000000), member.LastModificationDate);
            Assert.AreEqual(3145642, member.PackedSize);
            Assert.AreEqual(5293080, member.UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part1.rar", member.Volume);
            Assert.AreEqual(LowMemberFlags.FileContinuedOnNextVolume, member.LowFlags);

            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _test2TxtFileHeaderData)).Returns(0);

            member = reader.Read();

            Assert.AreEqual(RarStatus.Success, reader.Status);

            Assert.AreEqual("test.txt", member.Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, member.HighFlags);
            Assert.AreEqual(new DateTime(634751294360000000), member.LastModificationDate);
            Assert.AreEqual(297540, member.PackedSize);
            Assert.AreEqual(297540, member.UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part2.rar", member.Volume);
            Assert.AreEqual(LowMemberFlags.None, member.LowFlags);

            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _defaultHeaderData)).Returns(
                (uint) RarStatus.EndOfArchive);

            reader.Read();

            Assert.AreEqual(RarStatus.EndOfArchive, reader.Status);
        }
    }
}