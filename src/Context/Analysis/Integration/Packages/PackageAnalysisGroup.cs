using System;
using Appalachia.CI.Integration.Packages;
using Appalachia.Core.Context.Analysis.Core;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Integration.Packages
{
    [Serializable]
    public class
        PackageAnalysisGroup : AnalysisGroup<PackageAnalysisGroup, PackageMetadata,
            PackageAnalysisGroup.Types>
    {
        public enum Types
        {
            All = 00000,
            PackageVersions = 00010
        }

        public PackageVersionsAnalysis PackageVersions;

        public override Object Asset => Target.repository?.PackageJsonAsset;

        protected override void OnAnalyze()
        {
        }

        protected override void RegisterAllAnalysis()
        {
            PackageVersions = RegisterAnalysisType(new PackageVersionsAnalysis(this));
        }
    }
}
