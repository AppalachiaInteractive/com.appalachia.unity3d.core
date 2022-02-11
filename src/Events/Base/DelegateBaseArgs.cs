using System;
using Appalachia.Core.ObjectPooling;
using Unity.Profiling;

namespace Appalachia.Core.Events.Base
{
    public abstract class DelegateBaseArgs<T> : SelfPoolingObject<T>, IDisposable
        where T : DelegateBaseArgs<T>, new()
    {
#pragma warning disable CS0612
        protected DelegateBaseArgs()
#pragma warning restore CS0612
        {
        }

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                OnInitialize();
            }
        }

        public override void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                OnReset();
            }
        }

        public T Configure(Action<T> configuration)
        {
            using (_PRF_Configure.Auto())
            {
                configuration(this as T);

                return this as T;
            }
        }

        protected virtual void OnInitialize()
        {
            using (_PRF_OnInitialize.Auto())
            {
            }
        }

        protected virtual void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            using (_PRF_Dispose.Auto())
            {
                Return();
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        protected static readonly ProfilerMarker
            _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        protected static readonly ProfilerMarker _PRF_OnInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitialize));

        protected static readonly ProfilerMarker
            _PRF_OnReset = new ProfilerMarker(_PRF_PFX + nameof(OnReset));

        #endregion
    }
}
