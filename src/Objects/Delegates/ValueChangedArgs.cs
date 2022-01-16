using Appalachia.Core.Objects.Delegates.Base;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class ValueChangedArgs<T> : ValueChangedBaseArgs<ValueChangedArgs<T>, T>
    {
        public delegate void Handler(ValueChangedArgs<T> args);
    }
}
