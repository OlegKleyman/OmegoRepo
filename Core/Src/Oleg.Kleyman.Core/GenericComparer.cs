using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oleg.Kleyman.Core
{
    public class GenericComparer<T> : EqualityComparer<T>
    {
        private const string CANNOT_BE_NULL = "Cannot be null";
        private readonly Func<T, T, bool> _compareHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="compareHandler">Handler to handle the comparison of objects.</param>
        public GenericComparer(Func<T, T, bool> compareHandler)
        {
            _compareHandler = compareHandler;
        }

        /// <summary>
        /// Determines if two arguments are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>Boolean that specifies whether the two arguments used are equal.</returns>
        /// var comparer = new GenericComparer&lt;int&gt;((x, y) => x == y);
        /// var valuesAreEqual = comparer.Equals(5, 5);
        /// Debug.WriteLine(valuesAreEqual); //writes true to the debug window
        public override bool Equals(T x, T y)
        {
            ValidateState();
            
            if (x == null)
            {
                const string xParamName = "x";
                throw new ArgumentNullException(xParamName, CANNOT_BE_NULL);
            }

            if (y == null)
            {
                const string yParamName = "y";
                throw new ArgumentNullException(yParamName, CANNOT_BE_NULL);
            }
            
            return _compareHandler(x, y);
        }

        private void ValidateState()
        {
            if(_compareHandler == null)
            {
                const string comparerIsHandlerIsNull = "Comparer is handler is null";
                throw new InvalidCastException(comparerIsHandlerIsNull);
            }
        }

        /// <summary>
        /// Retreives the Hash Code of an object.
        /// </summary>
        /// <param name="target">Target object to retreive the hash code from.</param>
        /// <returns>Integer Hash Code of target object.</returns>
        /// <example>
        /// var comparer = new GenericComparer&lt;int&gt;((x, y) => x == y);
        /// var hashCode = comparer.GetHashCode(5);
        /// Debug.WriteLine(hashCode); //writes 5 to the debug window
        /// </example>
        public override int GetHashCode(T target)
        {
            return target.GetHashCode();
        }
    }
}
