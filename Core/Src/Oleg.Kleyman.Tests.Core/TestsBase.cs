using NUnit.Framework;

namespace Oleg.Kleyman.Tests.Core
{
    public abstract class TestsBase
    {
        [TestFixtureSetUp]
        public abstract void Setup();

        [Test]
        public abstract void CheckCoverage();
    }
}