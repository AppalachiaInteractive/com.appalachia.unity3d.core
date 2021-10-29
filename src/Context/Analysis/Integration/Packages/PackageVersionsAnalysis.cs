using System.Collections.Generic;
using System.Threading.Tasks;
using Appalachia.CI.Integration.Packages;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Packages
{
    public sealed class PackageVersionsAnalysis : PackageAnalysis
    {
        public PackageVersionsAnalysis(PackageAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => true;
        public override string ShortName => "Package Versions";

        public override PackageAnalysisGroup.Types Type => PackageAnalysisGroup.Types.PackageVersions;

        protected override void AnalyzeIssue(
            PackageAnalysisGroup group,
            PackageMetadata target,
            List<AnalysisMessage> messages)
        {
            var label = $"{target.Name}: {target.Version}";
            var text = $"Latest: {target.LatestVersion}";

            messages.Add(
                target.IsOutOfDate,
                AnalysisMessagePart.Paired(label, text, target.IsOutOfDate, IssueColor, goodColor)
            );
        }

        protected override void CorrectIssue(
            PackageAnalysisGroup group,
            PackageMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            var addRequest = UnityEditor.PackageManager.Client.Add(
                $"{target.packageInfo.name}@{target.packageInfo.versions.latest}"
            );

            while (!addRequest.IsCompleted)
            {
                Task.Delay(1);
            }
        }
    }
}
