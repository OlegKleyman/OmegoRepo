using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class FilterConfigurationElementTests
    {
        private IDictionary<string, object> PropertyNameValues { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            PropertyNameValues = new Dictionary<string, object>
                                     {
                                         {"filter", "test"}
                                     };
        }

        [Test]
        public void ConstructorTest()
        {
            var element = new FilterConfigurationElement(PropertyNameValues);
            Assert.AreEqual("test", element.Value);
        }
    }
}
