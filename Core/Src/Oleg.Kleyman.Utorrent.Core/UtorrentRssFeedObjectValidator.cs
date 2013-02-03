using System;
using System.Collections.Generic;
using System.Globalization;

namespace Oleg.Kleyman.Utorrent.Core
{
    /// <summary>
    /// Validates an <see cref="IList{T}"/> of an <see cref="Array"/> of <see cref="object"/>
    /// </summary>
    public class UtorrentRssFeedObjectValidator : Validator
    {
        private IList<object> _targetFeed;
        private const int ID_INDEX = 0;
        private const int NAME_URL_INDEX = 6;

        /// <summary>
        /// Initializes the object with the <see cref="IList{T}"/> of an
        /// <see cref="Array"/> of <see cref="object"/> to use for operations.
        /// </summary>
        /// <param name="targetFeed">
        /// The target <see cref="IList{T}"/> of an <see cref="Array"/> of <see cref="object"/>
        /// to use for operations
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when targetFeed parameter is null.</exception>
        public UtorrentRssFeedObjectValidator(IList<object> targetFeed)
        {
            if (targetFeed == null)
            {
                const string targetFeedParamName = "targetFeed";
                throw new ArgumentNullException(targetFeedParamName);
            }
            TargetFeed = targetFeed;
        }
        /// <summary>
        /// Gets or sets the target 
        /// <see cref="IList{T}"/> of an <see cref="Array"/> of <see cref="object"/> to validate
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when value parameter is null.</exception>
        public IList<object> TargetFeed
        {
            get { return _targetFeed; }
            set
            {
                if (value == null)
                {
                    const string valueParamName = "value";
                    throw new ArgumentNullException(valueParamName);
                }
                _targetFeed = value;
            }
        }

        /// <summary>
        /// Gets the validation message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        ///     Validates a scenario.
        /// </summary>
        /// <returns>True if valid and false if not.</returns>
        public override bool Validate()
        {
            if (TargetFeed.Count < 7)
            {
                const string invalidLengthMessage = "Invalid length";
                Message = invalidLengthMessage;
                return false;
            }

            if (!AreFeedArrayIndexesNull() || !AreElementsOfAValidType())
            {
                return false;
            }

            return true;
        }

        private bool AreFeedArrayIndexesNull()
        {
            var idIndexIsNullMessage = string.Format(CultureInfo.InvariantCulture, "ID index {0} is null", ID_INDEX);
            var nameUrlIndexIsNullMessage = string.Format(CultureInfo.InvariantCulture, "Name/Url index {0} is null", NAME_URL_INDEX);

            SetMessageOnNullIndex(ID_INDEX, idIndexIsNullMessage);
            SetMessageOnNullIndex(NAME_URL_INDEX, nameUrlIndexIsNullMessage);

            return Message == null;
        }

        private void SetMessageOnNullIndex(int targetIndex, string message)
        {
            if (TargetFeed[targetIndex] == null)
            {
                Message = message;
            }
        }

        private bool AreElementsOfAValidType()
        {
            var idIndexIsInvalidTypeMessage = string.Format(CultureInfo.InvariantCulture, "ID index {0} must be an int",
                                                            ID_INDEX);
            var nameUrlIndexIsInvalidTypeMessage = string.Format(CultureInfo.InvariantCulture,
                                                                 "Name/Url index {0} must be a string",
                                                                 NAME_URL_INDEX);
            SetMessageOnInvalidType(ID_INDEX, typeof(int), idIndexIsInvalidTypeMessage);
            SetMessageOnInvalidType(NAME_URL_INDEX, typeof(string), nameUrlIndexIsInvalidTypeMessage);

            return Message == null;
        }

        private void SetMessageOnInvalidType(int targetIndex, Type validType, string message)
        {
            if (TargetFeed[targetIndex].GetType() != validType)
            {
                Message = message;
            }
        }
    }
}