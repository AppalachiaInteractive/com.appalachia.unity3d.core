using System;
using Appalachia.Core.Objects.Root.Contracts;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public sealed class AppalachiaRepositorySingletonReference : AppalachiaRepositoryReference
    {
        public AppalachiaRepositorySingletonReference(string addressableGuid, Type type, ISingleton instance)
            : base(addressableGuid, type)
        {
            this.instance = instance;
        }

        #region Fields and Autoproperties

        [PropertyOrder(0), ShowInInspector, ReadOnly, HideLabel]
        public ISingleton instance { get; set; }

        #endregion

#if UNITY_EDITOR
        protected override string GetReferenceName()
        {
            return instance?.Name;
        }

        protected override bool _showAssetRefDisplayValue => instance == null;
#endif
    }
}
