using System.Collections.Generic;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class DependencyVersionsAnalysis : RepositoryAnalysis
    {
        public DependencyVersionsAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override string ShortName => "Dep. Versions";

        public override RepositoryAnalysisGroup.Types Type =>
            RepositoryAnalysisGroup.Types.DependencyVersions;

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            foreach (var dependency in target.dependencies)
            {
                var isIssue = false;
                
                if (dependency.IsOutOfDate)
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
                
                var packageName = refRepo.PackageName;
                var packageVersion = refRepo.PackageVersion;
                
                npmPackage.Dependencies[packageName] = packageVersion;
            }

            target.SavePackageJson(useTestFiles, reimport);
        }
    }
}
