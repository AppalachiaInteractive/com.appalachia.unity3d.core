using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IInitializable : INamed
    {
        public event InitializationCompleteHandler InitializationComplete;
        public bool FullyInitialized { get; }

        public bool HasInitializationStarted { get; }

        public bool HasInvokedInitializationCompleted { get; }

        InitializationState initializationState { get; }

        void AfterInitialization();

        void BeforeInitialization();

        AppaTask ExecuteInitialization();

        void InitializationExceptionHandler(Exception ex);

        AppaTask Initialize(Initializer initializer);

        void InitializeSynchronous();
    }
}
