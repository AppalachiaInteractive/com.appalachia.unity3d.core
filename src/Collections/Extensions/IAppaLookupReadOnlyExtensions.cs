using System;
using System.Collections.Generic;
using Appalachia.Core.Collections.Interfaces;
using Unity.Profiling;

namespace Appalachia.Core.Collections.Extensions
{
    public static class IAppaLookupReadOnlyExtensions
    {
        public static TValue First<TKey, TValue, TValueList>(
            this IAppaLookupReadOnly<TKey, TValue, TValueList> lookup)
            where TValueList : AppaList<TValue>
        {
            using (_PRF_First.Auto())
            {
                return lookup.at[0];
            }
        }

        public static TValue FirstOrDefault<TKey, TValue, TValueList>(
            this IAppaLookupReadOnly<TKey, TValue, TValueList> lookup)
            where TValueList : AppaList<TValue>
        {
            using (_PRF_FirstOrDefault.Auto())
            {
                if (lookup.Count == 0)
                {
                    return default;
                }

                return lookup.at[0];
            }
        }

        public static IEnumerable<TNew> Select<TKey, TValue, TValueList, TNew>(
            this IAppaLookupReadOnly<TKey, TValue, TValueList> lookup,
            Func<TKey, TValue, TNew> getter)
            where TValueList : AppaList<TValue>
        {
            foreach (var x in lookup.Lookup)
            {
                yield return getter(x.Key, x.Value);
            }
        }

        public static IEnumerable<TNew> SelectMany<TKey, TValue, TValueList, TNew>(
            this IAppaLookupReadOnly<TKey, TValue, TValueList> lookup,
            Func<TKey, TValue, IEnumerable<TNew>> getter)
            where TValueList : AppaList<TValue>
        {
            foreach (var x in lookup.Lookup)
            {
                var many = getter(x.Key, x.Value);

                foreach (var one in many)
                {
                    yield return one;
                }
            }
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> Where<TKey, TValue, TValueList>(
            this IAppaLookupReadOnly<TKey, TValue, TValueList> lookup,
            Predicate<KeyValuePair<TKey, TValue>> predicate)
            where TValueList : AppaList<TValue>
        {
            foreach (var x in lookup.Lookup)
            {
                if (predicate(x))
                {
                    yield return x;
                }
            }
        }


        #region Profiling

        private const string _PRF_PFX = nameof(IAppaLookupReadOnlyExtensions) + ".";
        private static readonly ProfilerMarker _PRF_First = new ProfilerMarker(_PRF_PFX + nameof(First));

        private static readonly ProfilerMarker _PRF_FirstOrDefault =
            new ProfilerMarker(_PRF_PFX + nameof(FirstOrDefault));

        #endregion
    }
}
