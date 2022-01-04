#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Objects.Scriptables
{
    [Serializable]
    public abstract class AutonamedIdentifiableAppalachiaObject<T> : IdentifiableAppalachiaObject<T>,
                                                                     IComparable<
                                                                         AutonamedIdentifiableAppalachiaObject
                                                                         <T>>,
                                                                     IComparable
        where T : AutonamedIdentifiableAppalachiaObject<T>
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

        [DebuggerStepThrough]
        public static bool operator >(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) > 0;
        }

        [DebuggerStepThrough]
        public static bool operator >=(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) >= 0;
        }

        [DebuggerStepThrough]
        public static bool operator <(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) < 0;
        }

        [DebuggerStepThrough]
        public static bool operator <=(
            AutonamedIdentifiableAppalachiaObject<T> left,
            AutonamedIdentifiableAppalachiaObject<T> right)
        {
            return Comparer<AutonamedIdentifiableAppalachiaObject<T>>.Default.Compare(left, right) <= 0;
        }

        public void UpdateName()
        {
            using (_PRF_UpdateName.Auto())
            {
#if UNITY_EDITOR
                if (name != profileName)
                {
                    Rename(profileName);

                    UpdateAllIDs();
                }
#endif
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await initializer.Do(
                    this,
                    nameof(profileName),
                    profileName.IsNullOrWhiteSpace(),
                    () =>
                    {
                        profileName = name;

                        UpdateName();
                    }
                );
            }
        }

#if UNITY_EDITOR
        protected override void OnUpdateAllIDs()
        {
            using (_PRF_OnUpateAllIDs.Auto())
            {
                if (string.IsNullOrWhiteSpace(profileName))
                {
                    Rename(profileName);
                }
            }
        }
#endif

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            using (_PRF_CompareTo.Auto())
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
                        ZString.Format(
                            "Object must be of type {0}",
                            nameof(AutonamedIdentifiableAppalachiaObject<T>)
                        )
                    );
            }
        }

        #endregion

        #region IComparable<AutonamedIdentifiableAppalachiaObject<T>> Members

        [DebuggerStepThrough]
        public int CompareTo(AutonamedIdentifiableAppalachiaObject<T> other)
        {
            using (_PRF_CompareTo.Auto())
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
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AutonamedIdentifiableAppalachiaObject<T>) + ".";

        private static readonly ProfilerMarker _PRF_UpdateName =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateName));

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_OnUpateAllIDs =
            new ProfilerMarker(_PRF_PFX + nameof(OnUpdateAllIDs));
#endif

        private static readonly ProfilerMarker _PRF_CompareTo =
            new ProfilerMarker(_PRF_PFX + nameof(CompareTo));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
