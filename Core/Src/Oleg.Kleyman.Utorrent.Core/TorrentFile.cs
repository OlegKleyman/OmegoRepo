using System;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    ///   Represents a torrent file.
    /// </summary>
    public class TorrentFile
    {
        /// <summary>
        ///   Gets or sets the name of the torrent file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Converts an object array representation of a torrent file to a <see cref="TorrentFile" /> type equivalent.
        /// </summary>
        /// <param name="properties"> The target object array to convert </param>
        /// <returns> A <see cref="TorrentFile" /> object. </returns>
        /// <exception cref="InvalidCastException">Thrown when the properties object array is an invalid representation of a torrent file.</exception>
        public static explicit operator TorrentFile(object[] properties)
        {
            ThrowExceptionIfInvalid(properties);
            var file = new TorrentFile();
            file.Name = properties[0].ToString();
            return file;
        }

        private static void ThrowExceptionIfInvalid(object[] properties)
        {
            ThrowInvalidCastExceptionIfInvalid(properties);
        }

        private static void ThrowInvalidCastExceptionIfInvalid(object[] properties)
        {
            if (properties.Length != 13)
            {
                var arrayWrongLengthMessage = string.Format("Unable to convert a {0} length array.", properties.Length);
                throw new InvalidCastException(arrayWrongLengthMessage);
            }

            if (properties[0] == null)
            {
                const string firstElementNullMessage = "First element containing Name is null.";
                throw new InvalidCastException(firstElementNullMessage);
            }
        }
    }
}