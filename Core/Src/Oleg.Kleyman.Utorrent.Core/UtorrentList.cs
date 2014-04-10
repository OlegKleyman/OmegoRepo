using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="feeds">An array of <see cref="UtorrentRssFeed"/> object to initialize the object with.</param>
        /// <param name="torrents">An array of <see cref="Torrent"/> object to initialize the object with.</param>
        public UTorrentList(UtorrentRssFeed[] feeds, Torrent[] torrents)
        {
            if (feeds == null)
            {
                const string feedParamName = "feeds";
                throw new ArgumentNullException(feedParamName);
            }
            if (torrents == null)
            {
                const string torrentsParamName = "torrents";
                throw new ArgumentNullException(torrentsParamName);
            }

            RssFeeds = feeds;
            Torrents = torrents;
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
            get
            {
                if (_list != null)
                {
                    return _list;
                }
                _list = new object[RssFeeds.Length][];
                for (int i = 0; i < RssFeeds.Length; i++)
                {
                    _list[i] = RssFeeds[i].ToArray();
                }
                return _list;
            }
            set
            {
                //only setting the RssFeeds property here.
                //This is needed for cleanliness because the Utorrent API
                //returns an object array.
                RssFeeds = value.Select(feed => (UtorrentRssFeed)feed).ToArray();
            }
        }

        private object[][] _torrentList;

        /// <summary>
        /// Gets or sets the Torrent property of this object. Converts an object array to <see cref="Torrent"/>
        /// or gets the object array representation of the Torrent property.
        /// </summary>
        /// <remarks>
        /// UTorrent api returns everything in an array. This mess should not be exposed to the public API. 
        /// Use Torrent property when accessing the public API instead. This property cannot be unit tested.
        /// </remarks>
        [DataMember(Name = "torrents")]
        private object[][] TorrentsApi
        {
            get
            {
                if (_torrentList != null)
                {
                    return _torrentList;
                }
                _torrentList = new object[Torrents.Length][];
                for (int i = 0; i < Torrents.Length; i++)
                {
                    _torrentList[i] = Torrents[i].ToArray();
                }
                return _torrentList;
            }
            set
            {
                //only setting the RssFeeds property here.
                //This is needed for cleanliness because the Utorrent API
                //returns an object array.
                Torrents = value.Select(torrent => (Torrent)torrent).ToArray();
            }
        }

        private UtorrentRssFeed[] _rssFeeds;
        
        /// <summary>
        /// Gets the RSS feeds
        /// </summary>
        public UtorrentRssFeed[] RssFeeds
        {
            get
            {
                //Returning the cloned object so it doesn't get modified by the consumer.
                return _rssFeeds.Select(feed => (UtorrentRssFeed) feed.Clone()).ToArray();
            }
            private set { _rssFeeds = value; }
        }

        private Torrent[] _torrents;

        /// <summary>
        /// Gets the Torrents
        /// </summary>
        public Torrent[] Torrents
        {
            get
            {
                //Returning the cloned object so it doesn't get modified by the consumer.
                return _torrents.Select(torrent => (Torrent)torrent.Clone()).ToArray();
            }
            private set { _torrents = value; }
        }
    }
}