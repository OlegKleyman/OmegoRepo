using System;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    public class ArchiveTests : TestsBase
    {
        private IUnrarDll UnrarDll { get; set; }
        private IUnrar Unrar { get; set; }
        private IUnrarHandle Handle { get; set; }
        private IFileSystem FileSystem { get; set; }

        public override void Setup()
        {
            UnrarDll = new NativeMethods();
            Handle = new UnrarHandle(UnrarDll);
            FileSystem = new FileSystem();
            Unrar = new Unrar(Handle, FileSystem);
        }

        [Test]
        public void ExtractTest()
        {
            Handle.RarFilePath =
                @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar";
            IFileSystemMember[] extractedMembers;
            using (Handle)
            {
                Handle.Mode = OpenMode.List;
                Handle.Open();
                var archive = Archive.Open(Unrar);
                extractedMembers =
                    archive.Extract(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing");
            }

            Assert.AreEqual(4, extractedMembers.Length);
            Assert.AreEqual(FileAttributes.Archive, extractedMembers[0].Attributes);
            Assert.IsTrue(extractedMembers[0].Exists);
            Assert.AreEqual(
                "C:\\GitRepos\\MainDefault\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\Testing\\TestFolder\\testFile.txt",
                extractedMembers[0].FullName);

            Assert.AreEqual(FileAttributes.Archive, extractedMembers[1].Attributes);
            Assert.IsTrue(extractedMembers[1].Exists);
            Assert.AreEqual(
                "C:\\GitRepos\\MainDefault\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\Testing\\test.txt",
                extractedMembers[1].FullName);

            Assert.AreEqual(FileAttributes.Directory, extractedMembers[2].Attributes);
            Assert.IsTrue(extractedMembers[2].Exists);
            Assert.AreEqual(
                "C:\\GitRepos\\MainDefault\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\Testing\\TestFolder\\InnerTestFolder",
                extractedMembers[2].FullName);

            Assert.AreEqual(FileAttributes.Directory, extractedMembers[3].Attributes);
            Assert.IsTrue(extractedMembers[3].Exists);
            Assert.AreEqual(
                "C:\\GitRepos\\MainDefault\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\Testing\\TestFolder",
                extractedMembers[3].FullName);

            Directory.Delete(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing", true);
        }

        [Test]
        public void OpenTest()
        {
            Handle.RarFilePath =
                @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar";
            IArchive archive;
            using (Handle)
            {
                Handle.Mode = OpenMode.List;
                Handle.Open();
                archive = Archive.Open(Unrar);
            }

            Assert.AreEqual(4, archive.Files.Count);
            Assert.AreEqual(HighMemberFlags.DictionarySize512K, archive.Files[0].HighFlags);
            Assert.AreEqual(new DateTime(634796685340000000), archive.Files[0].LastModificationDate);
            Assert.AreEqual(LowMemberFlags.None, archive.Files[0].LowFlags);
            Assert.AreEqual("TestFolder\\testFile.txt", archive.Files[0].Name);
            Assert.AreEqual(0, archive.Files[0].PackedSize);
            Assert.AreEqual(0, archive.Files[0].UnpackedSize);
            Assert.AreEqual(
                "..\\..\\..\\..\\..\\..\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\TestFolder.rar",
                archive.Files[0].Volume);

            Assert.AreEqual(HighMemberFlags.DictionarySize512K, archive.Files[1].HighFlags);
            Assert.AreEqual(new DateTime(634752216580000000), archive.Files[1].LastModificationDate);
            Assert.AreEqual(LowMemberFlags.None, archive.Files[1].LowFlags);
            Assert.AreEqual("test.txt", archive.Files[1].Name);
            Assert.AreEqual(41, archive.Files[1].PackedSize);
            Assert.AreEqual(297541, archive.Files[1].UnpackedSize);
            Assert.AreEqual(
                "..\\..\\..\\..\\..\\..\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\TestFolder.rar",
                archive.Files[1].Volume);

            Assert.AreEqual(HighMemberFlags.DirectoryRecord, archive.Files[2].HighFlags);
            Assert.AreEqual(new DateTime(634796685220000000), archive.Files[2].LastModificationDate);
            Assert.AreEqual(LowMemberFlags.None, archive.Files[2].LowFlags);
            Assert.AreEqual("TestFolder\\InnerTestFolder", archive.Files[2].Name);
            Assert.AreEqual(0, archive.Files[2].PackedSize);
            Assert.AreEqual(0, archive.Files[2].UnpackedSize);
            Assert.AreEqual(
                "..\\..\\..\\..\\..\\..\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\TestFolder.rar",
                archive.Files[2].Volume);

            Assert.AreEqual(HighMemberFlags.DirectoryRecord, archive.Files[3].HighFlags);
            Assert.AreEqual(new DateTime(634796685380000000), archive.Files[3].LastModificationDate);
            Assert.AreEqual(LowMemberFlags.None, archive.Files[3].LowFlags);
            Assert.AreEqual("TestFolder", archive.Files[3].Name);
            Assert.AreEqual(0, archive.Files[3].PackedSize);
            Assert.AreEqual(0, archive.Files[3].UnpackedSize);
            Assert.AreEqual(
                "..\\..\\..\\..\\..\\..\\Common\\Test\\Oleg.Kleyman.Winrar.Core.Tests.Integration\\TestFolder.rar",
                archive.Files[3].Volume);
        }
    }
}