#region

using System;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    public abstract class AppaLookup3<TKey1, TKey2, TKey3, TValue, TKey1List, TKey2List, TKey3List,
                                      TValueList, TSubNested, TNested, TSubNestedList, TNestedList> :
        AppaLookup<TKey1, TNested, TKey1List, TNestedList>
        where TKey1List : AppaList<TKey1>, new()
        where TKey2List : AppaList<TKey2>, new()
        where TKey3List : AppaList<TKey3>, new()
        where TValueList : AppaList<TValue>, new()
        where TSubNested : AppaLookup<TKey3, TValue, TKey3List, TValueList>, new()
        where TNested : AppaLookup2<TKey2, TKey3, TValue, TKey2List, TKey3List, TValueList, TSubNested,
            TSubNestedList>, new()
        where TSubNestedList : AppaList<TSubNested>, new()
        where TNestedList : AppaList<TNested>, new()
    {
        private const string _PRF_PFX =
            nameof(AppaLookup3<TKey1, TKey2, TKey3, TValue, TKey1List, TKey2List, TKey3List, TValueList,
                TSubNested, TNested, TSubNestedList, TNestedList>) +
            ".";

        private static readonly ProfilerMarker _PRF_AddOrUpdate = new(_PRF_PFX + nameof(AddOrUpdate));

        private static readonly ProfilerMarker _PRF_TryGetValue = new(_PRF_PFX + nameof(TryGetValue));

        private static readonly ProfilerMarker _PRF_TryGetValueWithFallback =
            new(_PRF_PFX + nameof(TryGetValueWithFallback));

        public bool TryGetValue(TKey1 primary, TKey2 secondary, TKey3 tertiary, out TValue value)
        {
            using (_PRF_TryGetValue.Auto())
            {
                if (base.TryGetValue(primary, out var sub1))
                {
                    if (sub1.TryGetValue(secondary, out var sub2))
                    {
                        if (sub2.TryGetValue(tertiary, out value))
                        {
                            return true;
                        }
                    }
                }

                value = default;
                return false;
            }
        }

        public bool TryGetValueWithFallback(
            TKey1 primary,
            TKey2 secondary,
            TKey3 tertiary,
            out TValue value,
            Predicate<TValue> fallbackCheck,
            string logFallbackAttempt = null,
            string logFallbackFailure = null)
        {
            using (_PRF_TryGetValueWithFallback.Auto())
            {
                if (base.TryGetValue(primary, out var sub1))
                {
                    if (sub1.TryGetValue(secondary, out var sub2))
                    {
                        if (sub2.TryGet(tertiary, out value))
                        {
                            return true;
                        }

                        if (logFallbackAttempt != null)
                        {
                            Debug.LogWarning(logFallbackAttempt);
                        }

                        value = sub2.FirstWithPreference_NoAlloc(fallbackCheck, out var foundFallback);

                        if (foundFallback)
                        {
                            return true;
                        }

                        if (logFallbackFailure != null)
                        {
                            Debug.LogWarning(logFallbackFailure);
                        }
                    }
                }

                value = default;
                return false;
            }
        }

        public void AddOrUpdate(TKey1 primary, TKey2 secondary, TKey3 tertiary, TValue value)
        {
            using (_PRF_AddOrUpdate.Auto())
            {
                if (!TryGetValue(primary, out var sub))
                {
                    sub = new TNested();

                    Add(primary, sub);
                }

                if (!sub.TryGetValue(secondary, out var subSub))
                {
                    subSub = new TSubNested();

                    sub.Add(secondary, subSub);
                }

                subSub.AddOrUpdate(tertiary, value);
            }
        }
    }
}
