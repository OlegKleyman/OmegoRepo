using NUnit.Framework;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class SingletonTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void InstanceTest()
        {
            Singleton instance = Singleton.Instance;
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<Singleton>(instance);
        }

        [Test]
        public void IsSingletonTest()
        {
            Singleton instance1 = Singleton.Instance;
            Singleton instance2 = Singleton.Instance;
            Assert.AreEqual(instance1, instance2);
        }
    }
}