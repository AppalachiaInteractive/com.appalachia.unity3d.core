using System;
using System.Diagnostics;

namespace Appalachia.Core.Comparisons.ComponentEquality
{
    public abstract class ComponentContentEqualityStateBase<T> : IEquatable<T>
        where T : ComponentContentEqualityStateBase<T>, new()
    {
        [DebuggerStepThrough] public abstract bool Equals(T other);
    }
}
