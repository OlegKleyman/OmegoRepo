using System;
using System.Runtime.Serialization;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///   Represents an unrar error.
    /// </summary>
    [Serializable]
    public class UnrarException : ApplicationException
    {
        const string STATUS_PROPERTY_NAME = "Status";

        protected UnrarException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                Status = (RarStatus) Enum.Parse(typeof(RarStatus), info.GetString(STATUS_PROPERTY_NAME), false);
            }
        }

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

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(STATUS_PROPERTY_NAME, Status.ToString());
        }
    }
}