using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class ReleaseTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void DefaultConstructorTest()
        {
            var release = new Release(ReleaseType.Tv, "Breaking Bad");
            Assert.AreEqual("Breaking Bad", release.Name);
            Assert.AreEqual(ReleaseType.Tv, release.ReleaseType);
        }
    }
}