using System;
using System.Collections.Generic;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
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
}
