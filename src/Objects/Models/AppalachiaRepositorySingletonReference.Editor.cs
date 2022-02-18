#if UNITY_EDITOR
using UnityEngine;

namespace Appalachia.Core.Objects.Models
{
    public sealed partial class AppalachiaRepositorySingletonReference
    {
        #region Fields and Autoproperties

        [HideInInspector] public Object editorAsset;

        #endregion

        /// <inheritdoc />
        protected override bool _showAssetRefDisplayValue => instance == null;

        /// <inheritdoc />
        protected override string GetReferenceName()
        {
            if (instance != null)
            {
                return instance.Name;
            }

            if (editorAsset != null)
            {
                return editorAsset.name;
            }

            return null;
        }
    }
}

#endif
