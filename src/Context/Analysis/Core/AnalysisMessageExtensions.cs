using System.Collections.Generic;

namespace Appalachia.Core.Context.Analysis.Core
{
    public static class AnalysisMessageExtensions
    {
        public static void Add(
            this List<AnalysisMessage> messages,
            bool isIssue,
            params AnalysisMessagePart[] parts)
        {
            messages.Add(new AnalysisMessage(isIssue, parts));
        }
    }
}
