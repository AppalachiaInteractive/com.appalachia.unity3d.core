using Appalachia.Core.Objects.Delegates.Base;
using UnityEngine;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class ComponentArgs<T> : ComponentBaseArgs<ComponentArgs<T>, T>
        where T : Component
    {
        public delegate void Handler(ComponentArgs<T> args);
    }
}
