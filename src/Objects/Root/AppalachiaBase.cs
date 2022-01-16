using System;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBase : AppalachiaSimpleBase
    {
        [Obsolete]
        protected AppalachiaBase()
        {
        }

        protected AppalachiaBase(Object owner)
        {
            _owner = owner;
            HandleInitialization().Forget();
        }

        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        protected internal Object _owner;

        #endregion

        public Object Owner => _owner;

        public void SetOwner(Object owner)
        {
            _owner = owner;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaBase) + ".";

        #endregion
    }

    public abstract partial class AppalachiaBase<T> : AppalachiaBase
        where T : AppalachiaBase<T>, new()
    {
#pragma warning disable CS0612
        protected AppalachiaBase()
#pragma warning restore CS0612
        {
        }
        
        protected AppalachiaBase(Object owner) : base(owner)
        {
        }

        public static T CreateWithOwner(Object owner)
        {
            using (_PRF_CreateWithOwner.Auto())
            {
                var instance = new T();
                instance.SetOwner(owner);

                return instance;
            }
        }

        #region Profiling

        protected const string _PRF_PFX = nameof(AppalachiaBase<T>) + ".";

        private static readonly ProfilerMarker _PRF_CreateWithOwner =
            new ProfilerMarker(_PRF_PFX + nameof(CreateWithOwner));

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        #endregion
    }
}
