using System;
using Appalachia.Core.Objects.Root.Contracts;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Objects.Models
{
    [Serializable]
    public sealed partial class AppalachiaRepositorySingletonReference : AppalachiaRepositoryReference
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
    }
}
