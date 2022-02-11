using System;
using System.Collections.Generic;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Unity.Profiling;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBase : AppalachiaSimpleBase
    {
        protected AppalachiaBase(Object owner)
        {
            _owner = owner;

            if (AppaTask.ExecutionIsAllowed && !APPASERIALIZE.CouldBeInSerializationWindow)
            {
                HandleInitialization().Forget();
            }
            else
            {
                _initializationFunctions ??= new Queue<Func<AppaTask>>();
                _initializationFunctions.Enqueue(HandleInitialization);
            }
        }

       
        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaBase) + ".";

        #endregion
    }

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
