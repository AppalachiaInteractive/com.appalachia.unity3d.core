using System.Linq;
using System.Text;

namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisMessage 
    {
        public AnalysisMessage(bool isIssue, params AnalysisMessagePart[] parts)
        {
            this.isIssue = isIssue;
            this.parts = parts;
        }

        private static StringBuilder _builder;

        public readonly AnalysisMessagePart[] parts;

        public readonly bool isIssue;

        public string PrintMessage()
        {
            _builder ??= new StringBuilder();

            _builder.Clear();
            
            var text = string.Join(
                ":",
                parts.Where(p => (p.action == null) && (p.text != null)).Select(p => p.text)
            );

            _builder.Append(text);

            return _builder.ToString();
        }
    }
}