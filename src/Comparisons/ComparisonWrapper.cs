using System;
using System.Collections.Generic;

namespace Appalachia.Core.Comparisons
{
    public class ComparisonWrapper<T> : IComparer<T>
    {
        public ComparisonWrapper(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        #region Fields and Autoproperties

        private readonly Comparison<T> _comparison;

        #endregion

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return _comparison?.Invoke(x, y) ?? 0;
        }

        #endregion
    }
}
