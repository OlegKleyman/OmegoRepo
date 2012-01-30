using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Tests.Integration
{
    [TestFixture]
    public class RarExtractorTests
    {
        protected DefaultSettings ConfigSettings { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            ConfigSettings = new DefaultSettings();
        }

        [Test]
        public void ExtractTest()
        {
            Extractor extractor = new RarExtractor(ConfigSettings);
            string destination = @"C:\testUnrar\";
            Directory.CreateDirectory(destination);
            extractor.Extract(@"..\..\..\..\..\..\Common\Test\testFile.Rar", destination);
            Assert.IsTrue(File.Exists(destination + "testFile.txt"));
            Directory.Delete(destination, true);
        }
    }
}

