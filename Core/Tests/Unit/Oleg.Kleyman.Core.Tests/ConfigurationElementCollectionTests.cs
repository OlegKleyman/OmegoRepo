using System.Collections.Generic;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class ConfigurationElementCollectionTests
    {
        private IDictionary<string, object> PropertyNameValues { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            PropertyNameValues = new Dictionary<string, object>
                                     {
                                         {"value", "test"},
                                         {"key", "someKey"}
                                     };
        }

        [Test]
        public void ConstructorTest()
        {
            var element = new MockConfigurationElement(PropertyNameValues);
            var configCollection = new ConfigurationElementCollection<MockConfigurationElement>(new[] {element});
            Assert.AreEqual(1, configCollection.Count);
            Assert.IsInstanceOf<MockConfigurationElement>(configCollection[0]);
            Assert.AreEqual("test", (configCollection[0]).Value);
            Assert.AreEqual("someKey", (configCollection[0]).Key);
        }

        [Test]
        public void IntIndexPropertyTest()
        {
            var element = new MockConfigurationElement(PropertyNameValues);
            var configCollection = new ConfigurationElementCollection<MockConfigurationElement>(new[] {element});
            Assert.AreEqual(1, configCollection.Count);
            Assert.IsInstanceOf<MockConfigurationElement>(configCollection[0]);
            Assert.AreEqual("test", (configCollection[0]).Value);
            Assert.AreEqual("someKey", (configCollection[0]).Key);
        }

        [Test]
        public void StringIndexPropertyTest()
        {
            var element = new MockConfigurationElement(PropertyNameValues);
            var configCollection = new ConfigurationElementCollection<MockConfigurationElement>(new[] {element});
            Assert.AreEqual(1, configCollection.Count);
            Assert.IsInstanceOf<MockConfigurationElement>(configCollection["someKey"]);
            Assert.AreEqual("test", (configCollection["someKey"]).Value);
            Assert.AreEqual("someKey", (configCollection["someKey"]).Key);
        }
    }
}