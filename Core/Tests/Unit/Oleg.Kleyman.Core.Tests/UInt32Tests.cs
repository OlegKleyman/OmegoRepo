using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class UInt32Tests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {
            
        }

        #endregion

        [Test]
        public void ToDateTest()
        {
            uint source = 1062684718;
            var result = source.ToDate();
            Assert.AreEqual(634549572880000000, result.Ticks);
        }
    }
}
