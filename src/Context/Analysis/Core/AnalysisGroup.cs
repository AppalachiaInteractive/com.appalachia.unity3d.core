using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Colors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Core
{
    [Serializable]
    public abstract class AnalysisGroup<TA, TT, TE>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        protected AnalysisGroup()
        {
            Initialize();
        }

        [NonSerialized] private bool _analyzing;

        [NonSerialized] private bool _initialized;

        private Dictionary<object, Color> _colors;

        private List<AnalysisType<TA, TT, TE>> _allTypes;

        private List<AnalysisTypeMetadata<TE>> _typeMetadata;

        private TT _target;

        public Color ResultColor { get; set; }

        public int IssueDisplayColumns { get; set; } = 3;

        public abstract Object Asset { get; }
        public bool AnyIssues => AllTypes.Any(a => a.HasIssues);
        
        public bool AnyAutoCorrectableIssues => AllTypes.Any(a => a.HasAutoCorrectableIssues);

        public IReadOnlyList<AnalysisType<TA, TT, TE>> AllTypes => _allTypes;

        public IReadOnlyList<AnalysisTypeMetadata<TE>> TypeMetadata => _typeMetadata;

        public string IDPREFIX => typeof(TT).Name;

        public TT Target => _target;

        internal Dictionary<object, Color> Colors
        {
            get => _colors;
            set => _colors = value;
        }

        protected abstract void OnAnalyze();

        protected abstract void RegisterAllAnalysis();

        public void Analyze()
        {
            if (_target == null)
            {
                throw new NotSupportedException("Target has not been set!");
            }
            
            ClearAnalysisResults();
            Initialize();
            
            if (_analyzing)
            {
                return;
            }

            SetAnalysisMetadata();

            _analyzing = true;

            if (Target == null)
            {
                return;
            }

            OnAnalyze();

            // ReSharper disable once NotAccessedVariable
            var sum = 0;
            foreach (var result in AllTypes)
            {
                result.CheckIssue();
            }

            _analyzing = false;
        }

        public void CorrectAllIssues(bool useTestFiles, bool reimport)
        {
            for (var index = 0; index < AllTypes.Count; index++)
            {
                var issue = AllTypes[index];

                issue.Correct(useTestFiles, reimport);
            }
        }

        public AnalysisType<TA, TT, TE> GetAnalysisByType(TE type)
        {
            foreach (var issue in _allTypes)
            {
                if (issue.Type.Equals(type))
                {
                    return issue;
                }
            }

            throw new NotSupportedException(type.ToString());
        }

        public Color GetColor(TE type)
        {
            foreach (var issue in AllTypes)
            {
                if (Equals(issue.Type, type))
                {
                    return issue.IssueColor;
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

            return ResultColor;
        }

        public int GetIssueCount(TE type)
        {
            var issue = GetAnalysisByType(type);

            return issue.IssueCount;
        }

        public bool HasIssues(TE type)
        {
            var issue = GetAnalysisByType(type);

            return issue.HasIssues;
        }

        public void Reanalyze()
        {
            if (_target == null)
            {
                throw new NotSupportedException("Target has not been set!");
            }

            Analyze();
        }

        public void SetAnalysisMetadata()
        {
            var metadataCount = AllTypes.Count;

            var colors = ColorPalette.Default.bad.Multiple(metadataCount);

            _typeMetadata = new List<AnalysisTypeMetadata<TE>>();

            for (var index = 0; index < metadataCount; index++)
            {
                var issue = AllTypes[index];

                issue.SetColor(colors[index]);

                var newMetadata = new AnalysisTypeMetadata<TE>
                {
                    color = colors[index], type = issue.Type, name = issue.DisplayName
                };

                _typeMetadata.Add(newMetadata);
            }
        }

        public void SetAnalysisTarget(TT target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            
            _target = target;
        }

        protected void ClearAnalysisResults()
        {
            if (_target == null)
            {
                throw new NotSupportedException("Target has not been set!");
            }

            _initialized = false;
            _analyzing = false;

            _colors?.Clear();
            _typeMetadata = null;

            foreach (var analysisType in _allTypes)
            {
                analysisType.ClearResults((TA) this, _target);
            }

            _allTypes = null;
        }

        protected T RegisterAnalysisType<T>(T analysis)
            where T : AnalysisType<TA, TT, TE>
        {
            _allTypes.Add(analysis);

            return analysis;
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            _allTypes = new List<AnalysisType<TA, TT, TE>>();

            RegisterAllAnalysis();
        }

        public static TA Create(TT target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            
            var instance = new TA();
            instance.SetAnalysisTarget(target);

            instance.Initialize();

            return instance;
        }
    }
}
