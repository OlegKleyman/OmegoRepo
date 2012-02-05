using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Oleg.Kleyman.Core.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source,
                                                             Func<TSource, TSource, bool> comparer)
        {
            return source.Distinct(new EqualityComparer<TSource>(comparer));
        }

        public static void ForEach<T>(this IEnumerable<T> target, Action<T> action)
        {
            foreach (T item in target)
            {
                action(item);
            }
        }

        public static void ForEach(this IEnumerable target, Action<object> action)
        {
            foreach (var item in target)
            {
                action(item);
            }
        }
    }
}