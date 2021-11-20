#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class AutonamedIdentifiableAppalachiaObject : IdentifiableAppalachiaObject,
                                                                  IComparable<
                                                                      AutonamedIdentifiableAppalachiaObject>,
                                                                  IComparable
    {
        #region Fields and Autoproperties

#if UNITY_EDITOR
        [SmartFoldoutGroup(GROUP)]
        [OnValueChanged(nameof(UpdateName))]
        [DelayedProperty]
        [PropertyOrder(-1000)]
        [SmartLabel]
#endif
        public string profileName;

        #endregion

        #region Event Functions

        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (string.IsNullOrWhiteSpace(profileName))
            {
                profileName = name;
#if UNITY_EDITOR
                UpdateName();
#endif
            }
        }

        #endregion

        [DebuggerStepThrough]
        public static bool operator >(
            AutonamedIdentifiableAppalachiaObject left,
            AutonamedIdentifiableAppalachiaObject right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject>.Default.Compare(left, right) > 0;
        }

        [DebuggerStepThrough]
        public static bool operator >=(
            AutonamedIdentifiableAppalachiaObject left,
            AutonamedIdentifiableAppalachiaObject right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject>.Default.Compare(left, right) >= 0;
        }

        [DebuggerStepThrough]
        public static bool operator <(
            AutonamedIdentifiableAppalachiaObject left,
            AutonamedIdentifiableAppalachiaObject right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject>.Default.Compare(left, right) < 0;
        }

        [DebuggerStepThrough]
        public static bool operator <=(
            AutonamedIdentifiableAppalachiaObject left,
            AutonamedIdentifiableAppalachiaObject right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject>.Default.Compare(left, right) <= 0;
        }

#if UNITY_EDITOR
        public void UpdateName()
        {
            if (name != profileName)
            {
                Rename(profileName);

                UpdateAllIDs();
            }
        }
#endif

#if UNITY_EDITOR
        protected override void OnUpateAllIDs()
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                Rename(profileName);
            }
        }
#endif

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return obj is AutonamedIdentifiableAppalachiaObject other
                ? CompareTo(other)
                : throw new ArgumentException(
                    $"Object must be of type {nameof(AutonamedIdentifiableAppalachiaObject)}"
                );
        }

        #endregion

        #region IComparable<AutonamedIdentifiableAppalachiaObject> Members

        [DebuggerStepThrough]
        public int CompareTo(AutonamedIdentifiableAppalachiaObject other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            var selfSavingAndIdentifyingScriptableObjectComparison = base.CompareTo(other);
            if (selfSavingAndIdentifyingScriptableObjectComparison != 0)
            {
                return selfSavingAndIdentifyingScriptableObjectComparison;
            }

            return string.Compare(profileName, other.profileName, StringComparison.Ordinal);
        }

        #endregion
    }
}
