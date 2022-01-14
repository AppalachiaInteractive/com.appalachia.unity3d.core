using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBase : AppalachiaSimpleBase
    {
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
        where T : AppalachiaBase<T>
    {
        protected AppalachiaBase(Object owner) : base(owner)
        {
        }

        #region Profiling

        protected const string _PRF_PFX = nameof(AppalachiaBase<T>) + ".";

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        #endregion
    }
}
