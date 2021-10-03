using System;

namespace Appalachia.Core.Data.ComponentEquality
{
    public abstract class ComponentContentEqualityStateBase<T> : IEquatable<T>
        where T : ComponentContentEqualityStateBase<T>, new()
    {
        public abstract bool Equals(T other);
    }
}