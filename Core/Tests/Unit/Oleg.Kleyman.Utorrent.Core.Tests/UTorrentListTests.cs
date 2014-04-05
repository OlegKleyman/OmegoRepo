﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Utorrent.Core.Tests
{
    [TestFixture]
    public class UTorrentListTests : TestsBase
    {
        public override void Setup()
        {
            
        }

        [Test]
        public void ConstructorShouldSetRssFeedProperty()
        {
            var feed = new UtorrentRssFeed
                {
                    Id = 1,
                    Name = "television",
                    Url = new Uri("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)")
                };
            var utorrentList = new UTorrentList(new []{ feed});
            
            Assert.That(utorrentList.RssFeeds, Is.Not.Null);
            Assert.That(utorrentList.RssFeeds[0].Name, Is.EqualTo("television"));
            Assert.That(utorrentList.RssFeeds[0].Id, Is.EqualTo(1));
            Assert.That(utorrentList.RssFeeds[0].Url.AbsoluteUri, Is.EqualTo("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)"));
        }

        [Test]
        public void RssFeedPropertyShouldReturnAClonedInstance()
        {
            var feed = new UtorrentRssFeed
            {
                Id = 1,
                Name = "television",
                Url = new Uri("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)")
            };
            var utorrentList = new UTorrentList(new[]{ feed });
            Assert.That(utorrentList.RssFeeds[0], Is.Not.SameAs(feed));
        }
    }
}
