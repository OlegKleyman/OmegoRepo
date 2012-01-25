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
            var distinctValues = new List<TSource>();
            foreach(var value in source)
            {
                var containsValue = distinctValues.Any(distinctValue => comparer(value, distinctValue));
                if(!containsValue)
                {
                    distinctValues.Add(value);
                }
            }
            return distinctValues;
        }
    }
}
