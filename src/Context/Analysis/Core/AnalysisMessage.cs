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

        #region Static Fields and Autoproperties

        private static StringBuilder _builder;

        #endregion

        #region Fields and Autoproperties

        public readonly AnalysisMessagePart[] parts;

        public readonly bool isIssue;

        #endregion

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
