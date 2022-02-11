using Appalachia.Core.Events;
using UnityEngine.EventSystems;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaSelectable<T>
    {
        #region Fields and Autoproperties

        protected ValueEvent<PointerEventData>.Data _Clicked;

        protected ValueEvent<BaseEventData>.Data _Deselected;

        protected ValueEvent<PointerEventData>.Data _DoubleClicked;

        protected ValueEvent<PointerEventData>.Data _Drag;

        protected ValueEvent<PointerEventData>.Data _DragBegin;

        protected ValueEvent<PointerEventData>.Data _DragEnd;

        protected ValueEvent<PointerEventData>.Data _HoverBegin;

        protected ValueEvent<PointerEventData>.Data _HoverEnd;

        protected ValueEvent<PointerEventData>.Data _InitializePotentialDrag;

        protected AppaEvent.Data _Interactable;

        protected AppaEvent.Data _NonInteractable;

        protected ValueEvent<PointerEventData>.Data _Pressed;

        protected ValueEvent<PointerEventData>.Data _Released;

        protected ValueEvent<BaseEventData>.Data _Selected;

        protected ValueEvent<BaseEventData>.Data _Submit;

        #endregion
    }
}
