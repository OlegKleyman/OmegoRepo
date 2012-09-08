using System;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///   Represents an unrar error.
    /// </summary>
    public class UnrarException : ApplicationException
    {
        /// <summary>
        ///   Initializes the <see cref="UnrarException" /> object.
        /// </summary>
        /// <param name="message"> The message of the error. </param>
        /// <param name="innerException"> The underlining exception that triggered this error. Null if none. </param>
        /// <param name="status"> The rar status at the time of the error. </param>
        public UnrarException(string message, Exception innerException, RarStatus status)
            : base(message, innerException)
        {
            Status = status;
        }

        /// <summary>
        ///   Initializes the <see cref="UnrarException" /> object.
        /// </summary>
        /// <param name="message"> The message of the error. </param>
        /// <param name="status"> The rar status at the time of the error. </param>
        public UnrarException(string message, RarStatus status) : this(message, null, status)
        {
        }

        /// <summary>
        ///   Gets the rar status at the time of the exception.
        /// </summary>
        public RarStatus Status { get; private set; }
    }
}