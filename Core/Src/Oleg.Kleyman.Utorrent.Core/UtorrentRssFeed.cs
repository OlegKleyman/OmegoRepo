using System;
using System.Collections.Generic;
using System.Globalization;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Represents a UTorrent RSS feed object.
    /// </summary>
    public class UtorrentRssFeed : ICloneable
    {
        private const int NAME_INDEX = 0;
        private const int URL_BEGINNING_INDEX = 1;
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
        public static explicit operator UtorrentRssFeed(object[] targetFeed)
        {
            var feed = new UtorrentRssFeed();

            ThrowExceptionIfFeedArrayIsInvalid(targetFeed);
            //The properties of the rss feed are kept in an array and thus the mess.
            feed.Id = (int)targetFeed[ID_INDEX];

            //The name and URL are kept in a single array element delimeted by a "|" character
            var nameUrl = (string)targetFeed[NAME_URL_INDEX];
            SetNameUrl(feed, nameUrl);

            return feed;
        }

        private static void ThrowExceptionIfFeedArrayIsInvalid(IList<object> targetFeed)
        {
            if (targetFeed.Count < 7)
            {
                const string invalidLengthMessage = "Invalid length";
                throw new InvalidCastException(invalidLengthMessage);
            }

            ThrowExceptionIfFeedArrayIndexesAreNull(targetFeed);
        }

        private static void ThrowExceptionIfFeedArrayIndexesAreNull(IList<object> targetFeed)
        {
            if (targetFeed[ID_INDEX] == null)
            {
                var idIndexIsNullMessage = string.Format(CultureInfo.InvariantCulture, "ID index {0} is null", ID_INDEX);
                throw new InvalidCastException(idIndexIsNullMessage);
            }

            if (targetFeed[NAME_URL_INDEX] == null)
            {
                var nameUrlIndexIsNullMessage = string.Format(CultureInfo.InvariantCulture, "Name/Url index {0} is null", NAME_URL_INDEX);
                throw new InvalidCastException(nameUrlIndexIsNullMessage);
            }

            if (!(targetFeed[ID_INDEX] is int))
            {
                var idIndexIsInvalidTypeMessage = string.Format(CultureInfo.InvariantCulture, "ID index {0} must be an int", ID_INDEX);
                throw new InvalidCastException(idIndexIsInvalidTypeMessage);
            }

            if (!(targetFeed[NAME_URL_INDEX] is string))
            {
                var nameUrlIndexIsInvalidTypeMessage = string.Format(CultureInfo.InvariantCulture, 
                                                                     "Name/Url index {0} must be a string", 
                                                                     NAME_URL_INDEX);
                throw new InvalidCastException(nameUrlIndexIsInvalidTypeMessage);
            }
        }

        private static void SetNameUrl(UtorrentRssFeed feed, string nameUrlContent)
        {
            //The name and URL are delimeted by a "|" character
            //The "|" character can be stored in the URL of the RSS feed in UTorrent
            //even though it's not a valid character in URLs. Thus the split string array
            //must be limited to 2 elements.
            const int maximumAmountOfElements = 2;
            var nameUrl = nameUrlContent.Split(new[] { '|' }, maximumAmountOfElements);
            feed.Name = nameUrl[NAME_INDEX];

            feed.Url = new Uri(nameUrl[URL_INDEX]);
        }
    }
}