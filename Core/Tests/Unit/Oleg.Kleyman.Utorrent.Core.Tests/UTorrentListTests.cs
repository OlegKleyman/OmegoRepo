using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class UTorrentListTests : TestsBase
    {
        private UtorrentRssFeed _utorrentRssFeed;
        private Torrent _torrent;

        [TestFixtureSetUp]
        public override void Setup()
        {
            _utorrentRssFeed = new UtorrentRssFeed
            {
                Id = 1,
                Name = "television",
                Url = new Uri("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)")
            };

            var file = new TorrentFile();
            file.Name = "testFile.mkv";
            _torrent = new Torrent(TorrentHash.Parse("FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC"), new []{ file });
        }

        [Test]
        public void ConstructorShouldSetRssFeedProperty()
        {
            var utorrentList = GetUtorrentList(_utorrentRssFeed, _torrent);
            
            Assert.That(utorrentList.RssFeeds, Is.Not.Null);
            Assert.That(utorrentList.RssFeeds[0].Name, Is.EqualTo("television"));
            Assert.That(utorrentList.RssFeeds[0].Id, Is.EqualTo(1));
            Assert.That(utorrentList.RssFeeds[0].Url.AbsoluteUri, Is.EqualTo("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)"));
        }

        [Test]
        public void RssFeedsPropertyShouldReturnAClonedInstance()
        {
            var utorrentList = GetUtorrentList(_utorrentRssFeed, _torrent);
            Assert.That(utorrentList.RssFeeds[0], Is.Not.SameAs(_utorrentRssFeed));
        }

        [Test]
        public void TorrentsPropertyShouldReturnAClonedInstance()
        {
            var utorrentList = GetUtorrentList(_utorrentRssFeed, _torrent);
            Assert.That(utorrentList.Torrents[0], Is.Not.SameAs(_torrent));
        }

        private static UTorrentList GetUtorrentList(UtorrentRssFeed feed, Torrent torrent)
        {
            return new UTorrentList(new[] { feed }, new[] { torrent });
        }
    }
}
