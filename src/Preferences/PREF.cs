#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences
{
    [ShowInInspector]
    [InlineProperty]
    [HideReferenceObjectPicker]
    public sealed class PREF<T> : PREF_BASE
    {
        private readonly T _defaultValue;
        private readonly T _high;

        private readonly T _low;

        private readonly PREF_STATE<T> _prefs;

        [HideLabel]
        [InlineProperty]
        [OnValueChanged(nameof(UIApplyValue))]
        [SerializeField]
        [InlineButton(nameof(Reset), " Reset ")]
        private T _value;

        internal PREF(
            string key,
            string grouping,
            string label,
            T defaultValue,
            T low,
            T high,
            int order,
            bool reset) : base(key, grouping, label,  order)
        {
            _defaultValue = defaultValue;
            _low = low;
            _high = high;
            _prefs = PREF_STATES.Get<T>();
            _reset = reset;
        }


        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_isAwake)
                {
                    _prefs.API.Save(_key, _value, _low, _high);
                }
            }
        }

        public T v
        {
            get => Value;
            set => Value = value;
        }

        internal T Low => _low;
        internal T High => _high;

        private void UIApplyValue()
        {
            if (_isAwake)
            {
                _prefs.API.Save(_key, _value, _low, _high);
            }
        }

        /*[HorizontalGroup("A", 52f)]
        [Button("Awake", ButtonSizes.Small)]
        [DisableIf(nameof(_isAwake))]*/
        public void WakeUp()
        {
            _value = _prefs.API.Get(_key, _defaultValue, _low, _high);

            ExecuteResetIfNecessary();
            _isAwake = true;
            
            OnAwake?.Invoke(this);
        }

        /*[HorizontalGroup("A", 58f)]
        [Button("Refresh", ButtonSizes.Small)]*/
        private void Refresh()
        {
            if (_isAwake)
            {
                _value = _prefs.API.Get(_key, _defaultValue, _low, _high);
            }
        }

        private void Reset()
        {
            _reset = true;
            ExecuteResetIfNecessary();
        }

        private void ExecuteResetIfNecessary()
        {
            if (_reset)
            {
                _value = _defaultValue;

                if (_isAwake)
                {
                    _prefs.API.Save(_key, _defaultValue, _low, _high);
                    _reset = false;
                }
            }
        }
        
        public delegate void OnAwakeCallback(PREF<T> awakened);

        public event OnAwakeCallback OnAwake;
    }
}
