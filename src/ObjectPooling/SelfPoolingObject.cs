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
            using (_PRF_SelfPoolingObject_Get.Auto())
            {
                _initializing = true;

                if (_internalPool == null)
                {
                    using (_PRF_SelfPoolingObject_Get_CreatePool.Auto())
                    {
                        _internalPool = ObjectPoolProvider.Create<T>(ExecuteReset, ExecuteInitialize);
                    }
                }

                var result = _internalPool.Get();

                _initializing = false;
                return result;
            }
        }

        public override void Return()
        {
            using (_PRF_SelfPoolingObject_Return.Auto())
            {
                _internalPool.Return((T)this);
            }
        }

        private static void ExecuteInitialize(T obj)
        {
            using (_PRF_SelfPoolingObject_ExecuteInitialize.Auto())
            {
                obj.Initialize();
            }
        }

        private static void ExecuteReset(T obj)
        {
            using (_PRF_SelfPoolingObject_ExecuteReset.Auto())
            {
                obj.Reset();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_SelfPoolingObject_ExecuteInitialize =
            new("SelfPoolingObject.ExecuteInitialize");

        private static readonly ProfilerMarker _PRF_SelfPoolingObject_ExecuteReset =
            new("SelfPoolingObject.ExecuteReset");

        private static readonly ProfilerMarker _PRF_SelfPoolingObject_Get = new("SelfPoolingObject.Get");

        private static readonly ProfilerMarker _PRF_SelfPoolingObject_Get_CreatePool =
            new("SelfPoolingObject.Get.CreatePool");

        private static readonly ProfilerMarker _PRF_SelfPoolingObject_Return =
            new("SelfPoolingObject.Return");

        #endregion
    }
}
