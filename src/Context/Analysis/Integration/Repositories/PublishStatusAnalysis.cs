using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Repositories;
using Appalachia.CI.Integration.Repositories.Publishing;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class PublishStatusAnalysis : RepositoryAnalysis
    {
        public PublishStatusAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => false;
        public override string ShortName => "Publish Status";

        public override RepositoryAnalysisGroup.Types Type => RepositoryAnalysisGroup.Types.PublishStatus;

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            var statusIssue = target.publishStatus.currentStatus == PublishStatus.Failed;
            var versionIssue = (target.PublishedVersion != null) &&
                               (target.PublishedVersion != target.DistributableVersion);
            var inProgress = target.publishStatus.currentStatus == PublishStatus.InProgress;

            var isIssue = !inProgress && (statusIssue || versionIssue);

            var versionMessage = versionIssue
                ? $"[remote:{target.PublishedVersion}] != [local:{target.DistributableVersion}]"
                : target.DistributableVersion;

            var statusMessage = target.publishStatus.currentStatus.ToString();

            if (statusIssue)
            {
                statusMessage += " | " + target.publishStatus.currentMessage;
            }

            if (isIssue)
            {
                SetColor(group, target, this);
            }

            var fullMessage = $"{statusMessage} {versionMessage}";

            messages.Add(isIssue, AnalysisMessagePart.Center(fullMessage, isIssue, IssueColor, goodColor));
        }

        protected override void CorrectIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            var npmPackage = target.npmPackage;

            var deps = target.dependencies.ToArray();

            foreach (var dependency in deps)
            {
                if (!dependency.IsValid)
                {
                    target.dependencies.Remove(dependency);
                    npmPackage.Dependencies.Remove(dependency.name);
                }
            }

            target.SavePackageJson(useTestFiles, reimport);
        }
    }
}
