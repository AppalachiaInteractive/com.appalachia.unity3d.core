using System;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Comparisons.ComponentEquality
{
    public abstract class
        ComponentContentEqualityState<T, TO> : ComponentContentEqualityStateBase<T>, IEquatable<TO>
        where T : ComponentContentEqualityState<T, TO>, new()
        where TO : Object
    {
        public abstract bool Equals(TO other);
        public abstract void Record(TO c);

        public static T CreateAndRecord(TO o)
        {
            var state = new T();
            state.Record(o);

            return state;
        }
    }
}
