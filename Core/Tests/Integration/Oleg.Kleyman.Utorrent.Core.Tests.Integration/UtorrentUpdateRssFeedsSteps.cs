using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [Binding]
    public class UtorrentUpdateRssFeedsSteps
    {
        private UTorrentList UTorrentList { get; set; }

        [Given(@"Retrieved all RSS feeds")]
        public void GivenRetrievedAllRssFeeds()
        {
            UTorrentList = UtorrentSteps.ServiceClient.GetList(UtorrentSteps.Key);
            Assert.That(UTorrentList.RssFeed, Is.Not.Null);
            Assert.That(UTorrentList.RssFeed.Name, Is.EqualTo("television"));
            Assert.That(UTorrentList.RssFeed.Id, Is.EqualTo(1));
            Assert.That(UTorrentList.RssFeed.Url.AbsoluteUri, Is.EqualTo("http://www.tvtorrents.com/mytaggedRSS?digest=fe63619e27875fe75ccdaf5968dd094a1397a8b8&hash=16384e56b48898aaf5a3fde5595e2a3a8e2060a7&include=(720p%7C1080p)&interval=12+days&exclude=(season)"));
        }

        [Then(@"I want to update all RSS feeds")]
        public void ThenIWantToUpdateAllRssFeeds()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
