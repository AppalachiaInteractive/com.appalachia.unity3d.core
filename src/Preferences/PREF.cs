#region

using System.Diagnostics;
using Appalachia.Core.Preferences.API;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences
{
    [DebuggerStepThrough]
    [ShowInInspector]
    [InlineProperty]
    [HideReferenceObjectPicker]
    public sealed class PREF<T> : PREF_BASE
    {
        internal PREF(
            string key,
            string grouping,
            string label,
            T defaultValue,
            T low,
            T high,
            int order,
            bool reset,
            IPAPI<T> api,
            string header) : base(key, grouping, label, order, header)
        {
            _defaultValue = defaultValue;
            _low = low;
            _high = high;
            _api = api;
            _reset = reset;
        }

        private readonly IPAPI<T> _api;
        private readonly T _defaultValue;
        private readonly T _high;

        private readonly T _low;

        [HideLabel]
        [InlineProperty]
        [OnValueChanged(nameof(UIApplyValue))]
        [SerializeField]
        [InlineButton(nameof(Reset), " Reset ")]
        private T _value;

        public T v
        {
            get => Value;
            set => Value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_isAwake)
                {
                    _api.Save(_key, _value, _low, _high);
                }
            }
        }

        internal T High => _high;

        internal T Low => _low;

        public event OnAwakeCallback OnAwake;

        /*[HorizontalGroup("A", 52f)]
        [Button("Awake", ButtonSizes.Small)]
        [DisableIf(nameof(_isAwake))]*/
        public void WakeUp()
        {
            _value = _api.Get(_key, _defaultValue, _low, _high);

            ExecuteResetIfNecessary();
            _isAwake = true;

            OnAwake?.Invoke(this);
        }

        private void ExecuteResetIfNecessary()
        {
            if (_reset)
            {
                _value = _defaultValue;

                if (_isAwake)
                {
                    _api.Save(_key, _defaultValue, _low, _high);
                    _reset = false;
                }
            }
        }

        /*[HorizontalGroup("A", 58f)]
        [Button("Refresh", ButtonSizes.Small)]*/
        private void Refresh()
        {
            if (_isAwake)
            {
                _value = _api.Get(_key, _defaultValue, _low, _high);
            }
        }

        private void Reset()
        {
            _reset = true;
            ExecuteResetIfNecessary();
        }

        private void UIApplyValue()
        {
            if (_isAwake)
            {
                _api.Save(_key, _value, _low, _high);
            }
        }

        [DebuggerStepThrough] public static implicit operator T(PREF<T> o)
        {
            return o.Value;
        }

        public delegate void OnAwakeCallback(PREF<T> awakened);
    }
}
