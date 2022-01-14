using System;
using System.Collections.Generic;
using Appalachia.Core.Layers;
using UnityEngine;

namespace Appalachia.Core.Filtering
{
    public interface IComponentFilterTask<T>
        where T : Component
    {
        IComponentFilterTask<T> ActiveOnly();
        IComponentFilterTask<T> ExcludeIf(Predicate<T> predicate);
        IComponentFilterTask<T> ExcludeLayers(params LayerInfo[] layers);
        IComponentFilterTask<T> ExcludeTags(params string[] tags);
        IComponentFilterTask<T> InactiveOnly();
        IComponentFilterTask<T> IncludeOnlyIf(Predicate<T> predicate);
        IComponentFilterTask<T> IncludeOnlyLayers(params LayerInfo[] layers);
        IComponentFilterTask<T> IncludeOnlyTags(params string[] tags);
        IComponentFilterLimitedTask<T> LimitResults(int count);
        T[] RunFilter();
        IComponentFilterSortedTask<T> SortBy(IComparer<T> comparer);
        IComponentFilterSortedTask<T> SortBy(Comparison<T> comparison);
    }
}
