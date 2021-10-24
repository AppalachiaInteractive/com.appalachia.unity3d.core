#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Returns <c>true</c> if the list is either null or empty. Otherwise <c>false</c>.
        /// </summary>
        /// <param name="list">The list.</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            if (list != null)
            {
                return list.Count == 0;
            }

            return true;
        }

        public static Dictionary<TKey, TValue> BuildLookup<TKey, TValue>(
            this IEnumerable<TValue> values,
            Func<TValue, TKey> keySelector)
        {
            var dictionary = new Dictionary<TKey, TValue>();

            foreach (var value in values)
            {
                var key = keySelector(value);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public static Dictionary<TValue, int> BuildReverseIndexLookup<TValue>(this IEnumerable<TValue> values)
        {
            var dictionary = new Dictionary<TValue, int>();

            var count = -1;

            foreach (var value in values)
            {
                count += 1;
                dictionary.Add(value, count);
            }

            return dictionary;
        }

        /*/// <summary>Convert a colletion to a HashSet.</summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new(source);
        }*/

        /// <summary>Convert a colletion to a HashSet.</summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            return new(source, comparer);
        }

        /// <summary>
        ///     Add an item to the end of a collection if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="append">Func to create the item to append.</param>
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, Func<T> append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            if (condition)
            {
                yield return append();
            }
        }

        /// <summary>
        ///     Add an item to the end of a collection if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="append">The item to append.</param>
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, T append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            if (condition)
            {
                yield return append;
            }
        }

        /// <summary>
        ///     Add a collection to the end of another collection if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="append">The collection to append.</param>
        public static IEnumerable<T> AppendIf<T>(
            this IEnumerable<T> source,
            bool condition,
            IEnumerable<T> append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            if (condition)
            {
                foreach (var obj in append)
                {
                    yield return obj;
                }
            }
        }

        /// <summary>
        ///     Add an item to the end of a collection if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="append">Func to create the item to append.</param>
        public static IEnumerable<T> AppendIf<T>(
            this IEnumerable<T> source,
            Func<bool> condition,
            Func<T> append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            if (condition())
            {
                yield return append();
            }
        }

        /// <summary>
        ///     Add an item to the end of a collection if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="append">The item to append.</param>
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, T append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            if (condition())
            {
                yield return append;
            }
        }

        /// <summary>
        ///     Add a collection to the end of another collection if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="append">The collection to append.</param>
        public static IEnumerable<T> AppendIf<T>(
            this IEnumerable<T> source,
            Func<bool> condition,
            IEnumerable<T> append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            if (condition())
            {
                foreach (var obj in append)
                {
                    yield return obj;
                }
            }
        }

        /// <summary>Add an item to the end of a collection.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="append">Func to create the item to append.</param>
        public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, Func<T> append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            yield return append();
        }

        /// <summary>Add an item to the end of a collection.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="append">The item to append.</param>
        public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, T append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            yield return append;
        }

        /// <summary>Add a collection to the end of another collection.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="append">The collection to append.</param>
        public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, IEnumerable<T> append)
        {
            foreach (var obj in source)
            {
                yield return obj;
            }

            foreach (var obj in append)
            {
                yield return obj;
            }
        }

        /// <summary>Convert each item in the collection.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="converter">Func to convert the items.</param>
        public static IEnumerable<T> Convert<T>(this IEnumerable source, Func<object, T> converter)
        {
            foreach (var obj in source)
            {
                yield return converter(obj);
            }
        }

        /// <summary>Calls an action on each item before yielding them.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="action">The action to call for each item.</param>
        public static IEnumerable<T> Examine<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var obj in source)
            {
                action(obj);
                yield return obj;
            }
        }

        /// <summary>
        ///     Returns and casts only the items of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="source">The collection.</param>
        public static IEnumerable<T> FilterCast<T>(this IEnumerable source)
        {
            foreach (var obj in source)
            {
                if (obj is T)
                {
                    yield return (T) obj;
                }
            }
        }

        /// <summary>
        ///     Returns and casts only the items of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="source">The collection.</param>
        public static IEnumerable<T> FilterCast2<T>(this IEnumerable source)
        {
            return source.FilterCast<T>();
        }

        /// <summary>Perform an action on each item.</summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The action to perform.</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var obj in source)
            {
                action(obj);
            }

            return source;
        }

        /// <summary>Perform an action on each item.</summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The action to perform.</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var num = 0;
            foreach (var obj in source)
            {
                action(obj, num++);
            }

            return source;
        }

        /// <summary>Add an item to the beginning of a collection.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="prepend">Func to create the item to prepend.</param>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, Func<T> prepend)
        {
            yield return prepend();
            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>Add an item to the beginning of a collection.</summary>
        /// <param name="source">The collection.</param>
        /// <param name="prepend">The item to prepend.</param>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T prepend)
        {
            yield return prepend;
            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add a collection to the beginning of another collection.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="prepend">The collection to prepend.</param>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, IEnumerable<T> prepend)
        {
            foreach (var obj in prepend)
            {
                yield return obj;
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add an item to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">Func to create the item to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, Func<T> prepend)
        {
            if (condition)
            {
                yield return prepend();
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add an item to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">The item to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, T prepend)
        {
            if (condition)
            {
                yield return prepend;
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add a collection to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">The collection to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(
            this IEnumerable<T> source,
            bool condition,
            IEnumerable<T> prepend)
        {
            if (condition)
            {
                foreach (var obj in prepend)
                {
                    yield return obj;
                }
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add an item to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">Func to create the item to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(
            this IEnumerable<T> source,
            Func<bool> condition,
            Func<T> prepend)
        {
            if (condition())
            {
                yield return prepend();
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add an item to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">The item to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, T prepend)
        {
            if (condition())
            {
                yield return prepend;
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add a collection to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">The collection to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(
            this IEnumerable<T> source,
            Func<bool> condition,
            IEnumerable<T> prepend)
        {
            if (condition())
            {
                foreach (var obj in prepend)
                {
                    yield return obj;
                }
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add an item to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">Func to create the item to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(
            this IEnumerable<T> source,
            Func<IEnumerable<T>, bool> condition,
            Func<T> prepend)
        {
            if (condition(source))
            {
                yield return prepend();
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add an item to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">The item to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(
            this IEnumerable<T> source,
            Func<IEnumerable<T>, bool> condition,
            T prepend)
        {
            if (condition(source))
            {
                yield return prepend;
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        /// <summary>
        ///     Add a collection to the beginning of another collection, if a condition is met.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="prepend">The collection to prepend.</param>
        public static IEnumerable<T> PrependIf<T>(
            this IEnumerable<T> source,
            Func<IEnumerable<T>, bool> condition,
            IEnumerable<T> prepend)
        {
            if (condition(source))
            {
                foreach (var obj in prepend)
                {
                    yield return obj;
                }
            }

            foreach (var obj in source)
            {
                yield return obj;
            }
        }

        public static IEnumerable<TValue> OrderByFrequencyDescending<TList, TValue>(
            this IEnumerable<TList> list,
            Func<TList, TValue> selector)
        {
            var counts = new Dictionary<TValue, int>();

            foreach (var value in list)
            {
                var selection = selector(value);

                if (selection == null)
                {
                    continue;
                }

                if (!counts.ContainsKey(selection))
                {
                    counts.Add(selection, 1);
                }
                else
                {
                    counts[selection] += 1;
                }
            }

            return counts.OrderByDescending(c => c.Value).Select(c => c.Key);
        }

        public static TValue MostFrequent<TList, TValue>(
            this IEnumerable<TList> list,
            Func<TList, TValue> selector)
        {
            var counts = new Dictionary<TValue, int>();

            foreach (var value in list)
            {
                var selection = selector(value);

                if (selection == null)
                {
                    continue;
                }

                if (!counts.ContainsKey(selection))
                {
                    counts.Add(selection, 1);
                }
                else
                {
                    counts[selection] += 1;
                }
            }

            return counts.OrderByDescending(c => c.Value).FirstOrDefault().Key;
        }

        /// <summary>Adds a collection to a hashset.</summary>
        /// <param name="hashSet">The hashset.</param>
        /// <param name="range">The collection.</param>
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
        {
            foreach (var obj in range)
            {
                hashSet.Add(obj);
            }
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the IList&lt;T&gt;.
        /// </summary>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            if (list is List<T>)
            {
                ((List<T>) list).AddRange(collection);
            }
            else
            {
                foreach (var obj in collection)
                {
                    list.Add(obj);
                }
            }
        }

        /// <summary>Adds a collection to a hashset.</summary>
        /// <param name="hashSet">The hashset.</param>
        /// <param name="range">The collection.</param>
        public static void AddRange2<T>(this HashSet<T> hashSet, IEnumerable<T> range)
        {
            foreach (var obj in range)
            {
                hashSet.Add(obj);
            }
        }

        /// <summary>Sets all items in the list to the given value.</summary>
        /// <param name="list">The list.</param>
        /// <param name="item">The value.</param>
        public static void Populate<T>(this IList<T> list, T item)
        {
            var count = list.Count;
            for (var index = 0; index < count; ++index)
            {
                list[index] = item;
            }
        }

        /// <summary>Adds a collection to a hashset.</summary>
        /// <param name="hashSet">The hashset.</param>
        /// <param name="range">The collection.</param>
        public static void RemoveRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
        {
            foreach (var obj in range)
            {
                hashSet.Remove(obj);
            }
        }

        public static void Slice<T>(this IList<T> values, int slices, int sliceCount, Action<T> sliceAction)
        {
            for (var i = 0; i < values.Count; i++)
            {
                if ((i % slices) == sliceCount)
                {
                    sliceAction(values[i]);
                }
            }
        }

        public static void Slice<T>(this T[] values, int slices, int sliceCount, Action<T> sliceAction)
        {
            for (var i = 0; i < values.Length; i++)
            {
                if ((i % slices) == sliceCount)
                {
                    sliceAction(values[i]);
                }
            }
        }

        /// <summary>Sorts an IList</summary>
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
        {
            if (list is List<T>)
            {
                ((List<T>) list).Sort(comparison);
            }
            else
            {
                var objList = new List<T>(list);
                objList.Sort(comparison);
                for (var index = 0; index < list.Count; ++index)
                {
                    list[index] = objList[index];
                }
            }
        }

        /// <summary>Sorts an IList</summary>
        public static void Sort<T>(this IList<T> list)
        {
            if (list is List<T>)
            {
                ((List<T>) list).Sort();
            }
            else
            {
                var objList = new List<T>(list);
                objList.Sort();
                for (var index = 0; index < list.Count; ++index)
                {
                    list[index] = objList[index];
                }
            }
        }
    }
}
