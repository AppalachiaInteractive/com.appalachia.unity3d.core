using System;
using UnityEngine;

namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisMessagePart
    {
        private const float DEFAULT_WIDTH = 60f;
            
        public AnalysisMessagePart(
            int value,
            Color color,
            TextAnchor alignment,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH) : this(value.ToString(), color, alignment, expandWidth, width)
        {
        }

        public AnalysisMessagePart(
            float value,
            Color color,
            TextAnchor alignment,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH) : this(value.ToString(), color, alignment, expandWidth, width)
        {
        }

        public AnalysisMessagePart(
            string text,
            Color color,
            TextAnchor alignment,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            this.text = text;
            this.color = color;
            this.alignment = alignment;
            this.expandWidth = expandWidth;
            this.width = width;
        }

        public AnalysisMessagePart(
            string label,
            Action action,
            Color color,
            TextAnchor alignment,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH) : this(label, color, alignment, expandWidth, width)
        {
            this.action = action;
        }

        public Action action;
        public bool expandWidth;
        public Color color;
        public float width;
        public string text;
        public TextAnchor alignment;

        public static AnalysisMessagePart Center(
            string text,
            bool isIssue,
            Color issueColor,
            Color goodColor,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            return new(text, isIssue ? issueColor : goodColor, TextAnchor.MiddleCenter, expandWidth, width);
        }

        public static AnalysisMessagePart Center(
            string text,
            Color color,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            return new(text, color, TextAnchor.MiddleCenter, expandWidth, width);
        }

        public static AnalysisMessagePart Left(
            string text,
            bool isIssue,
            Color issueColor,
            Color goodColor,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            return new(text, isIssue ? issueColor : goodColor, TextAnchor.MiddleLeft, expandWidth, width);
        }

        public static AnalysisMessagePart Left(
            string text,
            Color color,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            return new(text, color, TextAnchor.MiddleLeft, expandWidth, width);
        }

        public static AnalysisMessagePart Right(
            string text,
            bool isIssue,
            Color issueColor,
            Color goodColor,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            return new(text, isIssue ? issueColor : goodColor, TextAnchor.MiddleRight, expandWidth, width);
        }

        public static AnalysisMessagePart Right(
            string text,
            Color color,
            bool expandWidth = true,
            float width = DEFAULT_WIDTH)
        {
            return new(text, color, TextAnchor.MiddleRight, expandWidth, width);
        }

        public static AnalysisMessagePart[] Paired(
            string text1,
            string text2,
            bool isIssue,
            Color issueColor,
            Color goodColor,
            bool expandWidthLeft = true,
            float widthLeft = DEFAULT_WIDTH,
            bool expandWidthRight = true,
            float widthRight = DEFAULT_WIDTH)
        {
            return new[]
            {
                new AnalysisMessagePart(
                    text1,
                    isIssue ? issueColor : goodColor,
                    TextAnchor.MiddleRight,
                    expandWidthLeft,
                    widthLeft
                ),
                new AnalysisMessagePart(
                    text2,
                    isIssue ? issueColor : goodColor,
                    TextAnchor.MiddleLeft,
                    expandWidthRight,
                    widthRight
                )
            };
        }

        public static AnalysisMessagePart[] Paired(
            string text1,
            string text2,
            Color color,
            bool expandWidthLeft = true,
            float widthLeft = DEFAULT_WIDTH,
            bool expandWidthRight = true,
            float widthRight = DEFAULT_WIDTH)
        {
            return new[]
            {
                new AnalysisMessagePart(text1, color, TextAnchor.MiddleRight, expandWidthLeft,  widthLeft),
                new AnalysisMessagePart(text2, color, TextAnchor.MiddleLeft,  expandWidthRight, widthRight)
            };
        }

        public static AnalysisMessagePart[] PairedWithButton(
            string text1,
            string text2,
            Color color,
            string buttonLabel,
            Color buttonColor,
            Action action,
            bool expandWidthLeft = true,
            float widthLeft = DEFAULT_WIDTH,
            bool expandWidthRight = true,
            float widthRight = DEFAULT_WIDTH,
            bool expandWidthButton = true,
            float widthButton = DEFAULT_WIDTH)
        {
            return new[]
            {
                new AnalysisMessagePart(text1, color, TextAnchor.MiddleRight, expandWidthLeft, widthLeft),
                new AnalysisMessagePart(
                    text2,
                    color,
                    TextAnchor.MiddleRight,
                    expandWidthRight,
                    widthRight
                ),
                new AnalysisMessagePart(
                    buttonLabel,
                    action,
                    buttonColor,
                    TextAnchor.MiddleCenter,
                    expandWidthButton,
                    widthButton
                )
            };
        }
        
        public static AnalysisMessagePart[] PairedWith2Buttons(
            string text1,
            string text2,
            Color color,
            string buttonLabel1,
            Color buttonColor1,
            Action action1,
            string buttonLabel2,
            Color buttonColor2,
            Action action2,
            bool expandWidthLeft = true,
            float widthLeft = DEFAULT_WIDTH,
            bool expandWidthRight = true,
            float widthRight = DEFAULT_WIDTH,
            bool expandWidthButton1 = false,
            float widthButton1 = DEFAULT_WIDTH,
            bool expandWidthButton2 = false,
            float widthButton2 = DEFAULT_WIDTH)
        {
            return new[]
            {
                new AnalysisMessagePart(text1, color, TextAnchor.MiddleRight, expandWidthLeft, widthLeft),
                new AnalysisMessagePart(
                    text2,
                    color,
                    TextAnchor.MiddleRight,
                    expandWidthRight,
                    widthRight
                ),
                new AnalysisMessagePart(
                    buttonLabel1,
                    action1,
                    buttonColor1,
                    TextAnchor.MiddleCenter,
                    expandWidthButton1,
                    widthButton1
                ),
                new AnalysisMessagePart(
                    buttonLabel2,
                    action2,
                    buttonColor2,
                    TextAnchor.MiddleCenter,
                    expandWidthButton2,
                    widthButton2
                )
            };
        }
        
        public static AnalysisMessagePart[] PairedWith3Buttons(
            string text1,
            string text2,
            Color color,
            string buttonLabel1,
            Color buttonColor1,
            Action action1,
            string buttonLabel2,
            Color buttonColor2,
            Action action2,
            string buttonLabel3,
            Color buttonColor3,
            Action action3,
            bool expandWidthLeft = true,
            float widthLeft = DEFAULT_WIDTH,
            bool expandWidthRight = true,
            float widthRight = DEFAULT_WIDTH,
            bool expandWidthButton1 = false,
            float widthButton1 = DEFAULT_WIDTH,
            bool expandWidthButton2 = false,
            float widthButton2 = DEFAULT_WIDTH,
            bool expandWidthButton3 = false,
            float widthButton3 = DEFAULT_WIDTH)
        {
            return new[]
            {
                new AnalysisMessagePart(text1, color, TextAnchor.MiddleRight, expandWidthLeft, widthLeft),
                new AnalysisMessagePart(
                    text2,
                    color,
                    TextAnchor.MiddleRight,
                    expandWidthRight,
                    widthRight
                ),
                new AnalysisMessagePart(
                    buttonLabel1,
                    action1,
                    buttonColor1,
                    TextAnchor.MiddleCenter,
                    expandWidthButton1,
                    widthButton1
                ),
                new AnalysisMessagePart(
                    buttonLabel2,
                    action2,
                    buttonColor2,
                    TextAnchor.MiddleCenter,
                    expandWidthButton2,
                    widthButton2
                ),
                new AnalysisMessagePart(
                    buttonLabel3,
                    action3,
                    buttonColor3,
                    TextAnchor.MiddleCenter,
                    expandWidthButton3,
                    widthButton3
                )
            };
        }
    }
}
