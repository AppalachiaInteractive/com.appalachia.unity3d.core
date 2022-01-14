using System;
using Appalachia.Core.Objects.Root.Contracts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public sealed class AppalachiaRepositorySingletonReference : AppalachiaRepositoryReference
    {
        public AppalachiaRepositorySingletonReference(string addressableGuid, Type type, ISingleton instance)
            : base(addressableGuid, type)
        {
            this.instance = instance;
#if UNITY_EDITOR
            editorAsset = instance as UnityEngine.Object;
#endif
        }

        #region Fields and Autoproperties

        [PropertyOrder(0), ShowInInspector, ReadOnly, HideLabel]
        public ISingleton instance { get; set; }

        #endregion

#if UNITY_EDITOR

        [HideInInspector] public UnityEngine.Object editorAsset;

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

        protected override bool _showAssetRefDisplayValue => instance == null;
#endif
    }
}
