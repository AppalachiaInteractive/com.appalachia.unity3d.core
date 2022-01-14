#region

using System;
using System.Runtime.CompilerServices;
using Appalachia.Utility.Strings;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Math.Stats.Implementations
{
    public class intStatsTracker : StatsTracker<int>
    {
        #region Constants and Static Readonly

        private static readonly Comparison<int> _comparison = (v1, v2) => v1.CompareTo(v2);

        #endregion

        public intStatsTracker(bool trackMedian = false, int limit = 256) : base(trackMedian, limit)
        {
        }

        protected override Comparison<int> Comparer => _comparison;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string Format(int value, FormatType format)
        {
            using (_PRF_Format.Auto())
            {
                switch (format)
                {
                    case FormatType.F0:
                        return value.ToString(_F0);
                    case FormatType.F1:
                        return value.ToString(_F1);
                    case FormatType.F2:
                        return value.ToString(_F2);
                    case FormatType.F3:
                        return value.ToString(_F3);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int Add(int a, int b)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int Divide(int dividend, int divisor)
        {
            return dividend / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string Suffix(int value, SuffixType type)
        {
            using (_PRF_Suffix.Auto())
            {
                switch (type)
                {
                    case SuffixType.None:
                        return null;
                    case SuffixType.FPS:
                        return ZString.Format(suffix_fpx, value * 1000);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int Transform(int value, TransformationType type)
        {
            using (_PRF_Transform.Auto())
            {
                switch (type)
                {
                    case TransformationType.None:
                        return value;
                    case TransformationType.Inverse:
                        return 1 / value;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(intStatsTracker) + ".";

        private static readonly ProfilerMarker _PRF_Format = new(_PRF_PFX + nameof(Format));
        private static readonly ProfilerMarker _PRF_Transform = new(_PRF_PFX + nameof(Transform));
        private static readonly ProfilerMarker _PRF_Suffix = new(_PRF_PFX + nameof(Suffix));

        #endregion
    }
}
