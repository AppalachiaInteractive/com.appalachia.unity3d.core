using Appalachia.Core.Objects.Delegates.Base;

namespace Appalachia.Core.Objects.Delegates
{
    public sealed class GameObjectArgs : GameObjectBaseArgs<GameObjectArgs>
    {
        public delegate void Handler(GameObjectArgs args);
    }
}
