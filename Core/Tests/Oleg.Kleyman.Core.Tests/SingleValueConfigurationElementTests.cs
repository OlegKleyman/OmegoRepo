using System.Collections.Generic;
using NUnit.Framework;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class SingleValueConfigurationElementTests
    {
        private IDictionary<string, object> PropertyNameValues { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            PropertyNameValues = new Dictionary<string, object>
                                     {
                                         {"value", "test"}
                                     };
        }

        [Test]
        public void ConstructorTest()
        {
            var element = new SingleValueConfigurationElement(PropertyNameValues);
            Assert.AreEqual("test", element.Value);
        }

        [Test]
        public void GetHashCodeTest()
        {
            var element = new SingleValueConfigurationElement(PropertyNameValues);

            
        }
    }
}
