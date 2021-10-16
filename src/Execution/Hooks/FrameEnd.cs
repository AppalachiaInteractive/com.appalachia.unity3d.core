using Appalachia.Core.Attributes;

namespace Appalachia.Core.Execution.Hooks
{
    [ExecutionOrder(30000)]
    public class FrameEnd : FrameEventBehaviour<FrameEnd>
    {
        protected override string GetReadableName()
        {
            return nameof(FrameEnd);
        }
    }
}
