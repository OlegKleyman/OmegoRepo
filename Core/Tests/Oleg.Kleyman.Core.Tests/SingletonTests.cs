using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var instance = Singleton.Instance;
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<Singleton>(instance);
        }

        [Test]
        public void IsSingletonTest()
        {
            var instance1 = Singleton.Instance;
            var instance2 = Singleton.Instance;
            Assert.AreEqual(instance1, instance2);
        }
    }
}
