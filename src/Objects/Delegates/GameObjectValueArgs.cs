using Appalachia.Core.Objects.Delegates.Base;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class GameObjectValueArgs<T> : GameObjectValueBaseArgs<GameObjectValueArgs<T>, T>
    {
        public delegate void Handler(GameObjectValueArgs<T> args);
    }
}
