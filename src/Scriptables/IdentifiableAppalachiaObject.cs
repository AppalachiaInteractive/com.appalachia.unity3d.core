#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Assets;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class IdentifiableAppalachiaObject<T> : AppalachiaObject<T>, IComparable<T>, IComparable
        where T : IdentifiableAppalachiaObject<T>
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(IdentifiableAppalachiaObject<T>) + ".";

        #endregion

#if UNITY_EDITOR
        [FoldoutGroup("Metadata", false)]
        [ReadOnly]
        [PropertyOrder(-100)]
        [HorizontalGroup("Metadata/ID", .5f)]
        [SmartLabel]
        [ShowIf(nameof(ShowIDProperties))]
#endif
        public int id;

#if UNITY_EDITOR
        protected virtual bool ShowIDProperties => true;
#endif

        #region IComparable

        [DebuggerStepThrough] public int CompareTo(object obj)
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
                    $"Object must be of type {nameof(IdentifiableAppalachiaObject<T>)}"
                );
        }

        [DebuggerStepThrough] public int CompareTo(T other)
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

        [DebuggerStepThrough] public static bool operator <(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) < 0;
        }

        [DebuggerStepThrough] public static bool operator >(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) > 0;
        }

        [DebuggerStepThrough] public static bool operator <=(
            IdentifiableAppalachiaObject<T> left,
            IdentifiableAppalachiaObject<T> right)
        {
            return Comparer<IdentifiableAppalachiaObject<T>>.Default.Compare(left, right) <= 0;
        }

        [DebuggerStepThrough] public static bool operator >=(
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
        [HorizontalGroup("Metadata/ID")]
        [ShowIf(nameof(ShowIDProperties))]
        [LabelText("Update")]
        public static void UpdateAllIDs()
        {
            if (Application.isPlaying)
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

                var all = GetAllOfType();

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

                        instance.OnUpateAllIDs();

                        instance.SetDirty();
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

        protected virtual void OnUpateAllIDs()
        {
        }

        public override void OnCreate()
        {
            NewID();
        }

        private static readonly ProfilerMarker _PRF_CheckForBadIDs = new(_PRF_PFX + nameof(CheckForBadIDs));

        [ShowInInspector]
        [Button]
        [PropertyOrder(-99)]
        [HorizontalGroup("Metadata/ID")]
        [ShowIf(nameof(ShowIDProperties))]
        [LabelText("Check")]
        public static void CheckForBadIDs()
        {
            if (Application.isPlaying)
            {
                return;
            }

            using (_PRF_CheckForBadIDs.Auto())
            {
                var all = GetAllOfType();

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
        [HorizontalGroup("Metadata/ID")]
        [ShowIf(nameof(ShowIDProperties))]
        public void NewID()
        {
            using (_PRF_NewID.Auto())
            {
                if (Application.isPlaying)
                {
                    return;
                }

                var all = GetAllOfType();

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
                SetDirty();

                hasBadIDs = false;
                AssetDatabaseSaveManager.SaveAssetsNextFrame();
            }
        }

#endif
    }
}
