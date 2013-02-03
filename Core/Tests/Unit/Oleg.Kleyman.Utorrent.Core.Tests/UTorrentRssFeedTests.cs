using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class UTorrentRssFeedTests: TestsBase
    {
        public override void Setup()
        {
            
        }

        [Test]
        public void CastOperatorShouldConvertAValidArrayToAuTorrentRssFeedObject()
        {
            var feedArray = new object[]{1, 
                                         null, null, null, null, null, 
                                         "television|http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p|1080p)&interval=12+days&exclude=(season)"};
            var feed = (UtorrentRssFeed) feedArray;
            Assert.That(feed.Name, Is.EqualTo("television"));
            Assert.That(feed.Id, Is.EqualTo(1));
            Assert.That(feed.Url.AbsoluteUri, Is.EqualTo("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)"));
        }



        [Test]
        public void CastOperatorShouldThrowInvalidCastExceptionWhenNameUrlIsMissingPipeSymbol()
        {
            var feedArray = new object[]{1, 
                                         null, null, null, null, null, 
                                         string.Empty};
            var ex = Assert.Throws<InvalidCastException>(() => { var x = (UtorrentRssFeed)feedArray; });

            Assert.That(ex.Message, Is.EqualTo("Name/URL value format is invalid expected \"|\" symbol"));
        }

        [Test]
        public void CastOperatorShouldThrowInvalidCastExceptionWhenUrlFormatIsInvalid()
        {
            var feedArray = new object[]{1, 
                                         null, null, null, null, null, 
                                         "television|"};
            var ex = Assert.Throws<InvalidCastException>(() => { var x = (UtorrentRssFeed)feedArray; });

            Assert.That(ex.Message, Is.EqualTo("Invalid URL"));
        }
    }
}
