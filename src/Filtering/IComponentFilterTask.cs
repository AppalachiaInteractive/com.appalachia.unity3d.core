using System;
using System.Collections.Generic;
using Appalachia.Core.Layers;
using UnityEngine;

namespace Appalachia.Core.Filtering
{
    public interface IComponentFilterTask<T>
        where T : Component
    {
        T[] RunFilter();
        IComponentFilterTask<T> ActiveOnly();
        IComponentFilterTask<T> InactiveOnly();
        IComponentFilterTask<T> IncludeOnlyIf(Predicate<T> predicate);
        IComponentFilterTask<T> ExcludeIf(Predicate<T> predicate);
        IComponentFilterTask<T> IncludeOnlyTags(params string[] tags);
        IComponentFilterTask<T> ExcludeTags(params string[] tags);
        IComponentFilterTask<T> IncludeOnlyLayers(params LayerInfo[] layers);
        IComponentFilterTask<T> ExcludeLayers(params LayerInfo[] layers);
        IComponentFilterSortedTask<T> SortBy(IComparer<T> comparer);
        IComponentFilterSortedTask<T> SortBy(Comparison<T> comparison);
        IComponentFilterLimitedTask<T> LimitResults(int count);
    }
}
