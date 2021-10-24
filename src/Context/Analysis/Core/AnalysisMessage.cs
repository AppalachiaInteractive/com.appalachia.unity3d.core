namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisMessage
    {
        public AnalysisMessage(bool isIssue, params AnalysisMessagePart[] parts)
        {
            this.isIssue = isIssue;
            this.parts = parts;
        }

        public readonly bool isIssue;
        public readonly AnalysisMessagePart[] parts;
    }
}
