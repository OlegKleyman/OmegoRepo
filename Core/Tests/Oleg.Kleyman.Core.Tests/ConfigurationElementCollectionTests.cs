using System.Collections.Generic;
using NUnit.Framework;

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
                                         {"value", "test"}
                                     };
        }

        [Test]
        public void ConstructorTest()
        {
            var element = new SingleValueConfigurationElement(PropertyNameValues);
            var configCollection = new ConfigurationElementCollection<SingleValueConfigurationElement>(new[] { element });
            Assert.AreEqual(1, configCollection.Count);
            
            Assert.Pass();
        }
    }
}
