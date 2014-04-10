using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    ///     Represents a torrent.
    /// </summary>
    [DataContract]
    public class Torrent : UTorrentBase, ICloneable
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Torrent()
        {
            
        }
        /// <summary>
        /// Initializes the object with a <see cref="TorrentHash"/> and <see cref="files"/>
        /// </summary>
        /// <param name="hash">The hash of the torrent.</param>
        /// <param name="torrentFiles">File contents of the torrent.</param>
        public Torrent(TorrentHash hash, TorrentFile[] torrentFiles)
        {
            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }

            if (torrentFiles == null)
            {
                throw new ArgumentNullException("torrentFiles");
            }

            Hash = hash;
            TorrentFiles = torrentFiles;
        }

        private object[] _files;
        
        /// <summary>
        ///     Gets or sets the torrent files.
        /// </summary>
        /// <remarks>This property represents different properties of a torrent and not just the files.</remarks>
        [DataMember(Name = "files", Order = 2)]
        public object[] Files
        {
            get { return _files != null ? (object[]) _files.Clone() : null; }
            set
            {
                SetProperties(value);

                _files = value;
            }
        }

        /// <summary>
        /// Gets the torrent <see cref="Hash"/>
        /// </summary>
        public TorrentHash Hash { get; private set; }

        private TorrentFile[] _torrentFiles;

        /// <summary>
        /// Gets an array of <see cref="TorrentFile"/>s.
        /// </summary>
        public TorrentFile[] TorrentFiles
        {
            get { return _torrentFiles ?? (_torrentFiles = new TorrentFile[]{}); }
            private set { _torrentFiles = value; }
        }

        private void SetProperties(IList<object> value)
        {
            if (value.Count == 0)
            {
                const string invalidPropertyArrayMessage = "Invalid property array.";
                throw new InvalidOperationException(invalidPropertyArrayMessage);
            }
            Hash = TorrentHash.Parse(value[0].ToString());

            TorrentFiles = (from object[] file in value.Skip(1)
                            from object[] properties in file
                            select (TorrentFile) properties).ToArray();
        }

        /// <summary>
        /// Casts an object array to a <see cref="Torrent"/> object.
        /// </summary>
        /// <param name="targetTorrent">The target <see cref="object"/> array to cast.</param>
        /// <returns>A <see cref="UtorrentRssFeed"/> object</returns>
        /// <exception cref="InvalidCastException">When the target array format is invalid.</exception>
        public static explicit operator Torrent(object[] targetTorrent)
        {
            const int expectedLength = 29;
            if (targetTorrent.Length != expectedLength)
            {
                throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "Unexpected array length of {0}, {1} expected", targetTorrent.Length, expectedLength));
            }
            var torrent = new Torrent(TorrentHash.Parse(targetTorrent[0].ToString()), new TorrentFile[0]);
            
            return torrent;
        }

        /// <summary>
        /// Serializes the current object instance into an object array
        /// compatible with the UTorrent API.
        /// </summary>
        /// <returns></returns>
        public object[] ToArray()
        {
            var serializedArray = new object[29];
            serializedArray[0] = Hash.Value;
            return serializedArray;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var torrent = new Torrent(TorrentHash.Parse(Hash.Value), new TorrentFile[0]);
            
            return torrent;
        }
    }
}