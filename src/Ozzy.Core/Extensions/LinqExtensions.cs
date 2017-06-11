using System;
using System.Collections.Generic;

namespace Ozzy.Core
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Guard.ArgumentNotNull(source, nameof(source));
            Guard.ArgumentNotNull(keySelector, nameof(keySelector));

            return _(); IEnumerable<TSource> _()
            {
                var seenKeys = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (seenKeys.Add(keySelector(element)))
                        yield return element;
                }
            }
        }

    }
}
