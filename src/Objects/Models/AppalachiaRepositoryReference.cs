using System;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public abstract class AppalachiaRepositoryReference
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

        #endregion

        public AssetReference assetReference => _assetReference;
        public string assemblyName => _assemblyName;
        public string TypeFullName => _typeFullName;
        public string TypeName => _typeName;

        public Type GetReferenceType()
        {
            using (_PRF_GetReferenceType.Auto())
            {
                return ReflectionExtensions.GetByName(TypeFullName);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryReference) + ".";

        private static readonly ProfilerMarker _PRF_GetReferenceType =
            new ProfilerMarker(_PRF_PFX + nameof(GetReferenceType));

        #endregion

#if UNITY_EDITOR
        protected virtual string GetReferenceName()
        {
            return null;
        }

        protected abstract bool _showAssetRefDisplayValue { get; }

        [ShowInInspector, ReadOnly, ShowIf(nameof(_showAssetRefDisplayValue))]
        private string assetReferenceDisplayValue
        {
            get
            {
                var referenceName = GetReferenceName();
                if (referenceName != null)
                {
                    return referenceName;
                }

                if (_assetReference == null)
                {
                    return "NULL";
                }

                if (_assetReference.editorAsset != null)
                {
                    return _assetReference.editorAsset.name;
                }

                if (_assetReference.SubObjectName.IsNotNullOrWhiteSpace())
                {
                    return _assetReference.SubObjectName;
                }

                if (_assetReference.AssetGUID.IsNotNullOrWhiteSpace())
                {
                    return _assetReference.AssetGUID;
                }

                return $"[MISSING] {_typeName}";
            }
        }
#endif
    }
}
