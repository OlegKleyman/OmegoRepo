using System;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents a UTorrent RSS feed object.
    /// </summary>
    public class UtorrentRssFeed : ICloneable
    {
        private const int NAME_INDEX = 0;
        private const int ID_INDEX = 0;
        private const int NAME_URL_INDEX = 6;
        private const int URL_INDEX = 1;

        /// <summary>
        /// Gets or sets the name of the feed.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the feed.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the URL of the feed.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var feed = new UtorrentRssFeed();
            feed.Name = Name;
            feed.Id = Id;
            feed.Url = new Uri(Url.AbsoluteUri);

            return feed;
        }

        /// <summary>
        /// Casts an object array to a <see cref="UtorrentRssFeed"/> object.
        /// </summary>
        /// <param name="targetFeed">The target <see cref="object"/> array to cast.</param>
        /// <returns>A <see cref="UtorrentRssFeed"/> object</returns>
        /// <exception cref="InvalidCastException">When the target array format is invalid.</exception>
        public static explicit operator UtorrentRssFeed(object[] targetFeed)
        {
            var feed = new UtorrentRssFeed();

            var validator = new UtorrentRssFeedObjectValidator(targetFeed);

            if (!validator.Validate())
            {
                throw new InvalidCastException(validator.Message);
            }

            feed.Id = (int)targetFeed[ID_INDEX];

            //The name and URL are kept in a single array element delimeted by a "|" character
            var nameUrl = (string)targetFeed[NAME_URL_INDEX];
            SetNameUrl(feed, nameUrl);

            return feed;
        }

        private static void SetNameUrl(UtorrentRssFeed feed, string nameUrlContent)
        {
            //The properties of the rss feed are kept in an array and thus the mess.

            //The name and URL are delimeted by a "|" character
            //The "|" character can be stored in the URL of the RSS feed in UTorrent
            //even though it's not a valid character in URLs. Thus the split string array
            //must be limited to 2 elements because array must only contain the name and URL.
            const int maximumAmountOfElements = 2;
            var nameUrl = nameUrlContent.Split(new[] { '|' }, maximumAmountOfElements);
            if (nameUrl.Length != 2)
            {
                const string invalidFormatPipeMessage = "Name/URL value format is invalid expected \"|\" symbol";
                throw new InvalidCastException(invalidFormatPipeMessage);
            }

            feed.Name = nameUrl[NAME_INDEX];

            Uri url;
            if (!Uri.TryCreate(nameUrl[URL_INDEX], UriKind.Absolute, out url))
            {
                throw new InvalidCastException("Invalid URL");
            }
            feed.Url = url;
        }

        /// <summary>
        /// Serializes the current object instance into an object array
        /// compatible with the UTorrent API.
        /// </summary>
        /// <returns></returns>
        public object[] ToArray()
        {
            return new object[] { Id, null, null, null, null, null, Url.AbsoluteUri };
        }
    }
}