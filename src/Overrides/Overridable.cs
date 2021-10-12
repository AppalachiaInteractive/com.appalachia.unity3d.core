#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides
{
    [Serializable]
    [InlineProperty]
    [SmartLabel]
    public abstract class Overridable<T, TO> : IEquatable<Overridable<T, TO>>
        where TO : Overridable<T, TO>, new()
    {
        /*[SerializeField, HideInInspector]
        public bool isOverridingAllowed;*/

        [SerializeField]
        [HideLabel]
        [PropertyTooltip("Override")]
        [ToggleLeft]
        [HorizontalGroup("A", .02f)]
        public bool overrideEnabled;

        [SerializeField]
        [HideLabel]
        [LabelWidth(0)]
        [EnableIf(nameof(overrideEnabled))]
        [InlineProperty]
        [HorizontalGroup("A", .98f)]
        public T value;

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

        public static implicit operator T(Overridable<T, TO> a)
        {
            return a.value;
        }

        public static implicit operator Overridable<T, TO>(T a)
        {
            var to = new TO {overrideEnabled = false, value = a};

            return to;
        }

#region IEquatable

        public bool Equals(Overridable<T, TO> other)
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

        public override bool Equals(object obj)
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

            return Equals((Overridable<T, TO>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;

                //hashCode = isOverridingAllowed.GetHashCode();
                //hashCode = (hashCode * 397) ^ overrideEnabled.GetHashCode();

                hashCode = overrideEnabled.GetHashCode();
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(value);
                return hashCode;
            }
        }

        public static bool operator ==(Overridable<T, TO> left, Overridable<T, TO> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Overridable<T, TO> left, Overridable<T, TO> right)
        {
            return !Equals(left, right);
        }

#endregion
    }
}
