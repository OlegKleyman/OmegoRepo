using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using NUnit.Framework;
using Oleg.Kleyman.Core.Linq;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [Binding]
    public class UtorrentUpdateRssFeedsSteps
    {
        private const string SERVICE_TOKEN_KEY = "SERVICE_TOKEN";
        private UTorrentList UTorrentList { get; set; }

        [Given(@"Retrieved all RSS feeds")]
        public void GivenRetrievedAllRssFeeds()
        {
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);
            UTorrentList = UtorrentSteps.ServiceClient.GetList(token);
            Assert.That(UTorrentList.BuildNumber, Is.EqualTo(30303));
            Assert.That(UTorrentList.RssFeeds, Is.Not.Null);
            Assert.That(UTorrentList.RssFeeds[0].Name, Is.EqualTo("television"));
            Assert.That(UTorrentList.RssFeeds[0].Id, Is.EqualTo(1));
            Assert.That(UTorrentList.RssFeeds[0].Url.AbsoluteUri, Is.EqualTo("http://theomegoone.net/testFeed.rss"));
        }

        [Then(@"I want to update all RSS feeds")]
        public void ThenIWantToUpdateAllRssFeeds()
        {
            var token = ScenarioContext.Current.Get<string>(SERVICE_TOKEN_KEY);

            UTorrentList.RssFeeds.ForEach(feed =>
            {
                var result = UtorrentSteps.ServiceClient.UpdateRssFeed(token, feed.Id);
                Assert.That(result.BuildNumber, Is.EqualTo(30303));
            });
        }
    }
}
