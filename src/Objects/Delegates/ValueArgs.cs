using Appalachia.Core.Objects.Delegates.Base;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class ValueArgs<T> : ValueBaseArgs<ValueArgs<T>, T>
    {
        public delegate void Handler(ValueArgs<T> args);

        public static implicit operator T(ValueArgs<T> o)
        {
            return o.value;
        }
    }
}
