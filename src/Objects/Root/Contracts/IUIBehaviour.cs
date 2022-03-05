using UnityEngine;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IUIBehaviour : IBehaviour
    {
        RectTransform RectTransform { get; }
        bool HasRectTransform { get; }
    }
}
