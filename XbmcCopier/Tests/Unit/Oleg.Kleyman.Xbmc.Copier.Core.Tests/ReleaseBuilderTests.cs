using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class ReleaseBuilderTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void BuildMovieTest()
        {
            var builder = new ReleaseBuilder(new TestSettings(), "The.Matix.1999.1080p.BluRay");
            var release = builder.Build();
            Assert.AreEqual("The.Matix.1999.1080p.BluRay", release.Name);
            Assert.AreEqual(ReleaseType.Movie, release.ReleaseType);
        }

        [Test]
        public void BuildTvTest()
        {
            var builder = new ReleaseBuilder(new TestSettings(), "Breaking.Bad.S01E07.720p");
            var release = builder.Build();
            Assert.AreEqual("Breaking Bad", release.Name);
            Assert.AreEqual(ReleaseType.Tv, release.ReleaseType);
        }

        [Test]
        public void DefaultConstructorTest()
        {
            var builder = new ReleaseBuilder(new TestSettings(), "Breaking.Bad.S01E07.720p");
            Assert.AreEqual("Breaking.Bad.S01E07.720p", builder.Name);
        }
    }
}