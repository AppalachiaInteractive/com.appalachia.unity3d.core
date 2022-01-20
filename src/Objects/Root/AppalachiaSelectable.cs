using Appalachia.Core.Objects.Delegates;
using Appalachia.Core.Objects.Delegates.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Appalachia.Core.Objects.Root
{
    /// <summary>
    ///     Exposes C# events for UI selectables.
    /// </summary>
    [ExecuteAlways]
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class AppalachiaSelectable<T> : UnityEngine.UI.Selectable,
                                                            IPointerClickHandler,
                                                            IDragHandler,
                                                            IBeginDragHandler,
                                                            IEndDragHandler,
                                                            ISubmitHandler
        where T : AppalachiaSelectable<T>
    {
        protected event ValueArgs<PointerEventData>.Handler _Clicked;
        protected event ValueArgs<BaseEventData>.Handler _Deselected;
        protected event ValueArgs<PointerEventData>.Handler _DoubleClicked;
        protected event ValueArgs<PointerEventData>.Handler _Drag;
        protected event ValueArgs<PointerEventData>.Handler _DragBegin;
        protected event ValueArgs<PointerEventData>.Handler _DragEnd;
        protected event ValueArgs<PointerEventData>.Handler _HoverBegin;
        protected event ValueArgs<PointerEventData>.Handler _HoverEnd;
        protected event EventHandler _Interactable;
        protected event EventHandler _NonInteractable;
        protected event ValueArgs<PointerEventData>.Handler _Pressed;
        protected event ValueArgs<PointerEventData>.Handler _Released;
        protected event ValueArgs<BaseEventData>.Handler _Selected;
        protected event ValueArgs<BaseEventData>.Handler _Submit;

        #region Fields and Autoproperties

        private bool _isHovered;

        private bool _isPressed;

        [SerializeField] private bool expectedInteractable = true;

        [SerializeField] private RectTransform _rectTransform;

        private bool _isDragging;

        private bool _isSelected;

        #endregion

        public bool IsDragging => _isDragging;

        public bool IsHovered => _isHovered;

        public new bool IsInteractable => base.interactable;

        public new bool IsPressed => _isPressed;
        public bool IsSelected => _isSelected;

        public RectTransform rect
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        public new bool interactable
        {
            set
            {
                base.interactable = value;

                UpdateInteractable();
            }

            get => base.interactable;
        }

        #region Event Functions

        protected override void OnCanvasGroupChanged()
        {
            using (_PRF_OnCanvasGroupChanged.Auto())
            {
                base.OnCanvasGroupChanged();

                UpdateInteractable();
            }
        }

        #endregion

        public override void OnDeselect(BaseEventData eventData)
        {
            using (_PRF_OnDeselect.Auto())
            {
                base.OnDeselect(eventData);

                _isSelected = false;

                _Deselected.RaiseEvent(eventData);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            using (_PRF_OnPointerDown.Auto())
            {
                base.OnPointerDown(eventData);

                _isPressed = true;

                _Pressed.RaiseEvent(eventData);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            using (_PRF_OnPointerEnter.Auto())
            {
                base.OnPointerEnter(eventData);

                _isHovered = true;

                _HoverBegin.RaiseEvent(eventData);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            using (_PRF_OnPointerExit.Auto())
            {
                base.OnPointerExit(eventData);

                _isHovered = true;

                _HoverEnd.RaiseEvent(eventData);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            using (_PRF_OnPointerUp.Auto())
            {
                base.OnPointerUp(eventData);

                _isPressed = false;

                _Released.RaiseEvent(eventData);
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            using (_PRF_OnSelect.Auto())
            {
                base.OnSelect(eventData);
                _isSelected = true;

                _Selected.RaiseEvent(eventData);
            }
        }

        private void UpdateInteractable()
        {
            using (_PRF_UpdateInteractable.Auto())
            {
                var currentInteractable = IsInteractable();

                if (currentInteractable != expectedInteractable)
                {
                    expectedInteractable = currentInteractable;

                    if (expectedInteractable)
                    {
                        _Interactable.RaiseEvent();
                    }
                    else
                    {
                        _NonInteractable.RaiseEvent();
                    }
                }
            }
        }

        #region IBeginDragHandler Members

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            using (_PRF_OnBeginDrag.Auto())
            {
                _isDragging = true;

                _DragBegin.RaiseEvent(eventData);
            }
        }

        #endregion

        #region IDragHandler Members

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            using (_PRF_OnDrag.Auto())
            {
                _isDragging = true;

                _Drag.RaiseEvent(eventData);
            }
        }

        #endregion

        #region IEndDragHandler Members

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            using (_PRF_OnEndDrag.Auto())
            {
                _isDragging = true;

                _DragEnd.RaiseEvent(eventData);
            }
        }

        #endregion

        #region IPointerClickHandler Members

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            using (_PRF_OnPointerClick.Auto())
            {
                if (eventData.clickCount == 2)
                {
                    _DoubleClicked.RaiseEvent(eventData);
                }
                else
                {
                    _Clicked.RaiseEvent(eventData);
                }
            }
        }

        #endregion

        #region ISubmitHandler Members

        void ISubmitHandler.OnSubmit(BaseEventData eventData)
        {
            using (_PRF_OnSubmit.Auto())
            {
                _Submit.RaiseEvent(eventData);
            }
        }

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker _PRF_OnBeginDrag =
            new ProfilerMarker(_PRF_PFX + "OnBeginDrag");

        protected static readonly ProfilerMarker _PRF_OnCanvasGroupChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCanvasGroupChanged));

        protected static readonly ProfilerMarker _PRF_OnDeselect =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeselect));

        protected static readonly ProfilerMarker _PRF_OnDrag = new ProfilerMarker(_PRF_PFX + "OnDrag");

        protected static readonly ProfilerMarker _PRF_OnEndDrag = new ProfilerMarker(_PRF_PFX + "OnEndDrag");

        protected static readonly ProfilerMarker _PRF_OnPointerClick =
            new ProfilerMarker(_PRF_PFX + "OnPointerClick");

        protected static readonly ProfilerMarker _PRF_OnPointerDown =
            new ProfilerMarker(_PRF_PFX + nameof(OnPointerDown));

        protected static readonly ProfilerMarker _PRF_OnPointerEnter =
            new ProfilerMarker(_PRF_PFX + nameof(OnPointerEnter));

        protected static readonly ProfilerMarker _PRF_OnPointerExit =
            new ProfilerMarker(_PRF_PFX + nameof(OnPointerExit));

        protected static readonly ProfilerMarker _PRF_OnPointerUp =
            new ProfilerMarker(_PRF_PFX + nameof(OnPointerUp));

        protected static readonly ProfilerMarker _PRF_OnSelect =
            new ProfilerMarker(_PRF_PFX + nameof(OnSelect));

        private static readonly ProfilerMarker _PRF_OnSubmit = new ProfilerMarker(_PRF_PFX + "OnSubmit");

        protected static readonly ProfilerMarker _PRF_UpdateInteractable =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateInteractable));

        #endregion
    }
}
