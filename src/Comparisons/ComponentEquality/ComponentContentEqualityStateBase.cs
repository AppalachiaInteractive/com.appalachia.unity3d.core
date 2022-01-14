using System;
using System.Diagnostics;

namespace Appalachia.Core.Comparisons.ComponentEquality
{
    public abstract class ComponentContentEqualityStateBase<T> : IEquatable<T>
        where T : ComponentContentEqualityStateBase<T>, new()
    {
        #region IEquatable<T> Members

        [DebuggerStepThrough]
        public abstract bool Equals(T other);

        #endregion
    }
}
