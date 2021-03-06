#region

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Appalachia.Core.Objects.Layers;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Filtering
{
    public sealed class ComponentFilterTask<T> : IComponentFilterTask<T>,
                                                 IComponentFilterSortedTask<T>,
                                                 IComponentFilterLimitedTask<T>
        where T : Component
    {
        public ComponentFilterTask(T[] input)
        {
            using (_PRF_ComponentFilterTask_ComponentFilterTask.Auto())
            {
                _input = input;
                _validity = new bool[_input.Length];
            }
        }

        #region Fields and Autoproperties

        private readonly bool[] _validity;
        private readonly HashSet<int> _excludeLayers = new();
        private readonly HashSet<int> _includeOnlyLayers = new();
        private readonly HashSet<string> _excludeTags = new();
        private readonly HashSet<string> _includeOnlyTags = new();

        private readonly List<Predicate<T>> _excludeIfPredicates = new();
        private readonly List<Predicate<T>> _includeOnlyPredicates = new();
        private readonly T[] _input;
        private bool? _active;
        private Comparison<T> _sortComparison;
        private IComparer<T> _sortComparer;
        private int _resultLimit;

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CheckValidity()
        {
            using (_PRF_ComponentFilterTask_CheckValidity.Auto())
            {
                var sum = 0;
                for (var i = 0; i < _validity.Length; i++)
                {
                    sum += _validity[i] ? 1 : 0;
                }

                return sum;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ExecuteFilters()
        {
            using (_PRF_ComponentFilterTask_ExecuteFilters.Auto())
            {
                for (var objectIndex = 0; objectIndex < _input.Length; objectIndex++)
                {
                    var obj = _input[objectIndex];

                    var valid = true;
                    _validity[objectIndex] = true;

                    var tag = obj.tag;
                    var layer = obj.gameObject.layer;

                    using (_PRF_ComponentFilterTask_ExecuteFilters_Activity.Auto())
                    {
                        if (_active.HasValue && obj is Behaviour b)
                        {
                            if (_active.Value && !b.enabled)
                            {
                                valid = false;
                            }
                            else if (!_active.Value && b.enabled)
                            {
                                valid = false;
                            }

                            if (!valid)
                            {
                                _validity[objectIndex] = false;
                                continue;
                            }
                        }
                    }

                    using (_PRF_ComponentFilterTask_ExecuteFilters_IncludeOnlyPredicates.Auto())
                    {
                        if (_includeOnlyPredicates.Count > 0)
                        {
                            for (var predicateIndex = 0;
                                 predicateIndex < _includeOnlyPredicates.Count;
                                 predicateIndex++)
                            {
                                var predicate = _includeOnlyPredicates[predicateIndex];
                                if (!predicate(obj))
                                {
                                    valid = false;
                                    break;
                                }
                            }

                            if (!valid)
                            {
                                _validity[objectIndex] = false;
                                continue;
                            }
                        }
                    }

                    using (_PRF_ComponentFilterTask_ExecuteFilters_ExcludeIfPredicates.Auto())
                    {
                        if (_excludeIfPredicates.Count > 0)
                        {
                            for (var predicateIndex = 0;
                                 predicateIndex < _excludeIfPredicates.Count;
                                 predicateIndex++)
                            {
                                var predicate = _excludeIfPredicates[predicateIndex];
                                if (predicate(obj))
                                {
                                    valid = false;
                                    break;
                                }
                            }

                            if (!valid)
                            {
                                _validity[objectIndex] = false;
                                continue;
                            }
                        }
                    }

                    using (_PRF_ComponentFilterTask_ExecuteFilters_IncludeOnlyTags.Auto())
                    {
                        if (_includeOnlyTags.Count > 0)
                        {
                            if (!_includeOnlyTags.Contains(tag))
                            {
                                _validity[objectIndex] = false;
                                continue;
                            }
                        }
                    }

                    using (_PRF_ComponentFilterTask_ExecuteFilters_ExcludeIfTags.Auto())
                    {
                        if (_excludeTags.Count > 0)
                        {
                            if (_excludeTags.Contains(tag))
                            {
                                _validity[objectIndex] = false;
                                continue;
                            }
                        }
                    }

                    using (_PRF_ComponentFilterTask_ExecuteFilters_IncludeOnlyLayers.Auto())
                    {
                        if (_includeOnlyLayers.Count > 0)
                        {
                            if (!_includeOnlyLayers.Contains(layer))
                            {
                                _validity[objectIndex] = false;
                                continue;
                            }
                        }
                    }

                    using (_PRF_ComponentFilterTask_ExecuteFilters_ExcludeIfLayers.Auto())
                    {
                        if (_excludeLayers.Count > 0)
                        {
                            if (_excludeLayers.Contains(layer))
                            {
                                _validity[objectIndex] = false;
                            }
                        }
                    }
                }
            }
        }

        #region IComponentFilterTask<T> Members

        public IComponentFilterLimitedTask<T> LimitResults(int count)
        {
            _resultLimit = count;

            return this;
        }

        public IComponentFilterSortedTask<T> SortBy(IComparer<T> comparer)
        {
            _sortComparer = comparer;

            return this;
        }

        public IComponentFilterSortedTask<T> SortBy(Comparison<T> comparison)
        {
            _sortComparison = comparison;

            return this;
        }

        public IComponentFilterTask<T> ActiveOnly()
        {
            _active = true;

            return this;
        }

        public IComponentFilterTask<T> ExcludeIf(Predicate<T> predicate)
        {
            _excludeIfPredicates.Add(predicate);

            return this;
        }

        public IComponentFilterTask<T> ExcludeLayers(params LayerInfo[] layers)
        {
            for (var i = 0; i < layers.Length; i++)
            {
                _excludeLayers.Add(layers[i].Id);
            }

            return this;
        }

        public IComponentFilterTask<T> ExcludeTags(params string[] tags)
        {
            _excludeTags.AddRange(tags);

            return this;
        }

        public IComponentFilterTask<T> InactiveOnly()
        {
            _active = false;

            return this;
        }

        public IComponentFilterTask<T> IncludeOnlyIf(Predicate<T> predicate)
        {
            _includeOnlyPredicates.Add(predicate);

            return this;
        }

        public IComponentFilterTask<T> IncludeOnlyLayers(params LayerInfo[] layers)
        {
            for (var i = 0; i < layers.Length; i++)
            {
                _includeOnlyLayers.Add(layers[i].Id);
            }

            return this;
        }

        public IComponentFilterTask<T> IncludeOnlyTags(params string[] tags)
        {
            _includeOnlyTags.AddRange(tags);

            return this;
        }

        public T[] RunFilter()
        {
            using (_PRF_ComponentFilterTask_Complete.Auto())
            {
                ExecuteFilters();

                var sum = CheckValidity();

                var output = new T[sum];

                var offset = 0;
                for (var i = 0; i < _input.Length; i++)
                {
                    if (_validity[i])
                    {
                        output[offset] = _input[i];
                        offset += 1;
                    }
                }

                if (_sortComparer != null)
                {
                    Array.Sort(output, _sortComparer);
                }
                else if (_sortComparison != null)
                {
                    Array.Sort(output, _sortComparison);
                }

                if ((_resultLimit > 0) && (_resultLimit < sum))
                {
                    var limitedOutput = new T[_resultLimit];

                    Array.Copy(output, limitedOutput, _resultLimit);

                    return limitedOutput;
                }

                return output;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_CheckValidity =
            new("ComponentFilterTask.CheckValidity");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_Complete =
            new("ComponentFilterTask.Complete");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ComponentFilterTask =
            new("ComponentFilterTask.ComponentFilterTask");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExcludeTags =
            new("ComponentFilterTask.ExcludeTags");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters =
            new("ComponentFilterTask.ExecuteFilters");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_Activity =
            new("ComponentFilterTask.ExecuteFilters.Activity");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_ExcludeIfLayers =
            new("ComponentFilterTask.ExecuteFilters.ExcludeIfLayers");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_ExcludeIfPredicates =
            new("ComponentFilterTask.ExecuteFilters.ExcludeIfPredicates");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_ExcludeIfTags =
            new("ComponentFilterTask.ExecuteFilters.ExcludeIfTags");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_IncludeOnlyLayers =
            new("ComponentFilterTask.ExecuteFilters.IncludeOnlyLayers");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_IncludeOnlyPredicates =
            new("ComponentFilterTask.ExecuteFilters.IncludeOnlyPredicates");

        private static readonly ProfilerMarker _PRF_ComponentFilterTask_ExecuteFilters_IncludeOnlyTags =
            new("ComponentFilterTask.ExecuteFilters.IncludeOnlyTags");

        #endregion
    }
}
