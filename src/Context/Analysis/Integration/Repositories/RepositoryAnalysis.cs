using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public abstract class RepositoryAnalysis : AnalysisType<RepositoryAnalysisGroup, RepositoryMetadata,
        RepositoryAnalysisGroup.Types>
    {
        protected RepositoryAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }
    }
}
