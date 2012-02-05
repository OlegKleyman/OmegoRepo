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
            ISettingsProvider settings = XbmcCopierConfigurationSection.Settings;
        }
    }
}