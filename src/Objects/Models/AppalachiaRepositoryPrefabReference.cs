using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public sealed partial class AppalachiaRepositoryPrefabReference : AppalachiaRepositoryReference
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
    }
}
