using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Oleg.Kleyman.Core.Linq
{
    public static class Enumerable
    {
        /// <summary>
        ///     Returns distinct elements from a sequence by using a specified method to compare values.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of source. </typeparam>
        /// <param name="source"> The sequence to remove duplicate elements from. </param>
        /// <param name="comparer"> A method to compare values. </param>
        /// <returns>
        ///     An <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;" /> that contains distinct elements from the source sequence.
        /// </returns>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source,
                                                             Func<TSource, TSource, bool> comparer)
        {
            return source.Distinct(new EqualityComparer<TSource>(comparer));
        }

        /// <summary>
        ///     Executes an action for every element in the source sequence.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of the source. </typeparam>
        /// <param name="source"> The sequence to execute the action with. </param>
        /// <param name="action"> The action to execute. </param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ForEach((IEnumerable) source, action);
        }

        /// <summary>
        ///     Executes an action for every element in the source sequence.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of the source. </typeparam>
        /// <param name="source"> The sequence to execute the action with. </param>
        /// <param name="action"> The action to execute. </param>
        public static void ForEach<T>(this IEnumerable source, Action<T> action)
        {
            foreach (var item in source)
            {
                action((T) item);
            }
        }
    }
}