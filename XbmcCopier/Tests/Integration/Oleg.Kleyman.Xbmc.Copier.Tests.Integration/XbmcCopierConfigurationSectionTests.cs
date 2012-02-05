using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration
{
    [TestFixture]
    public class XbmcCopierConfigurationSectionTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void SettingsTest()
        {
            var settings = XbmcCopierConfigurationSection.Settings;
            Assert.AreEqual(@"C:\Program Files\WinRAR\unrar.exe", settings.UnrarPath);
            Assert.AreEqual(@"C:\Videos\Movies", settings.MoviesPath);
            Assert.AreEqual(@"C:\Videos\Tv", settings.TvPath);
            Assert.AreEqual(2, settings.Filters.Length);
            Assert.AreEqual("testing", settings.Filters[0]);
            Assert.AreEqual("second test", settings.Filters[1]);
        }
    }
}
