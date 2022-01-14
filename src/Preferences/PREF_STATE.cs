#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Preferences.API;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
#if UNITY_EDITOR
using Appalachia.Core.Preferences.API.Editor;
#endif

#endregion

namespace Appalachia.Core.Preferences
{
    public class PREF_STATE<T> : PREF_STATE_BASE
    {
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
                    API = new
#if UNITY_EDITOR
                        bool_EPAPI
#else
                    bool_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(Bounds))
                {
                    API = new
#if UNITY_EDITOR
                        Bounds_EPAPI
#else
                    Bounds_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(Color))
                {
                    API = new
#if UNITY_EDITOR
                        Color_EPAPI
#else
                    Color_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(Gradient))
                {
                    API = new
#if UNITY_EDITOR
                        Gradient_EPAPI
#else
                    Gradient_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(double))
                {
                    API = new
#if UNITY_EDITOR
                        double_EPAPI
#else
                    double_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(float))
                {
                    API = new
#if UNITY_EDITOR
                        float_EPAPI
#else
                    float_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(float2))
                {
                    API = new
#if UNITY_EDITOR
                        float2_EPAPI
#else
                    float2_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(float3))
                {
                    API = new
#if UNITY_EDITOR
                        float3_EPAPI
#else
                    float3_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(float4))
                {
                    API = new
#if UNITY_EDITOR
                        float4_EPAPI
#else
                    float4_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(int))
                {
                    API = new
#if UNITY_EDITOR
                        int_EPAPI
#else
                    int_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(quaternion))
                {
                    API = new
#if UNITY_EDITOR
                        quaternion_EPAPI
#else
                    quaternion_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT == typeof(string))
                {
                    API = new
#if UNITY_EDITOR
                        string_EPAPI
#else
                    string_PPAPI
#endif
                        () as IPAPI<T>;
                }
                else if (typeT.IsEnum || (typeT == typeof(Enum)))
                {
                    if (typeT.HasAttribute<FlagsAttribute>())
                    {
                        API = new
#if UNITY_EDITOR
                            Flags_EPAPI
#else
                        Flags_PPAPI
#endif
                            <T>();
                    }
                    else
                    {
                        API = new
#if UNITY_EDITOR
                            Enum_EPAPI
#else
                        Enum_PPAPI
#endif
                            <T>();
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

        #region Fields and Autoproperties

        public IPAPI<T> API { get; }

        private bool _sorted;

        #endregion

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

        #region Nested type: PrefComparer

        #region Nested Types

        public class PrefComparer : Comparer<PREF<T>>
        {
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

            #region Profiling

            private const string _PRF_PFX = nameof(PrefComparer) + ".";
            private static readonly ProfilerMarker _PRF_Compare = new(_PRF_PFX + nameof(Compare));

            #endregion
        }

        #endregion

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(PREF_STATE<T>) + ".";
        private static readonly ProfilerMarker _PRF_PREF_STATE = new(_PRF_PFX + nameof(PREF_STATE<T>));
        private static readonly ProfilerMarker _PRF_Add = new(_PRF_PFX + nameof(Add));
        private static readonly ProfilerMarker _PRF_Sort = new(_PRF_PFX + nameof(Sort));
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));

        #endregion
    }
}
