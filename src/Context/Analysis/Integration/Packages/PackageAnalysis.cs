using Appalachia.CI.Integration.Packages;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Packages
{
    public abstract class PackageAnalysis : AnalysisType<PackageAnalysisGroup, PackageMetadata,
        PackageAnalysisGroup.Types>
    {
        protected PackageAnalysis(PackageAnalysisGroup group) : base(group)
        {
        }
    }
}
