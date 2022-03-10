#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Objects.Scriptables
{
    [Serializable]
    public abstract partial class IdentifiableAppalachiaObject<T> : AppalachiaObject<T>,
                                                                    IComparable<IdentifiableAppalachiaObject<T>>,
                                                                    IComparable
        where T : IdentifiableAppalachiaObject<T>
    {
        #region Fields and Autoproperties

        [ReadOnly]
        [PropertyOrder(-100)]
        [HorizontalGroup(GROUP_INTERNAL + "/" + "ID")]
        [SmartLabel]
        [HideIf(nameof(HideIDProperties))]
        public int id;

        #endregion

        protected virtual bool HideIDProperties => false;

        #region IComparable

        [DebuggerStepThrough]
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return obj is IdentifiableAppalachiaObject<T> other
                ? CompareTo(other)
                : throw new ArgumentException(
                    ZString.Format("Object must be of type {0}", nameof(IdentifiableAppalachiaObject<T>))
                );
        }

        [DebuggerStepThrough]
        public int CompareTo(IdentifiableAppalachiaObject<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return id.CompareTo(other.id);
        }

        [DebuggerStepThrough]
        public static bool operator <(IdentifiableAppalachiaObject<T> left, IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) < 0;
        }

        [DebuggerStepThrough]
        public static bool operator >(IdentifiableAppalachiaObject<T> left, IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) > 0;
        }

        [DebuggerStepThrough]
        public static bool operator <=(IdentifiableAppalachiaObject<T> left, IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) <= 0;
        }

        [DebuggerStepThrough]
        public static bool operator >=(IdentifiableAppalachiaObject<T> left, IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) >= 0;
        }

        #endregion
    }
}
