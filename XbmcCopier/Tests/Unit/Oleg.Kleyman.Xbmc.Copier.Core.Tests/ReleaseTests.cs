using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core.Tests
{
    [TestFixture]
    public class ReleaseTests : TestsBase
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
            Assert.AreEqual("Breaking Bad", release.Name);
            Assert.AreEqual(ReleaseType.Tv, release.ReleaseType);
        }
    }
}
