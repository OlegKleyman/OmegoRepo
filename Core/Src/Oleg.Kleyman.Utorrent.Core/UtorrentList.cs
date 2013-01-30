using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Utorrent.Core
{
    [DataContract]
    public class UTorrentList : UTorrentBase
    {
        public UTorrentList(UtorrentRssFeed feed)
        {
            _feed = new object[1][];
            _feed[0] = new object[7];
            _feed[0][0] = feed.Name;
            _feed[0][6] = string.Format(CultureInfo.InvariantCulture, "{0}|{1}", feed.Name,
                                        Uri.UnescapeDataString(feed.Url.AbsoluteUri));

        }

        [DataMember(Name = "rssfeeds")]
        private readonly object[][] _feed;

        private UtorrentRssFeed _rssFeed;

        public UtorrentRssFeed RssFeed
        {
            get { return (UtorrentRssFeed) _rssFeed.Clone(); }
            private set { _rssFeed = value; }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            RssFeed = new UtorrentRssFeed();
            var targetFeed = _feed[0];
            RssFeed.Id = (int) targetFeed[0];
            var nameUrl = ((string) targetFeed[6]).Split('|');
            RssFeed.Name = nameUrl[0];
            var url = string.Join("|", nameUrl, 1, nameUrl.Length - 1);
            RssFeed.Url = new Uri(url);
        }

    }

    public class UtorrentRssFeed : ICloneable
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Uri Url { get; set; }
        public object Clone()
        {
            var feed = new UtorrentRssFeed();
            feed.Name = Name;
            feed.Id = Id;
            feed.Url = new Uri(Url.AbsoluteUri);

            return feed;
        }
    }
}