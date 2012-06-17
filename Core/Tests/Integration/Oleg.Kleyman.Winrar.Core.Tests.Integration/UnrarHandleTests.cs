using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    class UnrarHandleTests : TestsBase
    {
        private IUnrar UnrarDll { get; set; }
        private string RarFilePath { get; set; }
        private string InvalidRarFilePath { get; set; }
        private string BrokenRarFilePath { get; set; }

        #region Overrides of TestsBase

        public override void Setup()
        {
            UnrarDll = new UnrarDll();
            RarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            InvalidRarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\test.txt");
            BrokenRarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFileCorrupt.rar");
        }

        #endregion

        [Test]
        public void OpenArchiveTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDll, RarFilePath);
            var archive = unrarHandle.OpenArchive();
            Assert.IsNotNull(archive);
            Assert.AreEqual(RarFilePath, archive.FilePath);
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

            unrarHandle.Close();
        }

        [Test]
        [ExpectedException(typeof(UnrarException), ExpectedMessage = "Unable to open archive.", MatchType = MessageMatch.Exact)]
        public void OpenArchiveUnknownFormatTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDll, InvalidRarFilePath);
            try
            {
                unrarHandle.OpenArchive();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadArchive, ex.Status);
                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Unrar handle is not open.", MatchType = MessageMatch.Exact)]
        public void CloseAlreadyOpenExceptionTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDll, BrokenRarFilePath);
            unrarHandle.Close();
        }

        [Test]
        [ExpectedException(typeof(UnrarException), ExpectedMessage = "Unable to read header data.", MatchType = MessageMatch.Exact)]
        public void OpenArchiveCorruptTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDll, BrokenRarFilePath);
            try
            {
                unrarHandle.OpenArchive();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadData, ex.Status);
                throw;
            }
            finally
            {
                if(unrarHandle.IsOpen)
                {
                    unrarHandle.Close();
                }
            }
        }

        [Test]
        public void CloseTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDll, RarFilePath);
            var archive = unrarHandle.OpenArchive();
            Assert.IsNotNull(archive);
            unrarHandle.Close(); //No exception assumes success
        }

        [Test]
        public void DisposeTest()
        {
            using (var unrarHandle = new UnrarHandle(UnrarDll, RarFilePath))
            {
                var archive = unrarHandle.OpenArchive();
                Assert.IsNotNull(archive);
            }

            //No exception assumes that the object was disposed of correctly
        }
    }
}
