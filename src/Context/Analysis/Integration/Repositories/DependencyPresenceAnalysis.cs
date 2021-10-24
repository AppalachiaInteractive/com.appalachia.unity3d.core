using System.Collections.Generic;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class DependencyPresenceAnalysis : RepositoryAnalysis
    {
        public DependencyPresenceAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override string ShortName => "Dep. Presence";

        public override RepositoryAnalysisGroup.Types Type =>
            RepositoryAnalysisGroup.Types.DependencyPresence;

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            foreach (var dependency in target.dependencies)
            {
                var isIssue = false;

                if (dependency.IsMissing)
                {
                    isIssue = true;

                    SetColor(group, target, dependency, this);
                }

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.Paired(
                        dependency.name,
                        dependency.version,
                        isIssue,
                        IssueColor,
                        goodColor
                    )
                );
            }
        }

        protected override void CorrectIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            var changed = false;

            var npmPackage = target.npmPackage;

            foreach (var dependency in target.dependencies)
            {
                var refRepo = dependency.repository;

                if (refRepo != null)
                {
                    var packageName = refRepo.PackageName;
                    var packageVersion = refRepo.PackageVersion;

                    if (npmPackage.Dependencies == null)
                    {
                        npmPackage.Dependencies = new Dictionary<string, string>();
                    }

                    if (!npmPackage.Dependencies.ContainsKey(packageName))
                    {
                        changed = true;
                        npmPackage.Dependencies.Add(packageName, packageVersion);
                    }
                }
            }

            if (changed)
            {
                target.SavePackageJson(useTestFiles, reimport);
            }
        }
    }
}
