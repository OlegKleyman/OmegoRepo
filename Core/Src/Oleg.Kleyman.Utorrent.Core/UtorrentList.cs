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

            RssFeed = feed;

        }

        private object[][] _list;

        /// <summary>
        /// Gets or sets the RssFeed property of this object. Converts an object array to <see cref="UtorrentRssFeed"/>
        /// or gets the object array representation of the RssFeed property.
        /// </summary>
        /// <remarks>
        /// UTorrent api returns everything in an array. This mess should not be exposed to the public API. 
        /// Use RssFeed property when accessing the public API instead. This property cannot be unit tested.
        /// </remarks>
        [DataMember(Name = "rssfeeds")]
        private object[][] List
        {
            get { return _list ?? (_list = new[] { RssFeed.ToArray() }); }
            set
            {
                //only setting the RssFeed property here.
                //This is needed for cleanliness because the Utorrent API
                //returns an object array.
                RssFeed = (UtorrentRssFeed)value[0];
            }
        }

        private UtorrentRssFeed _rssFeed;

        /// <summary>
        /// Gets the RSS feed
        /// </summary>
        public UtorrentRssFeed RssFeed
        {
            get
            {
                //Returning the cloned object so it doesn't get modified by the consumer.
                return (UtorrentRssFeed)_rssFeed.Clone();
            }
            private set { _rssFeed = value; }
        }
    }
}