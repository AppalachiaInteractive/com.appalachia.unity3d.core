#region

using System;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    public abstract class AppaLookup4<TKey1, TKey2, TKey3, TKey4, TValue, TKey1List, TKey2List, TKey3List,
                                      TKey4List, TValueList, TSubSubNested, TSubNested, TNested,
                                      TSubSubNestedList, TSubNestedList, TNestedList> : AppaLookup<TKey1,
        TNested, TKey1List, TNestedList>
        where TKey1List : AppaList<TKey1>, new()
        where TKey2List : AppaList<TKey2>, new()
        where TKey3List : AppaList<TKey3>, new()
        where TKey4List : AppaList<TKey4>, new()
        where TValueList : AppaList<TValue>, new()
        where TSubSubNested : AppaLookup<TKey4, TValue, TKey4List, TValueList>, new()
        where TSubNested : AppaLookup2<TKey3, TKey4, TValue, TKey3List, TKey4List, TValueList, TSubSubNested,
            TSubSubNestedList>, new()
        where TNested : AppaLookup3<TKey2, TKey3, TKey4, TValue, TKey2List, TKey3List, TKey4List, TValueList,
            TSubSubNested, TSubNested, TSubSubNestedList, TSubNestedList>, new()
        where TSubSubNestedList : AppaList<TSubSubNested>, new()
        where TSubNestedList : AppaList<TSubNested>, new()
        where TNestedList : AppaList<TNested>, new()

    {
        public void AddOrUpdate(
            TKey1 primary,
            TKey2 secondary,
            TKey3 tertiary,
            TKey4 quaternary,
            TValue value)
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

                if (!subSub.TryGetValue(tertiary, out var subSubSub))
                {
                    subSubSub = new TSubSubNested();

                    subSub.Add(tertiary, subSubSub);
                }

                subSubSub.AddOrUpdate(quaternary, value);
            }
        }

        public void AddOrUpdate(
            TKey1 primary,
            TKey2 secondary,
            TKey3 tertiary,
            TKey4 quaternary,
            TValue value,
            Func<TNested> collectionCreator1,
            Func<TSubNested> collectionCreator2,
            Func<TSubSubNested> collectionCreator3)
        {
            using (_PRF_AddOrUpdate.Auto())
            {
                if (!TryGetValue(primary, out var sub))
                {
                    sub = collectionCreator1();

                    Add(primary, sub);
                }

                if (!sub.TryGetValue(secondary, out var subSub))
                {
                    subSub = collectionCreator2();

                    sub.Add(secondary, subSub);
                }

                if (!subSub.TryGetValue(tertiary, out var subSubSub))
                {
                    subSubSub = collectionCreator3();

                    subSub.Add(tertiary, subSubSub);
                }

                subSubSub.AddOrUpdate(quaternary, value);
            }
        }

        public bool TryGetValue(
            TKey1 primary,
            TKey2 secondary,
            TKey3 tertiary,
            TKey4 quaternary,
            out TValue value)
        {
            using (_PRF_TryGetValue.Auto())
            {
                if (base.TryGetValue(primary, out var sub1))
                {
                    if (sub1.TryGetValue(secondary, out var sub2))
                    {
                        if (sub2.TryGetValue(tertiary, out var sub3))
                        {
                            if (sub3.TryGet(quaternary, out value))
                            {
                                return true;
                            }
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
            TKey4 quaternary,
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
                        if (sub2.TryGetValue(tertiary, out var sub3))
                        {
                            if (sub3.TryGet(quaternary, out value))
                            {
                                return true;
                            }

                            if (logFallbackAttempt != null)
                            {
                                Context.Log.Warn(logFallbackAttempt);
                            }

                            value = sub3.FirstWithPreference_NoAlloc(fallbackCheck, out var foundFallback);

                            if (foundFallback)
                            {
                                return true;
                            }

                            if (logFallbackFailure != null)
                            {
                                Context.Log.Warn(logFallbackFailure);
                            }
                        }
                    }
                }

                value = default;
                return false;
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(AppaLookup4<TKey1, TKey2, TKey3, TKey4, TValue, TKey1List, TKey2List, TKey3List, TKey4List,
                TValueList, TSubSubNested, TSubNested, TNested, TSubSubNestedList, TSubNestedList,
                TNestedList>) +
            ".";

        private static readonly ProfilerMarker _PRF_AddOrUpdate = new(_PRF_PFX + nameof(AddOrUpdate));

        private static readonly ProfilerMarker _PRF_TryGetValue = new(_PRF_PFX + nameof(TryGetValue));

        private static readonly ProfilerMarker _PRF_TryGetValueWithFallback =
            new(_PRF_PFX + nameof(TryGetValueWithFallback));

        #endregion
    }
}
