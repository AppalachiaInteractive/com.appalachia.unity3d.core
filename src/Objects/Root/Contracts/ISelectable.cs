using Appalachia.Core.Objects.Models;
using Appalachia.Core.Objects.Models.Selectable;
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
        bool interactable { get; }
        bool IsDragging { get; }

        bool IsHovered { get; }

        bool IsInteractable { get; }

        bool IsPressed { get; }
        bool IsSelected { get; }
        
        SelectableState State { get; }

        RectTransform rectTransform { get; }

        AnimationTriggers animationTriggers { get; set; }

        ColorBlock colors { get; set; }

        Graphic targetGraphic { get; set; }

        Navigation navigation { get; set; }

        SpriteState spriteState { get; set; }

        Selectable.Transition transition { get; set; }

        Selectable FindSelectable(Vector3 dir);

        Selectable FindSelectableOnDown();

        Selectable FindSelectableOnLeft();

        Selectable FindSelectableOnRight();

        Selectable FindSelectableOnUp();

        void Select();
    }
}
