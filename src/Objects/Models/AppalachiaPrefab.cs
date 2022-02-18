using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Models
{
    public class AppalachiaPrefab : AppalachiaBehaviour<AppalachiaPrefab>
    {
        #region Static Fields and Autoproperties

        private static string[] _prefabAddresses;

        #endregion

        #region Fields and Autoproperties

        [SerializeField, ValueDropdown(nameof(GetPrefabValues))]
        [OnValueChanged(nameof(OnPrefabAddressChanged))]
        [InlineButton(nameof(RefreshOptions))]
        private string _prefabAddress;

        [NonSerialized, ShowInInspector, ReadOnly]
        private GameObject _prefab;

        #endregion

        public GameObject prefab => _prefab;

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            await ExecutePrefabLookup();
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();
            using (_PRF_WhenEnabled.Auto())
            {
                if (AppalachiaApplication.IsPlaying)
                {
                    InstantiatePrefab();
                }
            }
        }

        private static string[] GetPrefabValues()
        {
            using (_PRF_GetPrefabValues.Auto())
            {
                if (_prefabAddresses == null)
                {
                    _prefabAddresses = AppalachiaRepository.GetPrefabAddresses();
                }

                return _prefabAddresses;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
#endif
        private static void RefreshOptions()
        {
            using (_PRF_RefreshOptions.Auto())
            {
                _prefabAddresses = null;
            }
        }

        private async AppaTask ExecutePrefabLookup()
        {
            if ((_prefab == null) && _prefabAddress.IsNotNullOrWhiteSpace())
            {
                _prefab = await AppalachiaRepository.FindPrefab(_prefabAddress);
            }
        }

        private void InstantiatePrefab()
        {
            using (_PRF_InstantiatePrefab.Auto())
            {
                if (prefab == null)
                {
                    return;
                }

                var t = transform;
                var instance = prefab.InstantiatePrefab(t.parent, t.worldToLocalMatrix);
                instance.name = name;

                this.DestroySafely();
            }
        }

        private void OnPrefabAddressChanged()
        {
            using (_PRF_OnPrefabChangedAddress.Auto())
            {
                if (_prefab == null)
                {
                    ExecutePrefabLookup().Forget();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RefreshOptions =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshOptions));

        private static readonly ProfilerMarker _PRF_InstantiatePrefab =
            new ProfilerMarker(_PRF_PFX + nameof(InstantiatePrefab));

        private static readonly ProfilerMarker _PRF_OnPrefabChangedAddress =
            new ProfilerMarker(_PRF_PFX + nameof(OnPrefabAddressChanged));

        private static readonly ProfilerMarker _PRF_GetPrefabValues =
            new ProfilerMarker(_PRF_PFX + nameof(GetPrefabValues));

        #endregion
    }
}
