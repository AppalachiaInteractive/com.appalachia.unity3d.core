using Appalachia.Core.Events.Extensions;
using Appalachia.Core.Objects.Root.Contracts;
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
                                                            ISelectable,
                                                            IPointerClickHandler,
                                                            IDragHandler,
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
        where T : AppalachiaSelectable<T>
    {
        #region Fields and Autoproperties

        private bool _isHovered;

        private bool _isPressed;

        [SerializeField] private bool expectedInteractable = true;

        [SerializeField] private RectTransform _rectTransform;

        private bool _isDragging;

        private bool _isSelected;

        #endregion

        #region Event Functions

        /// <inheritdoc />
        protected override void OnCanvasGroupChanged()
        {
            using (_PRF_OnCanvasGroupChanged.Auto())
            {
                base.OnCanvasGroupChanged();

                UpdateInteractable();
            }
        }

        #endregion

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

        #region ISelectable Members

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

        /// <inheritdoc />
        public override void OnDeselect(BaseEventData eventData)
        {
            using (_PRF_OnDeselect.Auto())
            {
                base.OnDeselect(eventData);

                _isSelected = false;

                _Deselected.RaiseEvent(eventData);
            }
        }

        /// <inheritdoc />
        public override void OnPointerDown(PointerEventData eventData)
        {
            using (_PRF_OnPointerDown.Auto())
            {
                base.OnPointerDown(eventData);

                _isPressed = true;

                _Pressed.RaiseEvent(eventData);
            }
        }

        /// <inheritdoc />
        public override void OnPointerEnter(PointerEventData eventData)
        {
            using (_PRF_OnPointerEnter.Auto())
            {
                base.OnPointerEnter(eventData);

                _isHovered = true;

                _HoverBegin.RaiseEvent(eventData);
            }
        }

        /// <inheritdoc />
        public override void OnPointerExit(PointerEventData eventData)
        {
            using (_PRF_OnPointerExit.Auto())
            {
                base.OnPointerExit(eventData);

                _isHovered = true;

                _HoverEnd.RaiseEvent(eventData);
            }
        }

        /// <inheritdoc />
        public override void OnPointerUp(PointerEventData eventData)
        {
            using (_PRF_OnPointerUp.Auto())
            {
                base.OnPointerUp(eventData);

                _isPressed = false;

                _Released.RaiseEvent(eventData);
            }
        }

        /// <inheritdoc />
        public override void OnSelect(BaseEventData eventData)
        {
            using (_PRF_OnSelect.Auto())
            {
                base.OnSelect(eventData);
                _isSelected = true;

                _Selected.RaiseEvent(eventData);
            }
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            using (_PRF_OnBeginDrag.Auto())
            {
                _isDragging = true;

                _DragBegin.RaiseEvent(eventData);
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            using (_PRF_OnDrag.Auto())
            {
                _isDragging = true;

                _Drag.RaiseEvent(eventData);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            using (_PRF_OnEndDrag.Auto())
            {
                _isDragging = true;

                _DragEnd.RaiseEvent(eventData);
            }
        }

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

        void ISubmitHandler.OnSubmit(BaseEventData eventData)
        {
            using (_PRF_OnSubmit.Auto())
            {
                _Submit.RaiseEvent(eventData);
            }
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            using (_PRF_OnInitializePotentialDrag.Auto())
            {
                _InitializePotentialDrag.RaiseEvent(eventData);
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

        private static readonly ProfilerMarker _PRF_OnInitializePotentialDrag =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitializePotentialDrag));

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
