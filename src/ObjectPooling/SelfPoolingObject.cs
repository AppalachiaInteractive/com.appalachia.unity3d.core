#region

using System;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.ObjectPooling
{
    public abstract class SelfPoolingObject
    {
        public abstract void Initialize();
        public abstract void Reset();
        public abstract void Return();
    }

    public abstract class SelfPoolingObject<T> : SelfPoolingObject
        where T : SelfPoolingObject<T>, new()
    {
        [Obsolete]
        protected SelfPoolingObject()
        {
            if (!_initializing)
            {
                throw new NotSupportedException("Do not call constructor directly");
            }
        }

        #region Static Fields and Autoproperties

        private static bool _initializing;
        private static ObjectPool<T> _internalPool;

        #endregion

        public static T Get()
        {
            using (_PRF_Get.Auto())
            {
                _initializing = true;

                if (_internalPool == null)
                {
                    using (_PRF_Get_CreatePool.Auto())
                    {
                        _internalPool = ObjectPoolProvider.Create<T>(ExecuteReset, ExecuteInitialize);
                    }
                }

                var result = _internalPool.Get();

                _initializing = false;
                return result;
            }
        }

        /// <inheritdoc />
        public override void Return()
        {
            using (_PRF_Return.Auto())
            {
                _internalPool.Return((T)this);
            }
        }

        private static void ExecuteInitialize(T obj)
        {
            using (_PRF_ExecuteInitialize.Auto())
            {
                obj.Initialize();
            }
        }

        private static void ExecuteReset(T obj)
        {
            using (_PRF_ExecuteReset.Auto())
            {
                obj.Reset();
            }
        }

        #region Profiling

        protected static readonly string _PRF_PFX = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));

        private static readonly ProfilerMarker _PRF_Get_CreatePool =
            new ProfilerMarker(_PRF_PFX + nameof(Get) + ".CreatePool");

        protected static readonly ProfilerMarker _PRF_Return = new ProfilerMarker(_PRF_PFX + nameof(Return));

        protected static readonly ProfilerMarker _PRF_ExecuteInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialize));

        protected static readonly ProfilerMarker _PRF_ExecuteReset =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteReset));

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX + nameof(Reset));

        #endregion
    }
}
