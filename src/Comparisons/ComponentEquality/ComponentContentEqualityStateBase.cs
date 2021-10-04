using System;

namespace Appalachia.Core.Comparisons.ComponentEquality
{
    public abstract class ComponentContentEqualityStateBase<T> : IEquatable<T>
        where T : ComponentContentEqualityStateBase<T>, new()
    {
        public abstract bool Equals(T other);
    }
}