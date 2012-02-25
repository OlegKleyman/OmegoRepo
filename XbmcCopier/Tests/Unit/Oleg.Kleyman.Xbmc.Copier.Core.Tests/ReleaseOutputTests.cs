using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class ReleaseOutputTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        [Test]
        public void DefaultConstructorTest()
        {
            var release = new Release(ReleaseType.Tv, "Breaking Bad");
            var releaseOutput = new ReleaseOutput(release, "breaking.bad.S01E07.720P", @"C:\downloadPath");
            Assert.AreEqual("Breaking Bad", releaseOutput.Release.Name);
            Assert.AreEqual(ReleaseType.Tv, releaseOutput.Release.ReleaseType);
            Assert.AreEqual("breaking.bad.S01E07.720P", releaseOutput.FileName);
            Assert.AreEqual(@"C:\downloadPath", releaseOutput.DownloadPath);
        }
    }
}
