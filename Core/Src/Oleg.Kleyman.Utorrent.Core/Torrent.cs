using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    ///     Represents a torrent.
    /// </summary>
    [DataContract]
    public class Torrent : UTorrentBase
    {
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

        public TorrentHash Hash { get; private set; }

        public TorrentFile[] TorrentFiles { get; private set; }

        private void SetProperties(object[] value)
        {
            if (value.Length == 0)
            {
                const string invalidPropertyArrayMessage = "Invalid property array.";
                throw new InvalidOperationException(invalidPropertyArrayMessage);
            }
            Hash = TorrentHash.Parse(value[0].ToString());

            TorrentFiles = (from object[] file in value.Skip(1)
                            from object[] properties in file
                            select (TorrentFile) properties).ToArray();
        }
    }
}