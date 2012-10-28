using System;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    ///   Represents a torrent hash
    /// </summary>
    public class TorrentHash
    {
        private TorrentHash(string hash)
        {
            Value = hash;
        }

        /// <summary>
        ///   Gets the hash value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        ///   Converts the string representation of a hash to the <see cref="TorrentHash" /> equivalent.
        /// </summary>
        /// <param name="hash"> The hash to convert. </param>
        /// <returns> A <see cref="TorrentHash" /> object. </returns>
        /// <exception cref="InvalidOperationException">Thrown when an invalid hash string is used.</exception>
        public static TorrentHash Parse(string hash)
        {
            var hashValidator = new TorrentHashValidator(hash);
            TorrentHash torrentHash;
            if (hashValidator.Validate())
            {
                torrentHash = new TorrentHash(hash);
            }
            else
            {
                const string invalidHashMessage = "The torrent hash must be a 40 character string.";
                throw new InvalidOperationException(invalidHashMessage);
            }

            return torrentHash;
        }

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns> A string that represents the current object. </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Value;
        }
    }
}