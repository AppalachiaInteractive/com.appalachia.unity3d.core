using System;
using System.Collections.Generic;
using System.Linq;

namespace Appalachia.Core.Context.Analysis.Core
{
    public class AnalysisAggregate<T>
        where T : Enum
    {
        public AnalysisAggregate()
        {
            _counts = new Dictionary<T, int>();
        }

        private Dictionary<T, int> _counts;

        public bool AnyIssues => _counts.Values.Any(v => v > 0);

        public int this[T type]
        {
            get
            {
                if (!_counts.ContainsKey(type))
                {
                    _counts.Add(type, 0);
                }

                return _counts[type];
            }
        }

        public bool HasIssues(T type)
        {
            return this[type] > 0;
        }

        public void Add(IEnumerable<AnalysisResult<T>> results)
        {
            foreach (var issue in results)
            {
                if (!_counts.ContainsKey(issue.type))
                {
                    _counts.Add(issue.type, 0);
                }

                if (!issue.HasIssue)
                {
                    continue;
                }

                _counts[issue.type] += 1;
            }
        }

        public void Reset()
        {
            _counts?.Clear();
        }
    }
}
