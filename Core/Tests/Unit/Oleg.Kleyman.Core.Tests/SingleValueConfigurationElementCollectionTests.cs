using System;
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
        [ExpectedException(ExpectedException = typeof (ArgumentNullException),
            ExpectedExceptionName = "System.ArgumentNullException",
            ExpectedMessage = "Value cannot be null.\r\nParameter name: elements",
            MatchType = MessageMatch.Exact)]
        public void ConstructorNullArgumentTest()
        {
            new SingleValueConfigurationElementCollection<SingleValueConfigurationSection>(null);
        }

        [Test]
        public void ConstructorTest()
        {
            var element = new SingleValueConfigurationSection(PropertyNameValues);

            var configCollection =
                new SingleValueConfigurationElementCollection<SingleValueConfigurationSection>(new[] {element});
            Assert.AreEqual(1, configCollection.Count);
            Assert.IsInstanceOf<SingleValueConfigurationSection>(configCollection[0]);
            Assert.AreEqual("test", configCollection[0].Value);
        }

        [Test]
        public void PrivateConstructorTest()
        {
            var configCollection = (SingleValueConfigurationElementCollection<SingleValueConfigurationSection>)Activator.CreateInstance(typeof(SingleValueConfigurationElementCollection<SingleValueConfigurationSection>), true);
            Assert.AreEqual(0, configCollection.Count);
        }
    }
}