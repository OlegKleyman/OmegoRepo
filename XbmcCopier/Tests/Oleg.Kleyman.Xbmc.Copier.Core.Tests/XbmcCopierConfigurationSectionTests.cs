using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class XbmcCopierConfigurationSectionTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {

        }

        [Test]
        public void ConstructorTest()
        {
            var configElementCollection = new XbmcCopierConfigurationSection();
            Assert.Pass();
        }

        [Test]
        public void SettingsTest()
        {
            var settings = XbmcCopierConfigurationSection.Settings;
        }
    }
}
