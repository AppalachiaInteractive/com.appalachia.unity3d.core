using UnityEngine;

namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisMessagePart
    {
        public AnalysisMessagePart(int value, Color color, TextAnchor alignment) : this(
            value.ToString(),
            color,
            alignment
        )
        {
        }

        public AnalysisMessagePart(float value, Color color, TextAnchor alignment) : this(
            value.ToString(),
            color,
            alignment
        )
        {
        }

        public AnalysisMessagePart(string text, Color color, TextAnchor alignment)
        {
            this.text = text;
            this.color = color;
            this.alignment = alignment;
        }

        public Color color;
        public string text;
        public TextAnchor alignment;

        public static AnalysisMessagePart Center(string text, bool isIssue, Color issueColor, Color goodColor)
        {
            return new(text, isIssue ? issueColor : goodColor, TextAnchor.MiddleCenter);
        }

        public static AnalysisMessagePart Center(string text, Color color)
        {
            return new(text, color, TextAnchor.MiddleCenter);
        }

        public static AnalysisMessagePart Left(string text, bool isIssue, Color issueColor, Color goodColor)
        {
            return new(text, isIssue ? issueColor : goodColor, TextAnchor.MiddleLeft);
        }

        public static AnalysisMessagePart Left(string text, Color color)
        {
            return new(text, color, TextAnchor.MiddleLeft);
        }

        public static AnalysisMessagePart[] Paired(
            string text1,
            string text2,
            bool isIssue,
            Color issueColor,
            Color goodColor)
        {
            return new[]
            {
                new AnalysisMessagePart(text1, isIssue ? issueColor : goodColor, TextAnchor.MiddleRight),
                new AnalysisMessagePart(text2, isIssue ? issueColor : goodColor, TextAnchor.MiddleLeft)
            };
        }

        public static AnalysisMessagePart[] Paired(string text1, string text2, Color color)
        {
            return new[]
            {
                new AnalysisMessagePart(text1, color, TextAnchor.MiddleRight),
                new AnalysisMessagePart(text2, color, TextAnchor.MiddleLeft)
            };
        }

        public static AnalysisMessagePart Right(string text, bool isIssue, Color issueColor, Color goodColor)
        {
            return new(text, isIssue ? issueColor : goodColor, TextAnchor.MiddleRight);
        }

        public static AnalysisMessagePart Right(string text, Color color)
        {
            return new(text, color, TextAnchor.MiddleRight);
        }
    }
}
