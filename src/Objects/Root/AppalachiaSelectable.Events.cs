using Appalachia.Utility.Events;
using UnityEngine.EventSystems;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaSelectable<T>
    {
        #region Fields and Autoproperties

        protected AppaEvent.Data _Interactable;
        protected AppaEvent.Data _NonInteractable;

        protected AppaEvent<BaseEventData>.Data _Deselected;
        protected AppaEvent<BaseEventData>.Data _Selected;

        protected AppaEvent<BaseEventData>.Data _Submit;

        protected AppaEvent<PointerEventData>.Data _AlternatePress;
        protected AppaEvent<PointerEventData>.Data _AlternatePressBegin;
        protected AppaEvent<PointerEventData>.Data _AlternatePressDragBegin;
        protected AppaEvent<PointerEventData>.Data _AlternatePressDragContinue;
        protected AppaEvent<PointerEventData>.Data _AlternatePressDragEnd;
        protected AppaEvent<PointerEventData>.Data _AlternatePressEnd;
        protected AppaEvent<PointerEventData>.Data _DoubleAlternatePress;

        protected AppaEvent<PointerEventData>.Data _ContextMenu;
        protected AppaEvent<PointerEventData>.Data _ContextMenuBegin;
        protected AppaEvent<PointerEventData>.Data _ContextMenuDragBegin;
        protected AppaEvent<PointerEventData>.Data _ContextMenuDragContinue;
        protected AppaEvent<PointerEventData>.Data _ContextMenuDragEnd;
        protected AppaEvent<PointerEventData>.Data _ContextMenuEnd;
        protected AppaEvent<PointerEventData>.Data _DoubleContextMenu;

        protected AppaEvent<PointerEventData>.Data _HoverBegin;
        protected AppaEvent<PointerEventData>.Data _HoverContinue;
        protected AppaEvent<PointerEventData>.Data _HoverEnd;

        protected AppaEvent<PointerEventData>.Data _InitializePotentialDrag;

        protected AppaEvent<PointerEventData>.Data _DoublePress;
        protected AppaEvent<PointerEventData>.Data _Press;
        protected AppaEvent<PointerEventData>.Data _PressBegin;
        protected AppaEvent<PointerEventData>.Data _PressDragBegin;
        protected AppaEvent<PointerEventData>.Data _PressDragContinue;
        protected AppaEvent<PointerEventData>.Data _PressDragEnd;
        protected AppaEvent<PointerEventData>.Data _PressEnd;

        #endregion
    }
}
