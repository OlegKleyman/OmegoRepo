using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests
{
    [TestFixture]
    public class UnrarHandleTests : TestsBase
    {
        private Mock<IUnrar> UnrarDllMock { get; set; }
        public RAROpenArchiveDataEx OpenData;
        private RARHeaderDataEx _headerData;
        private const string FILE_PATH_TO_VALID_RAR = "C:\\filePathToValid.rar";

        #region Overrides of TestsBase

        public override void Setup()
        {
            UnrarDllMock = new Mock<IUnrar>();
            OpenData = new RAROpenArchiveDataEx
                           {
                               ArcName = FILE_PATH_TO_VALID_RAR,
                               OpenMode = OpenMode.List
                           };
            _headerData = new RARHeaderDataEx();
            UnrarDllMock.Setup(x => x.RAROpenArchiveEx(ref OpenData)).Returns(new IntPtr(1111));

            _headerData.FileNameW = "test2.txt";
            UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _headerData)).Returns(RarStatus.Success);
            
        }

        #endregion

        [Test]
        public void OpenArchiveTest()
        {
            var unrarHandle = new UnrarHandle(UnrarDllMock.Object, FILE_PATH_TO_VALID_RAR);
            unrarHandle.FileProcessed += validArchiveFileProcessed;
            var archive = unrarHandle.OpenArchive();
            Assert.IsNotNull(archive);
            Assert.AreEqual("C:\\filePathToValid.rar", archive.FilePath);
            Assert.AreEqual(2, archive.Files.Count);
            Assert.AreEqual("test2.txt", archive.Files[0].Name);
            Assert.AreEqual("test.txt", archive.Files[1].Name);
            Assert.AreEqual(unrarHandle, archive.Handle);
        }

        private void validArchiveFileProcessed(object sender, UnrarFileProcessedEventArgs e)
        {
            if (e.UnpackedFile.Name == "test2.txt")
            {
                _headerData.FileNameW = "test.txt";
                UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _headerData)).Returns(RarStatus.Success);
            }

            if (e.UnpackedFile.Name == "test.txt")
            {
                UnrarDllMock.Setup(x => x.RARReadHeaderEx(new IntPtr(1111), out _headerData)).Returns(RarStatus.EndOfArchive);
            }
        }
    }
}
