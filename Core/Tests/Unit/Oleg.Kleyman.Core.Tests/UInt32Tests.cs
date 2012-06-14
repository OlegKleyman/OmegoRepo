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
            const uint source = 1062684718;
            var result = source.ToDate();
            Assert.AreEqual(634549428880000000, result.Ticks);
        }

        [Test]
        public void JoinWithLeftTest()
        {
            const uint right = 2515968000;

            Assert.AreEqual(6810935296, right.JoinWithLeft(1));
        }

        [Test]
        public void JoinWithRightTest()
        {
            const uint left = 1;
            Assert.AreEqual(6810935296, left.JoinWithRight(2515968000));
        }
    }
}
