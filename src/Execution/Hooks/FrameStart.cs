#region

using Appalachia.Core.Attributes;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
    [ExecutionOrder(-30000)]
    public class FrameStart : FrameEventBehaviour<FrameStart>
    {
        protected override string GetReadableName()
        {
            return nameof(FrameStart);
        }
    }
}
