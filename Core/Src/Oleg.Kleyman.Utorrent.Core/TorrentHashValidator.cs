using System;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    ///   Represents a torrent hash validator.
    /// </summary>
    public class TorrentHashValidator : Validator
    {
        /// <summary>
        ///   Instantiates a <see cref="TorrentHashValidator" /> object.
        /// </summary>
        /// <param name="hash"> The hash to be used for validation. </param>
        /// <exception cref="ArgumentNullException">Thrown when the hash parameter is null.</exception>
        public TorrentHashValidator(string hash)
        {
            Hash = hash;
            if (hash == null)
            {
                const string hashParamName = "hash";
                throw new ArgumentNullException(hashParamName);
            }
        }

        #region Overrides of Validator

        /// <summary>
        ///   Validates a torrent hash.
        /// </summary>
        /// <returns> True if valid and false if not. </returns>
        public override bool Validate()
        {
            return Hash.Length == 40;
        }

        #endregion

        /// <summary>
        ///   Gets or sets the hash to validate.
        /// </summary>
        public string Hash { get; set; }
    }
}