#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Assets;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Objects.Scriptables
{
    [Serializable]
    public abstract class IdentifiableAppalachiaObject<T> : AppalachiaObject<T>,
                                                            IComparable<IdentifiableAppalachiaObject<T>>,
                                                            IComparable
        where T : IdentifiableAppalachiaObject<T>
    {
        #region Fields and Autoproperties

#if UNITY_EDITOR
        [ReadOnly]
        [PropertyOrder(-100)]
        [HorizontalGroup(GROUP + "/" + "ID")]
        [SmartLabel]
        [ShowIf(nameof(ShowIDProperties))]
#endif
        public int id;

        #endregion

#if UNITY_EDITOR
        protected virtual bool ShowIDProperties => true;
#endif

        #region Profiling

        private const string _PRF_PFX = nameof(IdentifiableAppalachiaObject<T>) + ".";

        #endregion

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
        public static bool operator <(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) < 0;
        }

        [DebuggerStepThrough]
        public static bool operator >(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) > 0;
        }

        [DebuggerStepThrough]
        public static bool operator <=(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) <= 0;
        }

        [DebuggerStepThrough]
        public static bool operator >=(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) >= 0;
        }

        #endregion

#if UNITY_EDITOR

        protected bool badID => (id == 0) || hasBadIDs;

        protected static bool hasBadIDs;

        [NonSerialized] private static bool checkedEnabled;

        private static readonly ProfilerMarker _PRF_UpdateAllIDs = new(_PRF_PFX + nameof(UpdateAllIDs));

        private static readonly ProfilerMarker _PRF_UpdateAllIDs_SaveAssets =
            new(_PRF_PFX + nameof(UpdateAllIDs) + ".SaveAssets");

        private static HashSet<int> _ids;

        [ShowInInspector]
        [Button]
        [PropertyOrder(-100)]
        [EnableIf(nameof(badID))]
        [HorizontalGroup(GROUP + "/" + "ID")]
        [ShowIf(nameof(ShowIDProperties))]
        [LabelText("Update")]
        public void UpdateAllIDs()
        {
            if (AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            if (checkedEnabled)
            {
                return;
            }

            using (_PRF_UpdateAllIDs.Auto())
            {
                checkedEnabled = true;

                var updatedAny = false;
                hasBadIDs = false;

                var all = GetAllOfType(GetType()).Cast<IdentifiableAppalachiaObject<T>>().ToArray();

                var maxID = 0;

                for (var index = 0; index < all.Length; index++)
                {
                    var instance = all[index];

                    if (instance.id > maxID)
                    {
                        maxID = instance.id;
                    }
                }

                var currentID = maxID + 1;

                for (var index = 0; index < all.Length; index++)
                {
                    var instance = all[index];

                    if (instance.id == 0)
                    {
                        instance.id = currentID;
                        currentID += 1;

                        instance.OnUpdateAllIDs();

                        instance.MarkAsModified();
                        updatedAny = true;
                    }
                }

                if (_ids == null)
                {
                    _ids = new HashSet<int>();
                }
                else
                {
                    _ids.Clear();
                }

                for (var index = 0; index < all.Length; index++)
                {
                    var instance = all[index];

                    if (_ids.Contains(instance.id))
                    {
                        instance.id = currentID;
                        _ids.Add(currentID);
                        currentID += 1;
                    }
                    else
                    {
                        _ids.Add(instance.id);
                    }
                }

                if (updatedAny)
                {
                    using (_PRF_UpdateAllIDs_SaveAssets.Auto())
                    {
                        AssetDatabaseSaveManager.SaveAssetsNextFrame();
                    }
                }
            }
        }

        private static readonly ProfilerMarker _PRF_AwakeActual =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeActual));

        protected override void AwakeActual()
        {
            using (_PRF_AwakeActual.Auto())
            {
                base.AwakeActual();

                if (id == 0)
                {
                    NewID();
                }
            }
        }

        protected virtual void OnUpdateAllIDs()
        {
        }

        private static readonly ProfilerMarker _PRF_CheckForBadIDs = new(_PRF_PFX + nameof(CheckForBadIDs));

        [ShowInInspector]
        [Button]
        [PropertyOrder(-99)]
        [HorizontalGroup(GROUP + "/" + APPASTR.ID)]
        [ShowIf(nameof(ShowIDProperties))]
        [LabelText("Check")]
        public void CheckForBadIDs()
        {
            using (_PRF_CheckForBadIDs.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                var all = GetAllOfType(GetType()).Cast<IdentifiableAppalachiaObject<T>>().ToArray();

                if (_ids == null)
                {
                    _ids = new HashSet<int>();
                }
                else
                {
                    _ids.Clear();
                }

                for (var index = 0; index < all.Length; index++)
                {
                    var instance = all[index];
                    if (instance.id == 0)
                    {
                        hasBadIDs = true;
                        return;
                    }

                    if (_ids.Contains(instance.id))
                    {
                        hasBadIDs = true;
                        return;
                    }

                    _ids.Add(instance.id);
                }
            }
        }

        private static readonly ProfilerMarker _PRF_NewID = new(_PRF_PFX + nameof(NewID));

        [ShowInInspector]
        [Button]
        [PropertyOrder(-99)]
        [HorizontalGroup(GROUP + "/" + APPASTR.ID)]
        [ShowIf(nameof(ShowIDProperties))]
        public void NewID()
        {
            using (_PRF_NewID.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                var all = GetAllOfType(GetType()).Cast<IdentifiableAppalachiaObject<T>>().ToArray();

                var maxID = 0;

                for (var index = 0; index < all.Length; index++)
                {
                    var instance = all[index];
                    if (instance.id > maxID)
                    {
                        maxID = instance.id;
                    }
                }

                id = maxID + 1;
                MarkAsModified();

                hasBadIDs = false;
                AssetDatabaseSaveManager.SaveAssetsNextFrame();
            }
        }

#endif
    }
}
