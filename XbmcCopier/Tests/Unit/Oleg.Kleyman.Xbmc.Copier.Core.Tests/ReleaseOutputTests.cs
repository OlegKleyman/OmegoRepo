using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class ReleaseOutputTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void DefaultConstructorTest()
        {
            var release = new Release(ReleaseType.Tv, "Breaking Bad");
            var releaseOutput = new ReleaseOutput("breaking.bad.S01E07.720P", @"C:\downloadPath", release);
            Assert.AreEqual("Breaking Bad", releaseOutput.Release.Name);
            Assert.AreEqual(ReleaseType.Tv, releaseOutput.Release.ReleaseType);
            Assert.AreEqual("breaking.bad.S01E07.720P", releaseOutput.FileName);
            Assert.AreEqual(@"C:\downloadPath", releaseOutput.TargetDirectory);
        }
    }
}