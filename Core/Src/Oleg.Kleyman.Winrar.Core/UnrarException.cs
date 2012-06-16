using System;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarException : ApplicationException
    {
        public UnrarException(string message, Exception innerException, RarStatus status)
            : base(message, innerException)
        {
            Status = status;
        }

        public UnrarException(string message, RarStatus status) : this(message, null, status) { }

        public RarStatus Status { get; private set; }
    }
}