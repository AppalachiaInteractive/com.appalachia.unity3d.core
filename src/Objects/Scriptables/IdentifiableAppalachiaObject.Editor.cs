#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Assets;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Scriptables
{
    public abstract partial class IdentifiableAppalachiaObject<T>
        where T : IdentifiableAppalachiaObject<T>
    {
        #region Static Fields and Autoproperties

        protected static bool hasBadIDs;

        [NonSerialized] private static bool checkedEnabled;

        private static HashSet<int> _ids;

        #endregion

        protected bool badID => (id == 0) || hasBadIDs;

        [ShowInInspector]
        [Button]
        [PropertyOrder(-99)]
        [HorizontalGroup(GROUP_INTERNAL + "/" + APPASTR.ID)]
        [HideIf(nameof(HideIDProperties))]
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

        [ShowInInspector]
        [Button]
        [PropertyOrder(-99)]
        [HorizontalGroup(GROUP_INTERNAL + "/" + APPASTR.ID)]
        [HideIf(nameof(HideIDProperties))]
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

        [ShowInInspector]
        [Button]
        [PropertyOrder(-100)]
        [EnableIf(nameof(badID))]
        [HorizontalGroup(GROUP_INTERNAL + "/" + "ID")]
        [HideIf(nameof(HideIDProperties))]
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

        protected virtual void OnUpdateAllIDs()
        {
        }

        /// <inheritdoc />
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_CheckForBadIDs = new(_PRF_PFX + nameof(CheckForBadIDs));

        private static readonly ProfilerMarker _PRF_NewID = new(_PRF_PFX + nameof(NewID));

        private static readonly ProfilerMarker _PRF_UpdateAllIDs = new(_PRF_PFX + nameof(UpdateAllIDs));

        private static readonly ProfilerMarker _PRF_UpdateAllIDs_SaveAssets =
            new(_PRF_PFX + nameof(UpdateAllIDs) + ".SaveAssets");

        #endregion
    }
}

#endif
