using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Linq
{
    public static class Extensions
    {
        /// <summary>Does <paramref name="action"/> for each item</summary>
        public static void ForEach<T>(this T[] source, Action<T> action)
        {
            if (!source.IsNullOrEmpty())
                for (var i = 0; i < source.Length; i++)
                    action(source[i]);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
                foreach (var item in source)
                    action(item);
        }
        
        public static void ForEach<T>(this IList<T> source, Action<T,int> action)
        {
            for (var index = 0; index < source.Count; index++)
            {
                var item = source[index];
                action(item, index);
            }
        }

        public static bool IsNullOrEmpty<T>(this T[] source) => source == null || source.Length == 0;
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || source.Any() == false;

        public static IEnumerable<T> ConcatNullable<T>(this IEnumerable<T> s1, IEnumerable<T> s2)
        {
            if (s1 == null)
                return s2;
            if (s2 == null)
                return s1;
            return s1.Concat(s2);
        }

        public static T GetOrDefault<T>(this IList<T> list, int index)
        {
            return index < list?.Count ? list[index] : default;
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
