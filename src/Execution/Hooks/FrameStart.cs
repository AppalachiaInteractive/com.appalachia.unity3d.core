#region

using Appalachia.Core.Attributes;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
    [ExecutionOrder(ExecutionOrders.FrameStart)]
    public class FrameStart : FrameEventBehaviour<FrameStart>
    {
        /// <inheritdoc />
        protected override string GetReadableName()
        {
            return nameof(FrameStart);
        }
    }
}
