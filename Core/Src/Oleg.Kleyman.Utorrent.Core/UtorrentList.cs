using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents a UTorrent list object
    /// </summary>
    [DataContract]
    public class UTorrentList : UTorrentBase
    {
        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="feed">A <see cref="UtorrentRssFeed"/> object to initialize the object with.</param>
        public UTorrentList(UtorrentRssFeed feed)
        {
            if (feed.Url == null)
            {
                const string feedParamName = "feed";
                throw new ArgumentNullException(feedParamName);
            }

            _list = new object[1][];
            _list[0] = new object[7];
            _list[0][0] = feed.Id;
            _list[0][6] = string.Format(CultureInfo.InvariantCulture, "{0}|{1}", feed.Name,
                                        Uri.UnescapeDataString(feed.Url.AbsoluteUri));

        }

        /// <summary>
        /// Contains the rssfeed array
        /// </summary>
        /// <remarks>UTorrent api returns everything in an array. This mess should not be exposed to the public API. Use RssFeed property instead.</remarks>
        [DataMember(Name = "rssfeeds")]
        private readonly object[][] _list;

        private UtorrentRssFeed _rssFeed;

        /// <summary>
        /// Gets the RSS feed
        /// </summary>
        public UtorrentRssFeed RssFeed
        {
            get { return (UtorrentRssFeed)_rssFeed.Clone(); }
        }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="context">Argument not required.</param>
        [OnDeserialized]
        public void Initialize(StreamingContext context)
        {
            _rssFeed = (UtorrentRssFeed)_list[0];
            
        }
    }
}