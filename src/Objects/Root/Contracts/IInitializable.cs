using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IInitializable : INamed
    {
        event InitializationCompleteHandler InitializationComplete;
        bool FullyInitialized { get; }

        bool HasInitializationStarted { get; }

        bool HasInvokedInitializationCompleted { get; }

        InitializationState initializationState { get; }

        void AfterInitialization();

        void BeforeInitialization();

        AppaTask ExecuteInitialization();

        void InitializationExceptionHandler(Exception ex);

        AppaTask Initialize(Initializer initializer);

        void InitializeSynchronous();
    }
}
