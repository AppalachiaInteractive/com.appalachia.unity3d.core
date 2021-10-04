#region

using System;
using System.Runtime.CompilerServices;
using Appalachia.Core.Collections.NonSerialized;
using Appalachia.Editing.Attributes;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using Random = System.Random;

#endregion

namespace Appalachia.Core.Math.Stats
{
    [Serializable]
    public abstract class StatsTracker
    {
        protected static Random ___random = new Random();
        protected static Unity.Mathematics.Random _random = new Unity.Mathematics.Random();
    }

    [Serializable]
    public abstract class StatsTracker<T> : StatsTracker
    {
        private const string _PRF_PFX = nameof(StatsTracker<T>) + ".";

        private const string AVG = "Average";
        private const string MIN = "Min";
        private const string MAX = "Max";
        private const string MED = "Median";
        private const string FMT = "F2";

        private const string _seperator = " | ";

        private const string _formattedLongest = "{0}{1}{2}{3}{4}{5}{6}";
        private const string _formattedLong = "{0}{1}{2}{3}{4}";

        private const string _formatted = "{0} {1}{2}";

        private static readonly ProfilerMarker _PRF_StatsTracker = new ProfilerMarker(_PRF_PFX + nameof(StatsTracker));

        private static readonly ProfilerMarker _PRF_Median = new ProfilerMarker(_PRF_PFX + nameof(Median));

        private static readonly ProfilerMarker _PRF_Minimum = new ProfilerMarker(_PRF_PFX + nameof(Minimum));

        private static readonly ProfilerMarker _PRF_Maximum = new ProfilerMarker(_PRF_PFX + nameof(Maximum));

        private static readonly ProfilerMarker _PRF_Sum = new ProfilerMarker(_PRF_PFX + nameof(Sum));

        private static readonly ProfilerMarker _PRF_Average = new ProfilerMarker(_PRF_PFX + nameof(Average));

        private static readonly ProfilerMarker _PRF_Calculate = new ProfilerMarker(_PRF_PFX + nameof(Calculate));

        private static readonly ProfilerMarker _PRF_Track = new ProfilerMarker(_PRF_PFX + nameof(Track));

        private static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX + nameof(Reset));

        private static readonly ProfilerMarker _PRF_ToString = new ProfilerMarker(_PRF_PFX + nameof(ToString));

        private static readonly ProfilerMarker _PRF_GetFormattedString = new ProfilerMarker(_PRF_PFX + nameof(GetFormattedString));

        private NonSerializedList<T> _values;

        [SmartLabel, HorizontalGroup("Meta"), SerializeField]
        private int _lastTrackingIndex;

        [SmartLabel, HorizontalGroup("Meta"), SerializeField]
        private int _limit;

        [SmartLabel, HorizontalGroup("Meta"), SerializeField]
        private bool _trackMedian;

        [SmartLabel, HorizontalGroup("Meta"), SerializeField]
        private int _calculatedAt;

        [SmartLabel, HorizontalGroup("Stats"), SerializeField]
        private T _median;

        [SmartLabel, HorizontalGroup("Stats"), SerializeField]
        private T _minimum;

        [SmartLabel, HorizontalGroup("Stats"), SerializeField]
        private T _maximum;

        [SmartLabel, HorizontalGroup("Stats"), SerializeField]
        private T _sum;

        [SmartLabel, HorizontalGroup("Stats"), SerializeField]
        private T _average;

        protected StatsTracker(bool trackMedian = false, int limit = 256)
        {
            using (_PRF_StatsTracker.Auto())
            {
                if (___random == null)
                {
                    ___random = new Random();
                }

                if (_random.state == 0)
                {
                    var seed = (uint) math.abs(___random.Next(1, 1024000));
                    _random.InitState(seed);
                }

                _trackMedian = trackMedian;
                _values = new NonSerializedList<T>(limit, noTracking: true);
                _limit = limit;
            }
        }

        private bool ShouldCalculate => _calculatedAt != _values.Count;

        public int Count => _values.Count;

        public T Median
        {
            get
            {
                using (_PRF_Median.Auto())
                {
                    if (!_trackMedian)
                    {
                        throw new NotSupportedException();
                    }

                    Calculate();
                    return _median;
                }
            }
        }

        public T Minimum
        {
            get
            {
                using (_PRF_Minimum.Auto())
                {
                    Calculate();
                    return _minimum;
                }
            }
        }

        public T Maximum
        {
            get
            {
                using (_PRF_Maximum.Auto())
                {
                    Calculate();
                    return _maximum;
                }
            }
        }

        public T Sum
        {
            get
            {
                using (_PRF_Sum.Auto())
                {
                    Calculate();
                    return _sum;
                }
            }
        }

        public T Mean => Average;

        public T Average
        {
            get
            {
                using (_PRF_Average.Auto())
                {
                    Calculate();
                    return _average;
                }
            }
        }

        protected abstract Comparison<T> Comparer { get; }

        private void Calculate()
        {
            using (_PRF_Calculate.Auto())
            {
                if (_values == null)
                {
                    _values = new NonSerializedList<T>(_limit, noTracking: true);
                }

                if (!ShouldCalculate)
                {
                    return;
                }

                _calculatedAt = _values.Count;

                if (_trackMedian)
                {
                    _values.Sort(Comparer);

                    var midIndex = _values.Count / 2;
                    _median = _values[midIndex];
                }

                _minimum = _values[0];
                _maximum = _values[0];
                _sum = _values[0];

                for (var i = 1; i < _values.Count; i++)
                {
                    var val = _values[i];

                    _sum = Add(val, _sum);
                    _minimum = Comparer(val, _minimum) < 0 ? val : _minimum;
                    _maximum = Comparer(val, _maximum) > 0 ? val : _maximum;
                }

                _average = Divide(_sum, _values.Count);
            }
        }

        protected abstract T Add(T a, T b);
        protected abstract T Divide(T dividend, int divisor);

        public void Track(T value)
        {
            using (_PRF_Track.Auto())
            {
                if (_values == null)
                {
                    _values = new NonSerializedList<T>(_limit, noTracking: true);
                }

                if (_values.Count < _limit)
                {
                    _values.Add(value);
                }
                else
                {
                    _calculatedAt = 0;

                    _values[_lastTrackingIndex] = value;

                    _lastTrackingIndex = _random.NextInt(0, _values.Count);
                }
            }
        }

        public void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                _values.ClearFast();
                _average = default;
                _maximum = default;
                _median = default;
                _minimum = default;
                _sum = default;
                _calculatedAt = 0;
            }
        }

        public override string ToString()
        {
            return ToString(_trackMedian, SuffixType.None, TransformationType.None, _seperator);
        }

        public string ToString(SuffixType suffix)
        {
            return ToString(_trackMedian, suffix, TransformationType.None, _seperator);
        }

        public string ToString(TransformationType transformation)
        {
            return ToString(_trackMedian, SuffixType.None, transformation, _seperator);
        }

        public string ToString(SuffixType suffix, TransformationType transformation)
        {
            return ToString(_trackMedian, suffix, transformation, _seperator);
        }

        public string ToString(
            bool includeMedian,
            SuffixType suffix,
            TransformationType transformation,
            string seperator,
            string labelAverage = AVG,
            string labelMin = MIN,
            string labelMax = MAX,
            string labelMed = MED,
            FormatType format = FormatType.F2)
        {
            using (_PRF_ToString.Auto())
            {
                var avg = GetFormattedString(Average, labelAverage, suffix, transformation, format);
                var min = GetFormattedString(Minimum, labelMin,     suffix, transformation, format);
                var max = GetFormattedString(Maximum, labelMax,     suffix, transformation, format);
                var s = seperator;

                if (includeMedian)
                {
                    var med = GetFormattedString(Median, labelMed, suffix, transformation, format);
                    return string.Format(_formattedLongest, avg, s, min, s, max, s, med);
                }

                return string.Format(_formattedLong, avg, s, min, s, max);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetFormattedString(T value, string prefix, SuffixType suffix, TransformationType transformation, FormatType format)
        {
            using (_PRF_GetFormattedString.Auto())
            {
                var sfx = Suffix(value, suffix);
                var v = Transform(value, transformation);

                var vs = Format(v, format);
                return string.Format(_formatted, prefix, vs, sfx);
            }
        }

        
        protected const string _F0 = "F0";
        protected const string _F1 = "F1";
        protected const string _F2 = "F2";
        protected const string _F3 = "F3";

        protected const string suffix_fpx = "fps ({0:F1}ms)";
        
        public abstract string Format(T value, FormatType format);

        protected abstract T Transform(T value, TransformationType type);
        protected abstract string Suffix(T value, SuffixType type);
    }
}
