using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class XbmcFileCopierTests : TestsBase
    {
        private Mock<IFileSystem> _fileSystem;
        private Mock<Extractor> _extractor;

        #region Overrides of TestsBase

        public override void Setup()
        {
            _fileSystem = new Mock<IFileSystem>();
            _extractor = new Mock<Extractor>();
        }

        #endregion

        [Test]
        [Ignore("Code is not complete. Results in a null reference.")]
        public void CopyTest()
        {
            var dependencies = new XbmcFileCopierDependencies(_extractor.Object, _fileSystem.Object);
            var copier = new XbmcFileCopier(new TestSettings(), dependencies);
            var release = new Release(ReleaseType.Tv, "Breaking Bad");
            var releaseOutput = new ReleaseOutput("Breaking.Bad.S01E07.720p.mkv", @"C:\TV", release);
            Output output = copier.Copy(releaseOutput);
            Assert.AreEqual(@"C:\TV\Breaking Bad", output.TargetDirectory);
            Assert.AreEqual(@"Breaking.Bad.S01E07.720p.mkv", output.FileName);
        }
    }
}
