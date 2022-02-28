using Appalachia.Utility.Events;
using UnityEngine.EventSystems;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaSelectable<T>
    {
        #region Fields and Autoproperties

        protected AppaEvent<PointerEventData>.Data _Clicked;

        protected AppaEvent<BaseEventData>.Data _Deselected;

        protected AppaEvent<PointerEventData>.Data _DoubleClicked;

        protected AppaEvent<PointerEventData>.Data _Drag;

        protected AppaEvent<PointerEventData>.Data _DragBegin;

        protected AppaEvent<PointerEventData>.Data _DragEnd;

        protected AppaEvent<PointerEventData>.Data _HoverBegin;

        protected AppaEvent<PointerEventData>.Data _HoverEnd;

        protected AppaEvent<PointerEventData>.Data _InitializePotentialDrag;

        protected AppaEvent.Data _Interactable;

        protected AppaEvent.Data _NonInteractable;

        protected AppaEvent<PointerEventData>.Data _Pressed;

        protected AppaEvent<PointerEventData>.Data _Released;

        protected AppaEvent<BaseEventData>.Data _Selected;

        protected AppaEvent<BaseEventData>.Data _Submit;

        #endregion
    }
}
