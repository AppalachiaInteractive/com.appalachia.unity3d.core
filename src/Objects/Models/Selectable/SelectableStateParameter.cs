using System;
using Appalachia.Core.Objects.Extensions;
using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Models.Selectable
{
    [Serializable]
    public abstract class SelectableStateParameter<T, TP> : AppalachiaBase<TP>
        where TP : SelectableStateParameter<T, TP>, new()
    {
        #region Fields and Autoproperties

        [HorizontalGroup("A")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private T _normal;

        [HorizontalGroup("A")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private T _hovering;

        [HorizontalGroup("A")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private T _pressed;

        [HorizontalGroup("B")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private T _dragging;

        [HorizontalGroup("B")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private T _selected;

        [HorizontalGroup("B")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private T _disabled;

        #endregion

        public T Disabled
        {
            get => _disabled;
            set
            {
                if (Equals(_disabled, value))
                {
                    return;
                }

                _disabled = value;
                OnChanged();
            }
        }

        public T Dragging
        {
            get => _dragging;
            set
            {
                if (Equals(_dragging, value))
                {
                    return;
                }

                _dragging = value;
                OnChanged();
            }
        }

        public T Hovering
        {
            get => _hovering;
            set
            {
                if (Equals(_hovering, value))
                {
                    return;
                }

                _hovering = value;
                OnChanged();
            }
        }

        public T Normal
        {
            get => _normal;
            set
            {
                if (Equals(_normal, value))
                {
                    return;
                }

                _normal = value;
                OnChanged();
            }
        }

        public T Pressed
        {
            get => _pressed;
            set
            {
                if (Equals(_pressed, value))
                {
                    return;
                }

                _pressed = value;
                OnChanged();
            }
        }

        public T Selected
        {
            get => _selected;
            set
            {
                if (Equals(_selected, value))
                {
                    return;
                }

                _selected = value;
                OnChanged();
            }
        }

        public static TP New(T all, UnityEngine.Object owner)
        {
            using (_PRF_New.Auto())
            {
                return New(all, all, all, all, all, all, owner);
            }
        }

        public static TP New(
            T normal,
            T hovering,
            T pressed,
            T dragging,
            T selected,
            T disabled,
            UnityEngine.Object owner)
        {
            using (_PRF_New.Auto())
            {
                var result = CreateWithOwner(owner);

                result._normal = normal;
                result._hovering = hovering;
                result._pressed = pressed;
                result._dragging = dragging;
                result._selected = selected;
                result._disabled = disabled;

                return result;
            }
        }

        public T GetByState(SelectableState state)
        {
            using (_PRF_GetByState.Auto())
            {
                var primaryState = state.GetPrimaryState();

                return primaryState switch
                {
                    SelectableState.Normal   => Normal,
                    SelectableState.Dragging => Dragging,
                    SelectableState.Hovering => Hovering,
                    SelectableState.Disabled => Disabled,
                    SelectableState.Pressed  => Pressed,
                    SelectableState.Selected => Selected,
                    _                        => throw new ArgumentOutOfRangeException(nameof(state), state, null)
                };
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetByState = new ProfilerMarker(_PRF_PFX + nameof(GetByState));

        private static readonly ProfilerMarker _PRF_New = new ProfilerMarker(_PRF_PFX + nameof(New));

        #endregion
    }
}
