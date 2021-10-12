#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Preferences.API;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences
{
    public class PREF_STATE<T> : PREF_STATE_BASE
    {
        internal readonly List<PREF<T>> _sortedValues = new();
        private readonly Dictionary<string, PREF<T>> _values = new();

        private bool _sorted;

        public PREF_STATE()
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
                API = new Enum_EPAPI<T>();
            }
            else
            {
                throw new NotSupportedException(typeT.Name);
            }
        }

        public IReadOnlyDictionary<string, PREF<T>> Values => _values;

        public IReadOnlyList<PREF<T>> SortedValues => _sortedValues;

        public PrefComparer Comparer { get; } = new();

        public IEditorPreferenceAPI<T> API { get; }

        public void Sort()
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

        public void Add(string key, PREF<T> value)
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

        public override void Awake()
        {
            foreach (var value in _values)
            {
                value.Value.WakeUp();
            }
        }

        public class PrefComparer : Comparer<PREF<T>>
        {
            public override int Compare(PREF<T> x, PREF<T> y)
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
}
