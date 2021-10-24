using System;
using System.Threading.Tasks;
using Appalachia.CI.Integration.Packages;
using Appalachia.Core.Context.Analysis.Core;
using UnityEditor.PackageManager;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Integration
{
    [Serializable]
    public class PackageAnalysis : AnalysisMetadata<PackageAnalysis, PackageMetadata, PackageAnalysis.Types>
    {
        public enum Types
        {
            All = 00000,
            PackageVersions = 00010,
        
        }
        
        public AnalysisResult<Types> PackageVersions;

        protected override void OnAnalyze()
        {
            AnalyzePackages(
                this,
                Target,
                PackageVersions
            );
        }

        protected override void RegisterAllAnalysis()
        {
            PackageVersions = RegisterAnalysis(
                "Package Versions",
                Types.PackageVersions,
                CheckPackageVersions,
                FixPackageVersions
            );
        }

        public override Object Asset => Target.repository?.PackageJsonAsset;

        protected override void ClearAnalysisResults()
        {
        }

        private bool CheckPackageVersions()
        {
            return Target.packageInfo.version != Target.packageInfo.versions.latest;
        }

        private void FixPackageVersions(bool useTestFiles, bool reimport)
        {
            var addRequest = Client.Add($"{Target.packageInfo.name}@{Target.packageInfo.versions.latest}");

            while (!addRequest.IsCompleted)
            {
                Task.Delay(1);
            }
        }

        private void AnalyzePackages(
            PackageAnalysis analysis,
            PackageMetadata package,
            AnalysisResult<Types> packageVersions)
        {
            if (analysis.CheckPackageVersions())
            {
                SetColor(analysis, packageVersions);
            }
        }
    }
}
