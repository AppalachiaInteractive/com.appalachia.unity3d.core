using System;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public abstract partial class AppalachiaRepositoryReference
    {
        protected AppalachiaRepositoryReference(string addressableGuid, Type type)
        {
            _assetReference = new AssetReference(addressableGuid);
            _typeName = type.Name;
            _typeFullName = type.FullName;
            _assemblyName = type.Assembly.FullName;
        }

        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private AssetReference _assetReference;

        [SerializeField, HideInInspector]
        private string _assemblyName;

        [SerializeField, HideInInspector]
        private string _typeFullName;

        [SerializeField, HideInInspector]
        private string _typeName;

        private Type _referenceType;

        #endregion

        public AssetReference assetReference => _assetReference;
        public string assemblyName => _assemblyName;
        public string TypeFullName => _typeFullName;
        public string TypeName => _typeName;

        public Type GetReferenceType()
        {
            using (_PRF_GetReferenceType.Auto())
            {
                if (_referenceType == null)
                {
                    _referenceType = ReflectionExtensions.GetByName(TypeFullName);
                }

                return _referenceType;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryReference) + ".";

        private static readonly ProfilerMarker _PRF_GetReferenceType =
            new ProfilerMarker(_PRF_PFX + nameof(GetReferenceType));

        #endregion
    }
}
