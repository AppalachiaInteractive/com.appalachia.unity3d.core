using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IInitializable : INamed
    {
        bool FullyInitialized { get; }

        bool HasInitializationStarted { get; }

        bool HasInvokedInitializationCompleted { get; }

        InitializationState initializationState { get; }
        event InitializationCompleteHandler InitializationComplete;

        void InitializationExceptionHandler(Exception ex);

        AppaTask Initialize(Initializer initializer);

        void InitializeSynchronous();

        AppaTask StartInitializing();

        void AfterInitialization()
        {
        }

        void BeforeInitialization()
        {
        }
    }
}
