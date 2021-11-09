using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Context.Analysis.Core
{
    [Serializable]
    public abstract class AnalysisType<TA, TT, TE>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(AnalysisType<TA, TT, TE>) + ".";

        private static readonly ProfilerMarker _PRF_DisplayName =
            new ProfilerMarker(_PRF_PFX + nameof(DisplayName));

        private static readonly ProfilerMarker _PRF_ClearResults =
            new ProfilerMarker(_PRF_PFX + nameof(ClearResults));

        private static readonly ProfilerMarker _PRF_Correct = new ProfilerMarker(_PRF_PFX + nameof(Correct));

        private static readonly ProfilerMarker _PRF_CheckIssue =
            new ProfilerMarker(_PRF_PFX + nameof(CheckIssue));

        private static readonly ProfilerMarker
            _PRF_SetColor = new ProfilerMarker(_PRF_PFX + nameof(SetColor));

        private static readonly ProfilerMarker _PRF_HasAutoCorrectableIssues =
            new ProfilerMarker(_PRF_PFX + nameof(HasAutoCorrectableIssues));

        private static readonly ProfilerMarker _PRF_HasIssues =
            new ProfilerMarker(_PRF_PFX + nameof(HasIssues));

        #endregion

        protected AnalysisType(TA group)
        {
            _group = group;
        }

        protected Color _color;

        private Color _issueColor;

        private List<AnalysisMessage> _messages;

        private string _longName;

        private TA _group;

        protected TA Group => _group;

        public virtual bool IsCorrectable => true;

        public abstract bool IsAutoCorrectable();

        public abstract string ShortName { get; }

        public abstract TE Type { get; }

        public bool HasAutoCorrectableIssues
        {
            get
            {
                using (_PRF_HasAutoCorrectableIssues.Auto())
                {
                    CheckIssue();

                    return IsAutoCorrectable() && (_messages.Count(m => m.isIssue) > 0);
                }
            }
        }

        public bool HasCorrectableIssues
        {
            get
            {
                using (_PRF_HasAutoCorrectableIssues.Auto())
                {
                    CheckIssue();

                    return IsCorrectable && (_messages.Count(m => m.isIssue) > 0);
                }
            }
        }

        public bool HasIssues
        {
            get
            {
                using (_PRF_HasIssues.Auto())
                {
                    CheckIssue();

                    return _messages.Count(m => m.isIssue) > 0;
                }
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
                using (_PRF_DisplayName.Auto())
                {
                    if (_longName == null)
                    {
                        _longName = Type.ToDisplayName();
                    }

                    return _longName;
                }
            }
        }

        protected Color disabledColor => ColorPalette.Default.disabled.Middle;
        protected Color goodColor => ColorPalette.Default.good.ThreeQuarters;

        protected abstract void AnalyzeIssue(TA group, TT target, List<AnalysisMessage> messages);

        protected abstract void CorrectIssue(TA group, TT target, bool useTestFiles, bool reimport);

        public virtual void ClearResults(TA group, TT target)
        {
            using (_PRF_ClearResults.Auto())
            {
                _messages?.Clear();
            }
        }

        public void Correct(bool useTestFiles, bool reimport)
        {
            using (_PRF_Correct.Auto())
            {
                if (HasIssues)
                {
                    foreach (var message in _messages)
                    {
                        if (message.isIssue)
                        {
                            AppaLog.Trace($"Fixing issue: {message.PrintMessage()}");
                        }
                    }
                    
                    CorrectIssue(_group, _group.Target, useTestFiles, reimport);
                }
            }
        }

        public void SetColor(Color c)
        {
            using (_PRF_SetColor.Auto())
            {
                _issueColor = c;
            }
        }

        internal void CheckIssue()
        {
            using (_PRF_CheckIssue.Auto())
            {
                if (_messages == null)
                {
                    _messages = new List<AnalysisMessage>();

                    AnalyzeIssue(_group, _group.Target, _messages);

                    _messages.Sort((a, b) => -1 * a.isIssue.CompareTo(b.isIssue));
                }
            }
        }

        protected void SetColor(object colorable, AnalysisType<TA, TT, TE> analysis, bool overwrite = false)
        {
            using (_PRF_SetColor.Auto())
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
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            object colorable3,
            object colorable4,
            AnalysisType<TA, TT, TE> analysis,
            bool overwrite = false)
        {
            using (_PRF_SetColor.Auto())
            {
                SetColor(colorable1, analysis, overwrite);
                SetColor(colorable2, analysis, overwrite);
                SetColor(colorable3, analysis, overwrite);
                SetColor(colorable4, analysis, overwrite);
            }
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            object colorable3,
            AnalysisType<TA, TT, TE> analysis,
            bool overwrite = false)
        {
            using (_PRF_SetColor.Auto())
            {
                SetColor(colorable1, analysis, overwrite);
                SetColor(colorable2, analysis, overwrite);
                SetColor(colorable3, analysis, overwrite);
            }
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            AnalysisType<TA, TT, TE> analysis,
            bool overwrite = false)
        {
            using (_PRF_SetColor.Auto())
            {
                SetColor(colorable1, analysis, overwrite);
                SetColor(colorable2, analysis, overwrite);
            }
        }
    }
}
