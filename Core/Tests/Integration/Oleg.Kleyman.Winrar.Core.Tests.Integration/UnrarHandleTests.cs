using System;
using System.IO;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    internal class UnrarHandleTests : TestsBase
    {
        private IUnrarDll UnrarDll { get; set; }
        private string RarFilePath { get; set; }
        private string InvalidRarFilePath { get; set; }
        private string BrokenRarFilePath { get; set; }
        protected IUnrarWrapper Wrapper { get; set; }
        private IPathBuilder PathBuilder { get; set; }
        private IPathBuilder DestinationPathBuilder { get; set; }

        public override void Setup()
        {
            UnrarDll = new NativeMethods();
            PathBuilder = new PathBuilder();
            DestinationPathBuilder = new DestinationPathBuilder(PathBuilder);
            Wrapper = new UnrarWrapper(UnrarDll, DestinationPathBuilder);

            RarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Test.part1.rar");
            InvalidRarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\test.txt");
            BrokenRarFilePath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\testFileCorrupt.rar");
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Unrar handle is not open.",
            MatchType = MessageMatch.Exact)]
        public void CloseAlreadyOpenExceptionTest()
        {
            var unrarHandle = new UnrarHandle(Wrapper, BrokenRarFilePath);
            unrarHandle.Close();
        }

        [Test]
        public void CloseTest()
        {
            var unrarHandle = new UnrarHandle(Wrapper, RarFilePath);
            unrarHandle.Open();
            unrarHandle.Close(); //No exception assumes success
        }

        [Test]
        public void DisposeTest()
        {
            using (var unrarHandle = new UnrarHandle(Wrapper, RarFilePath))
            {
                unrarHandle.Open();
            }

            //No exception assumes that the object was disposed of correctly
        }

        [Test]
        public void OpenArchiveTest()
        {
            var unrarHandle = new UnrarHandle(Wrapper, RarFilePath);
            unrarHandle.Open();

            unrarHandle.Close();
        }

        [Test]
        [ExpectedException(typeof (UnrarException), ExpectedMessage = "Unable to open archive.",
            MatchType = MessageMatch.Exact)]
        public void OpenArchiveUnknownFormatTest()
        {
            var unrarHandle = new UnrarHandle(Wrapper, InvalidRarFilePath);
            try
            {
                unrarHandle.Open();
            }
            catch (UnrarException ex)
            {
                Assert.AreEqual(RarStatus.BadArchive, ex.Status);
                throw;
            }
        }
    }
}