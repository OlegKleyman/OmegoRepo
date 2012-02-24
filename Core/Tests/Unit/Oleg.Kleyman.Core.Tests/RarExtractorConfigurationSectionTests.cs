using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class RarExtractorConfigurationSectionTests
    {
        private IDictionary<string, object> PropertyNameValues { get; set; }
        protected Mock<RarExtractorConfigurationSection> MockRarExtractorConfigurationSection { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            PropertyNameValues = new Dictionary<string, object>
                                     {
                                         {"unrarPath", "test"}
                                     };
            MockRarExtractorConfigurationSection = new Mock<RarExtractorConfigurationSection>();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ArgumentNullException),
            ExpectedExceptionName = "System.ArgumentNullException",
            ExpectedMessage = "Value cannot be null.\r\nParameter name: values",
            MatchType = MessageMatch.Exact)]
        public void ConstructorNullArgumentTest()
        {
            new RarExtractorConfigurationSection(null);
        }

        [Test]
        public void ConstructorTest()
        {
            var configurationSection = new RarExtractorConfigurationSection(PropertyNameValues);

            Assert.AreEqual(1, configurationSection.ElementInformation.Properties.Count);
            Assert.AreEqual("test", configurationSection.Value);
            Assert.AreEqual("test", ((IRarExtractorSettings) configurationSection).UnrarPath);
        }

        [Test]
        public void DefaultConstructorTest()
        {
            var result = MockRarExtractorConfigurationSection.Object;
            Assert.AreEqual(null, result.Value);
        }
    }
}