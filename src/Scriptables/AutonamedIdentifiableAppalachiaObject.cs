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
    public abstract class AutonamedIdentifiableAppalachiaObject<T> : IdentifiableAppalachiaObject<T>,
                                                                     IComparable<
                                                                         AutonamedIdentifiableAppalachiaObject
                                                                         <T>>,
                                                                     IComparable
        where T : AutonamedIdentifiableAppalachiaObject<T>
    {
#if UNITY_EDITOR
        [FoldoutGroup("Metadata", false)]
        [OnValueChanged(nameof(UpdateName))]
        [DelayedProperty]
        [PropertyOrder(-1000)]
        [SmartLabel]
#endif
        public string profileName;

        protected virtual void OnEnable()
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                profileName = name;
#if UNITY_EDITOR
                UpdateName();
#endif
            }
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

        [DebuggerStepThrough] public int CompareTo(AutonamedIdentifiableAppalachiaObject<T> other)
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
        
#if UNITY_EDITOR
        protected override void OnUpateAllIDs()
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                Rename(profileName);
            }
        }
#endif
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

            return obj is AutonamedIdentifiableAppalachiaObject<T> other
                ? CompareTo(other)
                : throw new ArgumentException(
                    $"Object must be of type {nameof(AutonamedIdentifiableAppalachiaObject<T>)}"
                );
        }

        [DebuggerStepThrough] public static bool operator >(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) > 0;
        }

        [DebuggerStepThrough] public static bool operator >=(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) >= 0;
        }

        [DebuggerStepThrough] public static bool operator <(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) < 0;
        }

        [DebuggerStepThrough] public static bool operator <=(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) <= 0;
        }
    }
}
