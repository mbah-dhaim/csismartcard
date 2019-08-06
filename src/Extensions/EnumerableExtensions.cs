using System;
using System.Collections.Generic;
using System.Linq;

namespace com.csi.smartcard.Extensions
{
    /// <summary>
    /// Enumerable Extension
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Iterate elements
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
        /// <summary>
        /// Split elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, int blockSize)
        {
            int returned = 0;

            do
            {
                yield return enumerable.Skip(returned).Take(blockSize);

                returned += blockSize;
            }
            while (returned < enumerable.Count());
        }
        /// <summary>
        /// Take last element(s)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> enumerable, int count)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (count > enumerable.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(count),
                    "You cannot take more elements than the enumerable contains.");
            }

            return enumerable.Skip(enumerable.Count() - count).Take(count);
        }
    }
}
