using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBase<T> : AppalachiaBase
        where T : AppalachiaBase<T>, new()
    {
#pragma warning disable CS0612
        protected AppalachiaBase() : base(null)
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
