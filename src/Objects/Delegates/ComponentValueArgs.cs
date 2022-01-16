using Appalachia.Core.Objects.Delegates.Base;
using UnityEngine;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class ComponentValueArgs<T, TV> : ComponentValueBaseArgs<ComponentValueArgs<T, TV>, T, TV>
        where T : Component
    {
        public delegate void Handler(ComponentValueArgs<T, TV> args);
    }
}
