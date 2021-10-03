namespace Appalachia.Core.Execution.Hooks
{
    public sealed class FrameEventDelegates<T>
        where T : FrameEventBehaviour<T>
    {
        public event FrameActionDelegate awake;
        public event FrameActionDelegate start;
        public event FrameActionDelegate onEnable;
        public event FrameActionDelegate onDisable;
        public event FrameActionDelegate update;
        public event FrameActionDelegate fixedUpdate;
        public event FrameActionDelegate onApplicationQuit;
        public event FrameActionDelegate onDestroy;

        public void InvokeAwake()
        {
            awake?.Invoke();
        }

        public void InvokeStart()
        {
            start?.Invoke();
        }

        public void InvokeOnEnable()
        {
            onEnable?.Invoke();
        }

        public void InvokeOnDisable()
        {
            onDisable?.Invoke();
        }

        public void InvokeUpdate()
        {
            update?.Invoke();
        }

        public void InvokeFixedUpdate()
        {
            fixedUpdate?.Invoke();
        }

        public void InvokeOnApplicationQuit()
        {
            onApplicationQuit?.Invoke();
        }

        public void InvokeOnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}
