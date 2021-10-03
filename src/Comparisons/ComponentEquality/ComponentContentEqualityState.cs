using System;

namespace Appalachia.Core.Data.ComponentEquality
{
    public abstract class ComponentContentEqualityState<T, TO> : ComponentContentEqualityStateBase<T>, IEquatable<TO>
        where T : ComponentContentEqualityState<T, TO>, new()
        where TO : UnityEngine.Object
    {
        public abstract void Record(TO c);

        public static T CreateAndRecord(TO o)
        {
            var state = new T();
            state.Record(o);

            return state;
        }

        public abstract bool Equals(TO other);

    }
}