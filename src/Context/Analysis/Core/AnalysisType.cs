using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Extensions;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Enums;
using UnityEngine;

namespace Appalachia.Core.Context.Analysis.Core
{
    [Serializable]
    public abstract class AnalysisType<TA, TT, TE>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        protected AnalysisType(TA group)
        {
            _group = group;
        }

        protected Color _color;

        private Color _issueColor;

        private List<AnalysisMessage> _messages;

        private string _longName;

        private TA _group;

        public abstract string ShortName { get; }

        public abstract TE Type { get; }

        public bool HasIssues
        {
            get
            {
                CheckIssue();

                return _messages.Count(m => m.isIssue) > 0;
            }
        }

        public Color Color => HasIssues ? IssueColor : disabledColor;
        public Color IssueColor => _issueColor;

        public int IssueCount
        {
            get
            {
                CheckIssue();

                return _messages.Count(m => m.isIssue);
            }
        }

        public IReadOnlyList<AnalysisMessage> Messages => _messages;

        public string DisplayName
        {
            get
            {
                if (_longName == null)
                {
                    _longName = Type.ToDisplayName();
                }

                return _longName;
            }
        }

        protected Color disabledColor => ColorPalette.Default.disabled.Middle;
        protected Color goodColor => ColorPalette.Default.good.ThreeQuarters;

        protected abstract void AnalyzeIssue(TA group, TT target, List<AnalysisMessage> messages);

        protected abstract void CorrectIssue(TA group, TT target, bool useTestFiles, bool reimport);

        public void ClearResults()
        {
            _messages?.Clear();
        }

        public void Correct(bool useTestFiles, bool reimport)
        {
            if (HasIssues)
            {
                CorrectIssue(_group, _group.Target, useTestFiles, reimport);
            }
        }

        public void SetColor(Color c)
        {
            _issueColor = c;
        }

        internal void CheckIssue()
        {
            if (_messages == null)
            {
                _messages = new List<AnalysisMessage>();

                AnalyzeIssue(_group, _group.Target, _messages);

                _messages.Sort((a, b) => -1 * a.isIssue.CompareTo(b.isIssue));
            }
        }

        protected void SetColor(object colorable, AnalysisType<TA, TT, TE> analysis, bool overwrite = false)
        {
            _color = IssueColor;

            if (_group.Colors == null)
            {
                _group.Colors = new Dictionary<object, Color>();
            }

            if (_group.Colors.ContainsKey(colorable))
            {
                if (overwrite)
                {
                    _group.Colors[colorable] = analysis.IssueColor;
                }
            }
            else
            {
                _group.Colors.Add(colorable, analysis.IssueColor);
            }
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            object colorable3,
            object colorable4,
            AnalysisType<TA, TT, TE> analysis,
            bool overwrite = false)
        {
            SetColor(colorable1, analysis, overwrite);
            SetColor(colorable2, analysis, overwrite);
            SetColor(colorable3, analysis, overwrite);
            SetColor(colorable4, analysis, overwrite);
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            object colorable3,
            AnalysisType<TA, TT, TE> analysis,
            bool overwrite = false)
        {
            SetColor(colorable1, analysis, overwrite);
            SetColor(colorable2, analysis, overwrite);
            SetColor(colorable3, analysis, overwrite);
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            AnalysisType<TA, TT, TE> analysis,
            bool overwrite = false)
        {
            SetColor(colorable1, analysis, overwrite);
            SetColor(colorable2, analysis, overwrite);
        }
    }
}
