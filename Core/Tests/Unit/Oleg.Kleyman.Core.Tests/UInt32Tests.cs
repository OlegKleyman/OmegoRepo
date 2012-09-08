using NUnit.Framework;
using Oleg.Kleyman.Core.Linq;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class UInt32Tests : TestsBase
    {
        public override void Setup()
        {
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

        [Test]
        public void ToDateTest()
        {
            const uint source = 1062684718;
            var result = source.ToDate();
            Assert.AreEqual(634549428880000000, result.Ticks);
        }
    }
}