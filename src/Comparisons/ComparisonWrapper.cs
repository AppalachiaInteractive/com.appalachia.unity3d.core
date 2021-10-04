using System;
using System.Collections.Generic;

namespace Appalachia.Core.Comparisons
{
    public class ComparisonWrapper<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparisonWrapper(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return _comparison?.Invoke(x, y) ?? 0;
        }
    }
}
