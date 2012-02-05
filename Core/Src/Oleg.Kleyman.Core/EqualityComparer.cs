using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents an equality comparer to compare if two objects are equal.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public class EqualityComparer<T> : System.Collections.Generic.EqualityComparer<T>
    {
        private const string CANNOT_BE_NULL = "Cannot be null";

        /// <summary>
        /// Gets or sets the comparer handler to use for compare operations.
        /// </summary>
        public Func<T, T, bool> CompareHandler { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="compareHandler">Handler to handle the comparison of objects.</param>
        public EqualityComparer(Func<T, T, bool> compareHandler)
        {
            CompareHandler = compareHandler;
        }

        /// <summary>
        /// Determines if two arguments are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>Boolean that specifies whether the two arguments used are equal.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when either of the arguments is null.</exception>
        /// <example>
        /// var comparer = new EqualityComparer&lt;int&gt;((x, y) => x == y);
        /// var valuesAreEqual = comparer.Equals(5, 5);
        /// Debug.WriteLine(valuesAreEqual); //writes true to the debug window
        /// </example>
        public override bool Equals(T x, T y)
        {
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
            
            ValidateState();

            return CompareHandler(x, y);
        }

        private void ValidateState()
        {
            if (CompareHandler == null)
            {
                const string comparerName = "ComparerHandler ";
                throw new InvalidOperationException(comparerName + CANNOT_BE_NULL);
            }
        }

        /// <summary>
        /// Retreives the Hash Code of an object.
        /// </summary>
        /// <param name="target">Target object to retreive the hash code from.</param>
        /// <returns>Integer Hash Code of target object.</returns>
        /// <example>
        /// var comparer = new EqualityComparer&lt;int&gt;((x, y) => x == y);
        /// var hashCode = comparer.GetHashCode(5);
        /// Debug.WriteLine(hashCode); //writes 5 to the debug window
        /// </example>
        public override int GetHashCode(T target)
        {
            return target.GetHashCode();
        }
    }
}
