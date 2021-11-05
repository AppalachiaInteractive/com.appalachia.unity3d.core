using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;

namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisAggregate<TA, TT, TE>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(AnalysisAggregate<TA, TT, TE>) + ".";

        private static readonly ProfilerMarker _PRF_AnyIssues =
            new ProfilerMarker(_PRF_PFX + nameof(AnyIssues));

        private static readonly ProfilerMarker _PRF_Add = new ProfilerMarker(_PRF_PFX + nameof(Add));

        private static readonly ProfilerMarker _PRF_GetIssueCount =
            new ProfilerMarker(_PRF_PFX + nameof(GetIssueCount));

        private static readonly ProfilerMarker _PRF_GetSubissueCount =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubissueCount));

        private static readonly ProfilerMarker _PRF_HasIssues =
            new ProfilerMarker(_PRF_PFX + nameof(HasIssues));

        private static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX + nameof(Reset));

        private static readonly ProfilerMarker _PRF_AnyCorrectableIssues =
            new ProfilerMarker(_PRF_PFX + nameof(AnyCorrectableIssues));

        private static readonly ProfilerMarker _PRF_AnyAutoCorrectableIssues =
            new ProfilerMarker(_PRF_PFX + nameof(AnyAutoCorrectableIssues));

        #endregion

        public AnalysisAggregate()
        {
            _issueCounts = new Dictionary<TE, int>();
            _subissueCounts = new Dictionary<TE, int>();
            _correctableIssueCounts = new Dictionary<TE, int>();
            _autoCorrectableIssueCounts = new Dictionary<TE, int>();
        }

        private readonly Dictionary<TE, int> _autoCorrectableIssueCounts;
        private readonly Dictionary<TE, int> _correctableIssueCounts;

        private readonly Dictionary<TE, int> _issueCounts;
        private readonly Dictionary<TE, int> _subissueCounts;

        public bool AnyAutoCorrectableIssues
        {
            get
            {
                using (_PRF_AnyAutoCorrectableIssues.Auto())
                {
                    return _autoCorrectableIssueCounts.Values.Any(v => v > 0);
                }
            }
        }

        public bool AnyCorrectableIssues
        {
            get
            {
                using (_PRF_AnyCorrectableIssues.Auto())
                {
                    return _correctableIssueCounts.Values.Any(v => v > 0);
                }
            }
        }

        public bool AnyIssues
        {
            get
            {
                using (_PRF_AnyIssues.Auto())
                {
                    return _issueCounts.Values.Any(v => v > 0);
                }
            }
        }

        public bool HasAutoCorrectableIssues(TE type)
        {
            using (_PRF_HasIssues.Auto())
            {
                return GetAutoCorrectableIssueCount(type) > 0;
            }
        }

        public bool HasCorrectableIssues(TE type)
        {
            using (_PRF_HasIssues.Auto())
            {
                return GetCorrectableIssueCount(type) > 0;
            }
        }

        public bool HasIssues(TE type)
        {
            using (_PRF_HasIssues.Auto())
            {
                return GetIssueCount(type) > 0;
            }
        }

        public int GetAutoCorrectableIssueCount(TE type)
        {
            using (_PRF_GetIssueCount.Auto())
            {
                if (!_autoCorrectableIssueCounts.ContainsKey(type))
                {
                    _autoCorrectableIssueCounts.Add(type, 0);
                }

                return _autoCorrectableIssueCounts[type];
            }
        }

        public int GetCorrectableIssueCount(TE type)
        {
            using (_PRF_GetIssueCount.Auto())
            {
                if (!_correctableIssueCounts.ContainsKey(type))
                {
                    _correctableIssueCounts.Add(type, 0);
                }

                return _correctableIssueCounts[type];
            }
        }

        public int GetIssueCount(TE type)
        {
            using (_PRF_GetIssueCount.Auto())
            {
                if (!_issueCounts.ContainsKey(type))
                {
                    _issueCounts.Add(type, 0);
                }

                return _issueCounts[type];
            }
        }

        public int GetSubissueCount(TE type)
        {
            using (_PRF_GetSubissueCount.Auto())
            {
                if (!_subissueCounts.ContainsKey(type))
                {
                    _subissueCounts.Add(type, 0);
                }

                return _subissueCounts[type];
            }
        }

        public void Add(IEnumerable<AnalysisType<TA, TT, TE>> results)
        {
            using (_PRF_Add.Auto())
            {
                if (results == null)
                {
                    return;
                }

                foreach (var issue in results)
                {
                    if (!_issueCounts.ContainsKey(issue.Type))
                    {
                        _issueCounts.Add(issue.Type, 0);
                    }

                    if (!_subissueCounts.ContainsKey(issue.Type))
                    {
                        _subissueCounts.Add(issue.Type, 0);
                    }

                    if (!_correctableIssueCounts.ContainsKey(issue.Type))
                    {
                        _correctableIssueCounts.Add(issue.Type, 0);
                    }

                    if (!_autoCorrectableIssueCounts.ContainsKey(issue.Type))
                    {
                        _autoCorrectableIssueCounts.Add(issue.Type, 0);
                    }

                    if (!issue.HasIssues)
                    {
                        continue;
                    }

                    _issueCounts[issue.Type] += 1;
                    _subissueCounts[issue.Type] += issue.IssueCount;

                    if (issue.IsCorrectable)
                    {
                        _correctableIssueCounts[issue.Type] += 1;
                    }

                    if (issue.IsAutoCorrectable())
                    {
                        _autoCorrectableIssueCounts[issue.Type] += 1;
                    }
                }
            }
        }

        public void AddAll(IEnumerable<IEnumerable<AnalysisType<TA, TT, TE>>> allAnalysisResults)
        {
            foreach (var analysisResults in allAnalysisResults)
            {
                Add(analysisResults);
            }
        }

        public void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                _issueCounts?.Clear();
                _subissueCounts?.Clear();
                _correctableIssueCounts?.Clear();
                _autoCorrectableIssueCounts?.Clear();
            }
        }
    }
}
