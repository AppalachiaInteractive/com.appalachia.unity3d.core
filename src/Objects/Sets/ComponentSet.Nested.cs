using System;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    public abstract partial class ComponentSet<TSet, TSetMetadata>
    {
        #region Nested type: SiblingIndexCalculator

        private class SiblingIndexCalculator
        {
            public static int GetDesiredSiblingIndex(
                ComponentSetSorting order,
                int currentIndex,
                int siblingCount)
            {
                using (_PRF_GetDesiredSiblingIndex.Auto())
                {
                    switch (order)
                    {
                        case ComponentSetSorting.Anywhere:
                            return currentIndex;
                        case ComponentSetSorting.First:
                            return GetIndexForFirst(currentIndex, siblingCount);
                        case ComponentSetSorting.Second:
                            return GetIndexForSecond(currentIndex, siblingCount);
                        case ComponentSetSorting.NotFirst:
                            return GetIndexForNotFirst(currentIndex, siblingCount);
                        case ComponentSetSorting.CloseToStart:
                            return GetIndexForCloseToStart(currentIndex, siblingCount);
                        case ComponentSetSorting.BeforeMiddle:
                            return GetIndexForBeforeMiddle(currentIndex, siblingCount);
                        case ComponentSetSorting.Middle:
                            return GetIndexForMiddle(currentIndex, siblingCount);
                        case ComponentSetSorting.AfterMiddle:
                            return GetIndexForAfterMiddle(currentIndex, siblingCount);
                        case ComponentSetSorting.CloseToLast:
                            return GetIndexForCloseToLast(currentIndex, siblingCount);
                        case ComponentSetSorting.NotLast:
                            return GetIndexForNotLast(currentIndex, siblingCount);
                        case ComponentSetSorting.Last:
                            return GetIndexForLast(currentIndex, siblingCount);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            private static int GetIndexForAfterMiddle(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForAfterMiddle.Auto())
                {
                    if (siblingCount == 1)
                    {
                        return 1;
                    }

                    var half = siblingCount / 2;
                    var afterMiddleIndex = half + 1;
                    var lastIndex = GetIndexForLast(currentIndex, siblingCount);

                    return Mathf.Clamp(currentIndex, afterMiddleIndex, lastIndex);
                }
            }

            private static int GetIndexForBeforeMiddle(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForBeforeMiddle.Auto())
                {
                    if (siblingCount == 1)
                    {
                        return 0;
                    }

                    var half = siblingCount / 2;
                    var beforeMiddleIndex = half - 1;
                    var firstIndex = GetIndexForFirst(currentIndex, siblingCount);

                    return Mathf.Clamp(currentIndex, firstIndex, beforeMiddleIndex);
                }
            }

            private static int GetIndexForCloseToLast(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForCloseToLast.Auto())
                {
                    var afterMiddleIndex = GetIndexForAfterMiddle(currentIndex, siblingCount);
                    var notLastIndex = GetIndexForNotLast(currentIndex, siblingCount);

                    return Mathf.Clamp(currentIndex, afterMiddleIndex, notLastIndex);
                }
            }

            private static int GetIndexForCloseToStart(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForCloseToStart.Auto())
                {
                    var notFirstIndex = GetIndexForNotFirst(currentIndex, siblingCount);
                    var beforeMiddleIndex = GetIndexForBeforeMiddle(currentIndex, siblingCount);

                    return Mathf.Clamp(currentIndex, notFirstIndex, beforeMiddleIndex);
                }
            }

            private static int GetIndexForFirst(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForFirst.Auto())
                {
                    return 0;
                }
            }

            private static int GetIndexForSecond(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForFirst.Auto())
                {
                    return 1;
                }
            }

            private static int GetIndexForLast(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForLast.Auto())
                {
                    return siblingCount;
                }
            }

            private static int GetIndexForMiddle(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForMiddle.Auto())
                {
                    if (siblingCount == 1)
                    {
                        return currentIndex;
                    }

                    var half = siblingCount / 2;

                    var target = half;

                    return Mathf.Clamp(target, 0, siblingCount);
                }
            }

            private static int GetIndexForNotFirst(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForNotFirst.Auto())
                {
                    var firstIndex = GetIndexForFirst(currentIndex, siblingCount);
                    var notFirstIndex = firstIndex + 1;
                    var lastIndex = GetIndexForLast(currentIndex, siblingCount);

                    return Mathf.Clamp(currentIndex, notFirstIndex, lastIndex);
                }
            }

            private static int GetIndexForNotLast(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForNotLast.Auto())
                {
                    var lastIndex = GetIndexForLast(currentIndex, siblingCount);
                    var notLastIndex = lastIndex + 1;
                    var firstIndex = GetIndexForFirst(currentIndex, siblingCount);

                    return Mathf.Clamp(currentIndex, firstIndex, notLastIndex);
                }
            }

            #region Profiling

            private const string _PRF_PFX = nameof(SiblingIndexCalculator) + ".";

            private static readonly ProfilerMarker _PRF_GetDesiredSiblingIndex =
                new ProfilerMarker(_PRF_PFX + nameof(GetDesiredSiblingIndex));

            private static readonly ProfilerMarker _PRF_GetIndexForFirst =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForFirst));

            private static readonly ProfilerMarker _PRF_GetIndexForNotFirst =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForNotFirst));

            private static readonly ProfilerMarker _PRF_GetIndexForCloseToStart =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForCloseToStart));

            private static readonly ProfilerMarker _PRF_GetIndexForBeforeMiddle =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForBeforeMiddle));

            private static readonly ProfilerMarker _PRF_GetIndexForMiddle =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForMiddle));

            private static readonly ProfilerMarker _PRF_GetIndexForAfterMiddle =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForAfterMiddle));

            private static readonly ProfilerMarker _PRF_GetIndexForCloseToLast =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForCloseToLast));

            private static readonly ProfilerMarker _PRF_GetIndexForNotLast =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForNotLast));

            private static readonly ProfilerMarker _PRF_GetIndexForLast =
                new ProfilerMarker(_PRF_PFX + nameof(GetIndexForLast));

            #endregion
        }

        #endregion
    }
}
