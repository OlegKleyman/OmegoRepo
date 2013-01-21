using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class TorrentFileTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void ExplicitCastOperatorShouldConvertObjectArrayTest()
        {
            var properties = new object[]
                {
                    "daa-alvh-1080p.mkv",
                    8210651843, 8210651843,
                    2,
                    0,
                    490,
                    true,
                    621537,
                    6300,
                    0,
                    0,
                    -1,
                    498125947598274559
                };

            var file = (TorrentFile) properties;
            Assert.AreEqual("daa-alvh-1080p.mkv", file.Name);
        }

        [Test]
        [ExpectedException(typeof (InvalidCastException), ExpectedMessage = "Unable to convert a 0 length array.",
            MatchType = MessageMatch.Exact)]
        public void ExplicitCastOperatorShouldThrowInvalidCastExceptionWhenArrayIsWrongLengthTest()
        {
            var file = (TorrentFile) new object[] {};
        }

        [Test]
        [ExpectedException(typeof (InvalidCastException), ExpectedMessage = "First element containing Name is null.",
            MatchType = MessageMatch.Exact)]
        public void ExplicitCastOperatorShouldThrowInvalidCastExceptionWhenFirstElementNameIsNullTest()
        {
            var properties = new object[]
                {
                    null,
                    8210651843, 8210651843,
                    2,
                    0,
                    490,
                    true,
                    621537,
                    6300,
                    0,
                    0,
                    -1,
                    498125947598274559
                };
            var file = (TorrentFile) properties;
        }
    }
}