using Appalachia.Core.Attributes;

namespace Appalachia.Core.Execution.Hooks
{
    [ExecutionOrder(30000)]
    public class FrameEnd : FrameEventBehaviour<FrameEnd>
    {
        protected override string GetReadableName()
        {
            const string name = nameof(FrameEnd);
            return name;
        }
    }
}
