using System;
using Appalachia.Core.ControlModel.Controls.Model;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Controls
{
    public abstract partial class AppaControl<TControl, TConfig>
    {
        #region Nested type: SiblingIndexCalculator

        private class SiblingIndexCalculator
        {
            public static int GetDesiredSiblingIndex(ControlSorting order, int currentIndex, int siblingCount)
            {
                using (_PRF_GetDesiredSiblingIndex.Auto())
                {
                    switch (order)
                    {
                        case ControlSorting.Anywhere:
                            return currentIndex;
                        case ControlSorting.First:
                            return GetIndexForFirst(currentIndex, siblingCount);
                        case ControlSorting.Second:
                            return GetIndexForSecond(currentIndex, siblingCount);
                        case ControlSorting.NotFirst:
                            return GetIndexForNotFirst(currentIndex, siblingCount);
                        case ControlSorting.CloseToStart:
                            return GetIndexForCloseToStart(currentIndex, siblingCount);
                        case ControlSorting.BeforeMiddle:
                            return GetIndexForBeforeMiddle(currentIndex, siblingCount);
                        case ControlSorting.Middle:
                            return GetIndexForMiddle(currentIndex, siblingCount);
                        case ControlSorting.AfterMiddle:
                            return GetIndexForAfterMiddle(currentIndex, siblingCount);
                        case ControlSorting.CloseToLast:
                            return GetIndexForCloseToLast(currentIndex, siblingCount);
                        case ControlSorting.NotLast:
                            return GetIndexForNotLast(currentIndex, siblingCount);
                        case ControlSorting.Last:
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

            private static int GetIndexForSecond(int currentIndex, int siblingCount)
            {
                using (_PRF_GetIndexForFirst.Auto())
                {
                    return 1;
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
