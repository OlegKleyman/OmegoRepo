using Moq;
using Ninject;
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

        public override void Setup()
        {
            _fileSystem = new Mock<IFileSystem>();
            _extractor = new Mock<Extractor>();
        }

        [Test]
        [Ignore("Code is not complete. Results in a null reference.")]
        public void CopyTest()
        {
            var dependencies = new XbmcFileCopierDependencies(_extractor.Object, _fileSystem.Object);
            var kernel = new StandardKernel();
            kernel.Bind<ISettingsProvider>().ToMethod(x => new TestSettings());
            var copier = new XbmcFileCopier(dependencies, kernel);
            var release = new Release(ReleaseType.Tv, "Breaking Bad");
            var releaseOutput = new ReleaseOutput("Breaking.Bad.S01E07.720p.mkv", @"C:\TV", release);
            var output = copier.Copy(releaseOutput);
            Assert.AreEqual(@"C:\TV\Breaking Bad", output.TargetDirectory);
            Assert.AreEqual(@"Breaking.Bad.S01E07.720p.mkv", output.FileName);
        }
    }
}