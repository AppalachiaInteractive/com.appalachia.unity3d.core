using Appalachia.Core.Objects.Delegates.Base;
using UnityEngine;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class
        ComponentValueChangedArgs<T, TV> : ComponentValueChangedBaseArgs<ComponentValueChangedArgs<T, TV>, T,
            TV>
        where T : Component
    {
        public delegate void Handler(ComponentValueChangedArgs<T, TV> args);
    }
}
