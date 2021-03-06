using Appalachia.Core.Attributes;

namespace Appalachia.Core.Execution.Hooks
{
    [ExecutionOrder(ExecutionOrders.FrameEnd)]
    public class FrameEnd : FrameEventBehaviour<FrameEnd>
    {
        /// <inheritdoc />
        protected override string GetReadableName()
        {
            return nameof(FrameEnd);
        }
    }
}
