using System;
using System.Collections.Generic;
using System.Linq;

namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisAggregate<TA, TT, TE>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        public AnalysisAggregate()
        {
            _issueCounts = new Dictionary<TE, int>();
            _subissueCounts = new Dictionary<TE, int>();
        }

        private Dictionary<TE, int> _issueCounts;
        private Dictionary<TE, int> _subissueCounts;

        public bool AnyIssues => _issueCounts.Values.Any(v => v > 0);

        public void Add(IEnumerable<AnalysisType<TA, TT, TE>> results)
        {
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

                if (!issue.HasIssues)
                {
                    continue;
                }

                _issueCounts[issue.Type] += 1;
                _subissueCounts[issue.Type] += issue.IssueCount;
            }
        }

        public int GetIssueCount(TE type)
        {
            if (!_issueCounts.ContainsKey(type))
            {
                _issueCounts.Add(type, 0);
            }

            return _issueCounts[type];
        }

        public int GetSubissueCount(TE type)
        {
            if (!_subissueCounts.ContainsKey(type))
            {
                _subissueCounts.Add(type, 0);
            }

            return _subissueCounts[type];
        }

        public bool HasIssues(TE type)
        {
            return GetIssueCount(type) > 0;
        }

        public void Reset()
        {
            _issueCounts?.Clear();
            _subissueCounts?.Clear();
        }
    }
}
