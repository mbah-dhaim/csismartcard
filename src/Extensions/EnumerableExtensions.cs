using System;
using System.Collections.Generic;
using System.Linq;

namespace com.csi.smartcard.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

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
