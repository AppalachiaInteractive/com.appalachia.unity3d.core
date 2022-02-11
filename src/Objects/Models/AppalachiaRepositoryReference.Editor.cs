#if UNITY_EDITOR
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Objects.Models
{
    public abstract partial class AppalachiaRepositoryReference
    {
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

        protected virtual string GetReferenceName()
        {
            return null;
        }
    }
}

#endif
