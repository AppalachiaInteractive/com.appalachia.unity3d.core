using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface ISelectable : IPointerClickHandler,
                                   IDragHandler,
                                   IInitializePotentialDragHandler,
                                   IBeginDragHandler,
                                   IEndDragHandler,
                                   ISubmitHandler,
                                   IMoveHandler,
                                   IPointerDownHandler,
                                   IPointerUpHandler,
                                   IPointerEnterHandler,
                                   IPointerExitHandler,
                                   ISelectHandler,
                                   IDeselectHandler
    {
        public bool interactable { get; }
        public bool IsDragging { get; }

        public bool IsHovered { get; }

        public bool IsInteractable { get; }

        public bool IsPressed { get; }
        public bool IsSelected { get; }

        public RectTransform rect { get; }

        public AnimationTriggers animationTriggers { get; set; }

        public ColorBlock colors { get; set; }

        public Graphic targetGraphic { get; set; }

        public Navigation navigation { get; set; }

        public SpriteState spriteState { get; set; }

        public Selectable.Transition transition { get; set; }

        public Selectable FindSelectable(Vector3 dir);

        Selectable FindSelectableOnDown();

        Selectable FindSelectableOnLeft();

        Selectable FindSelectableOnRight();

        Selectable FindSelectableOnUp();

        void Select();
    }
}
