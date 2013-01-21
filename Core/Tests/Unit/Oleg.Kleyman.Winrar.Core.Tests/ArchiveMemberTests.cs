using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class ArchiveMemberTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void RarHeaderDataExShouldConvertToArchiveMemberSuccessfully()
        {
            var target = new RARHeaderDataEx
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

            var archiveMember = (ArchiveMember) target;
            Assert.AreEqual("test.txt", archiveMember.Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, archiveMember.HighFlags);
            Assert.AreEqual(new DateTime(634751294360000000), archiveMember.LastModificationDate);
            Assert.AreEqual(297540, archiveMember.PackedSize);
            Assert.AreEqual(297540, archiveMember.UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part2.rar", archiveMember.Volume);
            Assert.AreEqual(LowMemberFlags.None, archiveMember.LowFlags);
        }

        [Test]
        public void RarHeaderDataExShouldConvertToArchiveMemberWithHighUnpackedSizeSetToNonZeroSuccessfully()
        {
            var target = new RARHeaderDataEx
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
                    UnpSizeHigh = 1,
                    UnpVer = 20
                };

            var archiveMember = (ArchiveMember) target;
            Assert.AreEqual("test.txt", archiveMember.Name);
            Assert.AreEqual(HighMemberFlags.DictionarySize4096K, archiveMember.HighFlags);
            Assert.AreEqual(new DateTime(634751294360000000), archiveMember.LastModificationDate);
            Assert.AreEqual(297540, archiveMember.PackedSize);
            Assert.AreEqual(4295264836, archiveMember.UnpackedSize);
            Assert.AreEqual(@"C:\GitRepos\MainDefault\Common\Test\Test.part2.rar", archiveMember.Volume);
            Assert.AreEqual(LowMemberFlags.None, archiveMember.LowFlags);
        }
    }
}