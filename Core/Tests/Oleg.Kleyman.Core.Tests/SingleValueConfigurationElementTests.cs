using System.Collections.Generic;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;

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
    }
}