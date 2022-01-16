using Appalachia.Core.Objects.Delegates.Base;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class
        GameObjectValueChangedArgs<T> : GameObjectValueChangedBaseArgs<GameObjectValueChangedArgs<T>, T>
    {
        public delegate void Handler(GameObjectValueChangedArgs<T> args);
    }
}
