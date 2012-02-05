using System.Collections.Generic;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class SingleValueConfigurationElementCollectionTests
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

            var configCollection =
                new SingleValueConfigurationElementCollection<SingleValueConfigurationElement>(new[] {element});
            Assert.AreEqual(1, configCollection.Count);
            Assert.IsInstanceOf<SingleValueConfigurationElement>(configCollection[0]);
            Assert.AreEqual("test", configCollection[0].Value);
        }
    }
}