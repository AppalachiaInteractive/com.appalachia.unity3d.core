using System;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public sealed class AppalachiaRepositoryPrefabReference : AppalachiaRepositoryReference
    {
        public AppalachiaRepositoryPrefabReference(string addressableGuid, string prefabAddress) : base(
            addressableGuid,
            typeof(GameObject)
        )
        {
            _prefabAddress = prefabAddress;
        }

        #region Fields and Autoproperties

        [PropertyOrder(0), ShowInInspector, ReadOnly, HideLabel]
        public GameObject prefab { get; set; }

        [SerializeField] private string _prefabAddress;

        #endregion

        public string PrefabAddress => _prefabAddress;

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryPrefabReference) + ".";

        #endregion

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_GetReferenceName =
            new ProfilerMarker(_PRF_PFX + nameof(GetReferenceName));

        protected override string GetReferenceName()
        {
            using (_PRF_GetReferenceName.Auto())
            {
                if (prefab == null)
                {
                    return _prefabAddress;
                }

                return prefab.name;
            }
        }

        protected override bool _showAssetRefDisplayValue => prefab == null;
#endif
    }
}
