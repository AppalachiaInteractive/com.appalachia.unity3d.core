#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Contracts;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Standards;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    [DebuggerDisplay("Overriding = {Overriding} | Value - {ValueDebuggerDisplay}")]
    public abstract class Overridable<T, TO> : IEquatable<Overridable<T, TO>>,
                                               IOverridable,
                                               ISerializationCallbackReceiver,
                                               IChangePublisher,
                                               IUnique,
                                               IRemotelyEnabledController
        where TO : Overridable<T, TO>, new()
    {
        protected Overridable(bool overriding, T value)
        {
            //this.isOverridingAllowed = isOverridingAllowed;
            _overriding = overriding;
            _value = value;
            _initial = value;

            if (_value is IChangePublisher irc)
            {
                irc.SubscribeToChanges(OnChanged);
            }
        }

        protected Overridable(Overridable<T, TO> value) : this(value.Overriding, value.Value)
        {
        }

        #region Preferences

        [NonSerialized] private PREF<Color> _overrideDisabledColor;

        [NonSerialized] private PREF<Color> _overrideEnabledColor;

        protected PREF<Color> overrideDisabledColor
        {
            get
            {
                if (_overrideDisabledColor == null)
                {
                    _overrideDisabledColor = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        DisabledColorPrefName,
                        Colors.WhiteSmokeGray96
                    );
                }

                return _overrideDisabledColor;
            }
        }

        protected PREF<Color> overrideEnabledColor
        {
            get
            {
                if (_overrideEnabledColor == null)
                {
                    _overrideEnabledColor = PREFS.REG(PKG.Prefs.Colors.Group, EnabledColorPrefName, Colors.PaleGreen4);
                }

                return _overrideEnabledColor;
            }
        }

        #endregion

        #region Fields and Autoproperties

        public AppaEvent.Data Changed;

        [NonSerialized] private bool _disabled;

        [NonSerialized] private bool _frozen;

        [FormerlySerializedAs("_overrideEnabled")]
        [FormerlySerializedAs("overrideEnabled")]
        [HideLabel]
        [HorizontalGroup("A", .02f)]
        [LabelWidth(0)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyOrder(10)]
        [PropertyTooltip("Override")]
        [SerializeField]
        [GUIColor(nameof(_overridingColor))]
        private bool _overriding;

        [HideInInspector]
        [SerializeField]
        private ObjectID _objectId;

        [SerializeField, HideInInspector]
        private T _initial;

        [EnableIf(nameof(Overriding))]
        [FormerlySerializedAs("value")]
        [HorizontalGroup("A", .98f)]
        [HideLabel]
        [InlineProperty]
        [LabelWidth(0)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyOrder(0)]
        [SerializeField]
        private T _value;

        #endregion

        protected virtual string DisabledColorPrefName => "Override Disabled Color";
        protected virtual string EnabledColorPrefName => "Override Enabled Color";
        protected virtual string ToggleLabel => "Override";

        public T initial => _initial;

        public T Value
        {
            get => _value;
            set
            {
                if (_frozen)
                {
                    return;
                }

                if (!Equals(_value, value))
                {
                    if (value is IChangePublisher ircNew)
                    {
                        ircNew.SubscribeToChanges(OnChanged);
                    }

                    if (_value is IChangePublisher ircOld)
                    {
                        ircOld.UnsubscribeFromChanges(OnChanged);
                    }

                    _value = value;
                    OnChanged();
                }
            }
        }

        private Color _overridingColor => _overriding ? overrideEnabledColor : overrideDisabledColor;

        private string ValueDebuggerDisplay
        {
            get
            {
                if (Value is INamed n)
                {
                    return n.Name;
                }

                return Value.ToString();
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(Overridable<T, TO> left, Overridable<T, TO> right)
        {
            using (_PRF_eq.Auto())
            {
                return Equals(left, right);
            }
        }

        [DebuggerStepThrough]
        public static implicit operator T(Overridable<T, TO> a)
        {
            using (_PRF_op_Implicit.Auto())
            {
                return a.Value;
            }
        }

        [DebuggerStepThrough]
        public static bool operator !=(Overridable<T, TO> left, Overridable<T, TO> right)
        {
            using (_PRF_neq.Auto())
            {
                return !Equals(left, right);
            }
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            using (_PRF_Equals.Auto())
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((Overridable<T, TO>)obj);
            }
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            using (_PRF_GetHashCode.Auto())
            {
                unchecked
                {
                    //hashCode = isOverridingAllowed.GetHashCode();
                    //hashCode = (hashCode * 397) ^ overrideEnabled.GetHashCode();

                    var hashCode = Overriding.GetHashCode();
                    hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
                    return hashCode;
                }
            }
        }

        public T Get(T defaultValue)
        {
            using (_PRF_Get.Auto())
            {
                if (Overriding)
                {
                    return Value;
                }

                return defaultValue;
            }
        }

        public void OverrideValue(T value)
        {
            using (_PRF_OverrideValue.Auto())
            {
                Overriding = true;
                Value = value;
            }
        }

        private void OnChanged()
        {
            using (_PRF_OnChanged.Auto())
            {
                Changed.RaiseEvent();
            }
        }

        #region IChangePublisher Members

        void IChangePublisher.OnChanged()
        {
            OnChanged();
        }

        public void SubscribeToChanges(AppaEvent.Handler handler)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                Changed.Event += handler;
            }
        }

        public void UnsubscribeFromChanges(AppaEvent.Handler handler)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                Changed.Event -= handler;
            }
        }

        #endregion

        #region IEquatable<Overridable<T,TO>> Members

        [DebuggerStepThrough]
        public bool Equals(Overridable<T, TO> other)
        {
            using (_PRF_Equals.Auto())
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return /* isOverridingAllowed == other.isOverridingAllowed && */
                    (Overriding == other.Overriding) && EqualityComparer<T>.Default.Equals(Value, other.Value);
            }
        }

        #endregion

        #region IOverridable Members

        string IOverridable.ToggleLabel => ToggleLabel;

        public bool Frozen
        {
            get => _frozen;
            set => _frozen = value;
        }

        public bool Disabled
        {
            get => _disabled;
            set => _disabled = value;
        }

        public Type ValueType => typeof(T);

        public bool Overriding
        {
            get
            {
                if (_disabled)
                {
                    return false;
                }

                return _overriding;
            }
            set
            {
                if (_frozen)
                {
                    return;
                }

                if (_overriding != value)
                {
                    _overriding = value;
                    OnChanged();
                }
            }
        }

        object IOverridable.Value => _value;
        public Color ToggleColor => _overridingColor;

        #endregion

        #region IRemotelyEnabledController Members

        public bool ShouldEnable => _overriding;

        public void BindValueEnabledState()
        {
            if (Value is IRemotelyEnabled v)
            {
                v.BindEnabledStateTo(this);
            }
        }

        #endregion

        #region ISerializationCallbackReceiver Members

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            using var scope = APPASERIALIZE.OnAfterDeserialize();

            PREFS.RegisterPreSettingsProviderAction(
                () =>
                {
                    var color1 = overrideDisabledColor;
                    var color2 = overrideEnabledColor;
                }
            );

            if ((_value != null) && _value is IChangePublisher irc)
            {
                irc.SubscribeToChanges(OnChanged);
            }
        }

        #endregion

        #region IUnique Members

        public ObjectID ObjectID
        {
            get
            {
                if ((_objectId == null) || (_objectId == ObjectID.Empty))
                {
                    _objectId = ObjectID.NewObjectID();
                    OnChanged();
                }

                return _objectId;
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(Overridable<T, TO>) + ".";

        private static readonly ProfilerMarker _PRF_OverrideValue =
            new ProfilerMarker(_PRF_PFX + nameof(OverrideValue));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromChanges));

        private static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToChanges));

        private static readonly ProfilerMarker _PRF_OnChanged = new ProfilerMarker(_PRF_PFX + nameof(OnChanged));
        private static readonly ProfilerMarker _PRF_op_Implicit = new ProfilerMarker(_PRF_PFX + "op_Implicit");
        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));
        private static readonly ProfilerMarker _PRF_Equals = new ProfilerMarker(_PRF_PFX + nameof(Equals));
        private static readonly ProfilerMarker _PRF_GetHashCode = new ProfilerMarker(_PRF_PFX + nameof(GetHashCode));
        private static readonly ProfilerMarker _PRF_eq = new ProfilerMarker(_PRF_PFX + "eq");

        private static readonly ProfilerMarker _PRF_neq = new ProfilerMarker(_PRF_PFX + "neq");

        #endregion
    }
}
