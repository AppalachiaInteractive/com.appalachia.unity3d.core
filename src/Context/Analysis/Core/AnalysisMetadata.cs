using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Colors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Core
{
    [Serializable]
    public abstract class AnalysisMetadata<TA, TT, TE>
        where TA : AnalysisMetadata<TA, TT, TE>, new()
        where TE : Enum
    {
        protected AnalysisMetadata()
        {
            _allIssues = new List<AnalysisResult<TE>>();

            RegisterAllAnalysis();
            Analyze();
        }

        [NonSerialized] private bool _analyzed;
        [NonSerialized] private bool _analyzing;

        private Dictionary<object, Color> _colors;

        private List<AnalysisResult<TE>> _allIssues;
        private Color[] _allColors;
        private TE[] _allTypes;

        private TT _target;

        public string IDENTIFIERPREFIX => nameof(TT);
        
        public abstract Object Asset { get; }

        public Color IssueColor { get; set; }

        public int IssueDisplayColumns { get; set; } = 3;
        public bool AnyIssues => AllIssues.Any(a => a.HasIssue);

        public IReadOnlyList<AnalysisResult<TE>> AllIssues => _allIssues;
        
        public Color[] AllColors => _allColors;
        public TE[] AllTypes => _allTypes;

        public TT Target => _target;

        protected abstract void ClearAnalysisResults();

        protected abstract void OnAnalyze();

        protected abstract void RegisterAllAnalysis();

        public void Analyze()
        {
            if (_analyzed || _analyzing)
            {
                return;
            }

            SetIssueMetadata();
            
            _analyzing = true;

            if (Target == null)
            {
                return;
            }

            OnAnalyze();

            _analyzed = true;
            _analyzing = false;
        }

        public void CorrectAllIssues(bool useTestFiles, bool reimport)
        {
            for (var index = 0; index < AllIssues.Count; index++)
            {
                var issue = AllIssues[index];

                issue.Correct(useTestFiles, reimport);
            }
        }

        public Color GetColor(TE type)
        {
            foreach (var issue in AllIssues)
            {
                if (Equals(issue.type, type))
                {
                    return issue.color;
                }
            }

            throw new NotSupportedException(type.ToString());
        }

        public Color GetColor(object colorable)
        {
            if (_colors == null)
            {
                _colors = new Dictionary<object, Color>();
            }

            if (_colors.ContainsKey(colorable))
            {
                return _colors[colorable];
            }

            return IssueColor;
        }

        public bool HasIssues(TE type)
        {
            var issue = IssueByType(type);

            return issue.HasIssue;
        }

        public AnalysisResult<TE> IssueByType(TE type)
        {
            foreach (var issue in _allIssues)
            {
                if (issue.type.Equals(type))
                {
                    return issue;
                }
            }

            throw new NotSupportedException(type.ToString());
        }

        public void Reanalyze()
        {
            _analyzed = false;
            _analyzing = false;

            _colors?.Clear();
            _allColors = null;
            _allIssues.Clear();
            
            ClearAnalysisResults();

            Analyze();
        }

        public void SetIssueMetadata()
        {
            _allColors = ColorPalette.Default.bad.Multiple(AllIssues.Count);
            _allTypes = new TE[AllIssues.Count];

            for (var index = 0; index < AllIssues.Count; index++)
            {
                var issue = AllIssues[index];
                issue.color = _allColors[index];

                _allTypes[index] = issue.type;
            }
        }

        public void SetTarget(TT target)
        {
            _target = target;
        }

        protected AnalysisResult<TE> RegisterAnalysis(
            string name,
            TE type,
            Func<bool> issueChecker,
            Action<bool, bool> correction)
        {
            var result = new AnalysisResult<TE>(name, type, issueChecker, correction);

            _allIssues.Add(result);

            return result;
        }

        protected void SetColor(object colorable, AnalysisResult<TE> analysis, bool overwrite = false)
        {
            if (_colors == null)
            {
                _colors = new Dictionary<object, Color>();
            }

            if (_colors.ContainsKey(colorable))
            {
                if (overwrite)
                {
                    _colors[colorable] = analysis.color;
                }
            }
            else
            {
                _colors.Add(colorable, analysis.color);
            }
        }

        public static TA Create(TT target)
        {
            var instance = new TA();
            instance.SetTarget(target);

            return instance;
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            object colorable3,
            object colorable4,
            AnalysisResult<TE> analysis,
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
            AnalysisResult<TE> analysis,
            bool overwrite = false)
        {
            SetColor(colorable1, analysis, overwrite);
            SetColor(colorable2, analysis, overwrite);
            SetColor(colorable3, analysis, overwrite);
        }

        protected void SetColor(
            object colorable1,
            object colorable2,
            AnalysisResult<TE> analysis,
            bool overwrite = false)
        {
            SetColor(colorable1, analysis, overwrite);
            SetColor(colorable2, analysis, overwrite);
        }
    }
}
