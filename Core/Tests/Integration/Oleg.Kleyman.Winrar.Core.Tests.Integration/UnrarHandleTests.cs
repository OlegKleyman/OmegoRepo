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

        #region Overrides of TestsBase

        public override void Setup()
        {
            UnrarDll = new UnrarDll();
            RarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
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
            Assert.AreEqual("test.txt", archive.Files[1].Name);
            unrarHandle.Close();
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
