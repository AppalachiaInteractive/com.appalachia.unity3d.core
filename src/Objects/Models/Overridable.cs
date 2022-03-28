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
using Appalachia.Utility.Reflection.Extensions;
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
            this.overriding = overriding;
            this.value = value;
            _initial = value;

            if (this.value is IChangePublisher irc)
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

        [FormerlySerializedAs("_overriding")]
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
        public bool overriding;

        public AppaEvent.Data Changed;

        [FormerlySerializedAs("_value")]
        [EnableIf(nameof(Overriding))]
        [HorizontalGroup("A", .98f)]
        [HideLabel]
        [InlineProperty]
        [LabelWidth(0)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyOrder(0)]
        [SerializeField]
        [DelayedProperty]
        public T value;

        [NonSerialized] private bool _disabled;

        [NonSerialized] private bool _frozen;

        [HideInInspector]
        [SerializeField]
        private ObjectID _objectId;

        [SerializeField, HideInInspector]
        private T _initial;

        #endregion

        protected virtual string DisabledColorPrefName => "Override Disabled Color";
        protected virtual string EnabledColorPrefName => "Override Enabled Color";
        protected virtual string ToggleLabel => "Override";

        public T initial => _initial;

        public T Value
        {
            get => value;
            set
            {
                if (_frozen)
                {
                    return;
                }

                if (!Equals(this.value, value))
                {
                    if (value is IChangePublisher ircNew)
                    {
                        ircNew.SubscribeToChanges(OnChanged);
                    }

                    if (this.value is IChangePublisher ircOld)
                    {
                        ircOld.UnsubscribeFromChanges(OnChanged);
                    }

                    this.value = value;
                    OnChanged();
                }
            }
        }

        private Color _overridingColor => overriding ? overrideEnabledColor : overrideDisabledColor;

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

        public void OverrideValue(T value, bool suppressOnChanged = false)
        {
            if (_PRF_OverrideValue.Handle == IntPtr.Zero)
            {
                _PRF_OverrideValue = new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OverrideValue));
            }

            using (_PRF_OverrideValue.Auto())
            {
                if (suppressOnChanged)
                {
                    overriding = true;
                    this.value = value;
                }
                else
                {
                    Overriding = true;
                    Value = value;
                }
            }
        }

        public void OverrideValueQuiet(T value)
        {
            if (_PRF_OverrideValueQuiet.Handle == IntPtr.Zero)
            {
                _PRF_OverrideValueQuiet =
                    new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OverrideValueQuiet));
            }

            using (_PRF_OverrideValueQuiet.Auto())
            {
                overriding = true;
                this.value = value;
            }
        }

        private void OnChanged()
        {
            if (_PRF_OnChanged.Handle == IntPtr.Zero)
            {
                _PRF_OnChanged = new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OnChanged));
            }

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

        public void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                if (value is IChangePublisher p)
                {
                    p.SuspendChanges();
                }
                
                Changed.Suspend();
            }
        }

        public void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                Changed.Unsuspend();

                if (value is IChangePublisher p)
                {
                    p.UnsuspendChanges();
                }
            }
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

                return overriding;
            }
            set
            {
                if (_frozen)
                {
                    return;
                }

                if (overriding != value)
                {
                    overriding = value;
                    OnChanged();
                }
            }
        }

        object IOverridable.Value => value;
        public Color ToggleColor => _overridingColor;

        #endregion

        #region IRemotelyEnabledController Members

        public bool ShouldEnable => overriding;

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

            if ((value != null) && value is IChangePublisher irc)
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

        private static readonly ProfilerMarker _PRF_SuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromChanges));

        private static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToChanges));

        private static readonly ProfilerMarker _PRF_op_Implicit = new ProfilerMarker(_PRF_PFX + "op_Implicit");
        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));
        private static readonly ProfilerMarker _PRF_Equals = new ProfilerMarker(_PRF_PFX + nameof(Equals));
        private static readonly ProfilerMarker _PRF_GetHashCode = new ProfilerMarker(_PRF_PFX + nameof(GetHashCode));
        private static readonly ProfilerMarker _PRF_eq = new ProfilerMarker(_PRF_PFX + "eq");

        private static readonly ProfilerMarker _PRF_neq = new ProfilerMarker(_PRF_PFX + "neq");

        private ProfilerMarker _PRF_OnChanged;

        private ProfilerMarker _PRF_OverrideValue;
        private ProfilerMarker _PRF_OverrideValueQuiet;

        #endregion
    }
}
