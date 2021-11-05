#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Preferences.API;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences
{
    public class PREF_STATE<T> : PREF_STATE_BASE
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(PREF_STATE<T>) + ".";
        private static readonly ProfilerMarker _PRF_PREF_STATE = new(_PRF_PFX + nameof(PREF_STATE<T>));
        private static readonly ProfilerMarker _PRF_Add = new(_PRF_PFX + nameof(Add));
        private static readonly ProfilerMarker _PRF_Sort = new(_PRF_PFX + nameof(Sort));
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));

        #endregion

        public PREF_STATE()
        {
            using (_PRF_PREF_STATE.Auto())
            {
                var typeT = typeof(T);

                if (typeT == null)
                {
                    throw new TypeAccessException();
                }

                if (typeT == typeof(bool))
                {
                    API = new bool_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(Bounds))
                {
                    API = new Bounds_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(Color))
                {
                    API = new Color_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(Gradient))
                {
                    API = new Gradient_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(double))
                {
                    API = new double_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(float))
                {
                    API = new float_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(float2))
                {
                    API = new float2_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(float3))
                {
                    API = new float3_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(float4))
                {
                    API = new float4_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(int))
                {
                    API = new int_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(quaternion))
                {
                    API = new quaternion_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT == typeof(string))
                {
                    API = new string_EPAPI() as IEditorPreferenceAPI<T>;
                }
                else if (typeT.IsEnum || (typeT == typeof(Enum)))
                {
                    if (typeT.HasAttribute<FlagsAttribute>())
                    {
                        API = new Flags_EPAPI<T>();   
                    }
                    else
                    {
                        API = new Enum_EPAPI<T>();                        
                    }
                }
                else
                {
                    throw new NotSupportedException(typeT.Name);
                }
            }
        }

        #region Preferences

        internal readonly List<PREF<T>> _sortedValues = new();
        private readonly Dictionary<string, PREF<T>> _values = new();

        public PrefComparer Comparer { get; } = new();

        public IReadOnlyList<PREF<T>> SortedValues => _sortedValues;

        public IReadOnlyDictionary<string, PREF<T>> Values => _values;

        #endregion

        private bool _sorted;

        public IEditorPreferenceAPI<T> API { get; }

        public override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                foreach (var value in _values)
                {
                    value.Value.WakeUp();
                }
            }
        }

        public void Add(string key, PREF<T> value)
        {
            using (_PRF_Add.Auto())
            {
                if (allPreferences == null)
                {
                    allPreferences = new List<PREF_BASE>();
                }

                if (_values.ContainsKey(key))
                {
                    _values[key] = value;
                }
                else
                {
                    _values.Add(key, value);
                    allPreferences.Add(value);
                }

                _sorted = false;
            }
        }

        public void Sort()
        {
            using (_PRF_Sort.Auto())
            {
                if (_values.Count == 0)
                {
                    return;
                }

                if (_sorted)
                {
                    return;
                }

                _sorted = true;
                _sortedValues.Clear();

                foreach (var value in _values)
                {
                    _sortedValues.Add(value.Value);
                }

                _sortedValues.Sort(Comparer);
            }
        }

        #region Nested Types

        public class PrefComparer : Comparer<PREF<T>>
        {
            #region Profiling And Tracing Markers

            private const string _PRF_PFX = nameof(PrefComparer) + ".";
            private static readonly ProfilerMarker _PRF_Compare = new(_PRF_PFX + nameof(Compare));

            #endregion

            public override int Compare(PREF<T> x, PREF<T> y)
            {
                using (_PRF_Compare.Auto())
                {
                    if ((x == null) && (y == null))
                    {
                        return 0;
                    }

                    if ((x != null) && (y == null))
                    {
                        return -1;
                    }

                    if ((x == null) && (y != null))
                    {
                        return 1;
                    }

                    var order = x.Order.CompareTo(y.Order);

                    return order != 0 ? order : string.Compare(x.Key, y.Key, StringComparison.Ordinal);
                }
            }
        }

        #endregion
    }
}
