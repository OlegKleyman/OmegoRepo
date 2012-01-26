using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oleg.Kleyman.Core.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> comparer)
        {
            return source.Distinct(new GenericComparer<TSource>(comparer));
        }
    }
}
