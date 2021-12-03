#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Appalachia.Core.Overrides
{
    [Serializable]
    [InlineProperty]
    [SmartLabel]
    public abstract class Overridable<T, TO> : IEquatable<Overridable<T, TO>>
        where TO : Overridable<T, TO>, new()
    {
        protected Overridable(bool overrideEnabled, T value)
        {
            //this.isOverridingAllowed = isOverridingAllowed;
            this.overrideEnabled = overrideEnabled;
            this.value = value;
        }

        protected Overridable(Overridable<T, TO> value)
        {
            //this.isOverridingAllowed = value.isOverridingAllowed;
            overrideEnabled = value.overrideEnabled;
            this.value = value.value;
        }

        #region Fields and Autoproperties

        /*[SerializeField, HideInInspector]
        public bool isOverridingAllowed;*/

        [SerializeField]
        [HideLabel]
        [LabelWidth(0)]
        [PropertyTooltip("Override")]
        [HorizontalGroup("A", .02f)]
        [PropertyOrder(10)]
        [FormerlySerializedAs("overrideEnabled")]
        private bool _overrideEnabled;

        [SerializeField]
        [HideLabel]
        [LabelWidth(0)]
        [EnableIf(nameof(overrideEnabled))]
        [InlineProperty]
        [HorizontalGroup("A", .98f)]
        [PropertyOrder(0)]
        [FormerlySerializedAs("value")]
        private T _value;

        #endregion

        public bool overrideEnabled
        {
            get => _overrideEnabled;
            set => _overrideEnabled = value;
        }

        public T value
        {
            get => _value;
            set => _value = value;
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
        public static implicit operator Overridable<T, TO>(T a)
        {
            using (_PRF_op_Implicit.Auto())
            {
                var to = new TO { overrideEnabled = false, value = a };

                return to;
            }
        }

        [DebuggerStepThrough]
        public static implicit operator T(Overridable<T, TO> a)
        {
            using (_PRF_op_Implicit.Auto())
            {
                return a.value;
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

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            using (_PRF_GetHashCode.Auto())
            {
                unchecked
                {
                    int hashCode;

                    //hashCode = isOverridingAllowed.GetHashCode();
                    //hashCode = (hashCode * 397) ^ overrideEnabled.GetHashCode();

                    hashCode = overrideEnabled.GetHashCode();
                    hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(value);
                    return hashCode;
                }
            }
        }

        public T Get(T defaultValue)
        {
            using (_PRF_Get.Auto())
            {
                if (overrideEnabled)
                {
                    return value;
                }

                return defaultValue;
            }
        }

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
                    (overrideEnabled == other.overrideEnabled) &&
                    EqualityComparer<T>.Default.Equals(value, other.value);
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(Overridable<T, TO>) + ".";

        private static readonly ProfilerMarker _PRF_op_Implicit =
            new ProfilerMarker(_PRF_PFX + "op_Implicit");

        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));
        private static readonly ProfilerMarker _PRF_Equals = new ProfilerMarker(_PRF_PFX + nameof(Equals));

        private static readonly ProfilerMarker _PRF_GetHashCode =
            new ProfilerMarker(_PRF_PFX + nameof(GetHashCode));

        private static readonly ProfilerMarker _PRF_eq = new ProfilerMarker(_PRF_PFX + "eq");

        private static readonly ProfilerMarker _PRF_neq = new ProfilerMarker(_PRF_PFX + "neq");

        #endregion
    }
}
