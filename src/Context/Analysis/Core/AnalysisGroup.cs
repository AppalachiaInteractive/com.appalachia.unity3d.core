using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Colors;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Core
{
    public abstract class AnalysisGroup<TA, TT, TE>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        #region Nested Types

        public delegate void ReanalyzeHandler();

        #endregion

        public event ReanalyzeHandler OnReanalyzeNecessary;

        protected AnalysisGroup()
        {
            Initialize();
        }

        #region Fields and Autoproperties

        public Color ResultColor { get; set; }

        public int IssueDisplayColumns { get; set; } = 3;

        [NonSerialized] private bool _analyzing;

        [NonSerialized] private bool _initialized;

        private Dictionary<object, Color> _colors;

        private List<AnalysisType<TA, TT, TE>> _allTypes;

        private List<AnalysisTypeMetadata<TE>> _typeMetadata;

        private TT _target;

        #endregion

        public abstract Object Asset { get; }

        public bool AnyAutoCorrectableIssues => AllTypes.Any(a => a.HasAutoCorrectableIssues);
        public bool AnyCorrectableIssues => AllTypes.Any(a => a.HasCorrectableIssues);
        public bool AnyIssues => AllTypes.Any(a => a.HasIssues);

        public IReadOnlyList<AnalysisType<TA, TT, TE>> AllTypes => _allTypes;

        public IReadOnlyList<AnalysisTypeMetadata<TE>> TypeMetadata => _typeMetadata;

        public string IDPREFIX => typeof(TT).Name;

        public TT Target => _target;

        internal Dictionary<object, Color> Colors
        {
            get => _colors;
            set => _colors = value;
        }

        public static TA Create(TT target)
        {
            using (_PRF_Create.Auto())
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

        public void Analyze()
        {
            using (_PRF_Analyze.Auto())
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
                //var sum = 0;
                foreach (var result in AllTypes)
                {
                    result.CheckIssue();
                }

                _analyzing = false;
            }
        }

        public void CorrectAllIssues(bool useTestFiles, bool reimport)
        {
            using (_PRF_CorrectAllIssues.Auto())
            {
                for (var index = 0; index < AllTypes.Count; index++)
                {
                    var issue = AllTypes[index];

                    issue.Correct(useTestFiles, reimport);
                }
            }
        }

        public AnalysisType<TA, TT, TE> GetAnalysisByType(TE type)
        {
            using (_PRF_GetAnalysisByType.Auto())
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
        }

        public Color GetColor(TE type)
        {
            using (_PRF_GetColor.Auto())
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
        }

        public Color GetColor(object colorable)
        {
            using (_PRF_GetColor.Auto())
            {
                if (_colors == null)
                {
                    _colors = new Dictionary<object, Color>();
                }

                if (_colors.TryGetValue(colorable, out var result)) return result;

                return ResultColor;
            }
        }

        public int GetIssueCount(TE type)
        {
            using (_PRF_GetIssueCount.Auto())
            {
                var issue = GetAnalysisByType(type);

                return issue.IssueCount;
            }
        }

        public bool HasIssues(TE type)
        {
            using (_PRF_HasIssues.Auto())
            {
                var issue = GetAnalysisByType(type);

                return issue.HasIssues;
            }
        }

        public void Reanalyze()
        {
            using (_PRF_Reanalyze.Auto())
            {
                if (_target == null)
                {
                    throw new NotSupportedException("Target has not been set!");
                }

                ExecuteReanalyzeNecessary();

                Analyze();
            }
        }

        public void SetAnalysisMetadata()
        {
            using (_PRF_SetAnalysisMetadata.Auto())
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
        }

        public void SetAnalysisTarget(TT target)
        {
            using (_PRF_SetAnalysisTarget.Auto())
            {
                if (target == null)
                {
                    throw new ArgumentNullException(nameof(target));
                }

                _target = target;
            }
        }

        protected abstract void OnAnalyze();

        protected abstract void RegisterAllAnalysis();

        protected virtual void ExecuteReanalyzeNecessary()
        {
            OnReanalyzeNecessary?.Invoke();
        }

        protected void ClearAnalysisResults()
        {
            using (_PRF_ClearAnalysisResults.Auto())
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
                    analysisType.ClearResults((TA)this, _target);
                }

                _allTypes = null;
            }
        }

        protected T RegisterAnalysisType<T>(T analysis)
            where T : AnalysisType<TA, TT, TE>
        {
            using (_PRF_RegisterAnalysisType.Auto())
            {
                _allTypes.Add(analysis);

                return analysis;
            }
        }

        private void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                if (_initialized)
                {
                    return;
                }

                _initialized = true;

                _allTypes = new List<AnalysisType<TA, TT, TE>>();

                RegisterAllAnalysis();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AnalysisGroup<TA, TT, TE>) + ".";

        private static readonly ProfilerMarker _PRF_Analyze = new ProfilerMarker(_PRF_PFX + nameof(Analyze));

        private static readonly ProfilerMarker _PRF_CorrectAllIssues =
            new ProfilerMarker(_PRF_PFX + nameof(CorrectAllIssues));

        private static readonly ProfilerMarker _PRF_GetAnalysisByType =
            new ProfilerMarker(_PRF_PFX + nameof(GetAnalysisByType));

        private static readonly ProfilerMarker
            _PRF_GetColor = new ProfilerMarker(_PRF_PFX + nameof(GetColor));

        private static readonly ProfilerMarker _PRF_GetIssueCount =
            new ProfilerMarker(_PRF_PFX + nameof(GetIssueCount));

        private static readonly ProfilerMarker _PRF_HasIssues =
            new ProfilerMarker(_PRF_PFX + nameof(HasIssues));

        private static readonly ProfilerMarker _PRF_Reanalyze =
            new ProfilerMarker(_PRF_PFX + nameof(Reanalyze));

        private static readonly ProfilerMarker _PRF_SetAnalysisMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(SetAnalysisMetadata));

        private static readonly ProfilerMarker _PRF_SetAnalysisTarget =
            new ProfilerMarker(_PRF_PFX + nameof(SetAnalysisTarget));

        private static readonly ProfilerMarker _PRF_ClearAnalysisResults =
            new ProfilerMarker(_PRF_PFX + nameof(ClearAnalysisResults));

        private static readonly ProfilerMarker _PRF_RegisterAnalysisType =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterAnalysisType));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Create = new ProfilerMarker(_PRF_PFX + nameof(Create));

        #endregion
    }
}
