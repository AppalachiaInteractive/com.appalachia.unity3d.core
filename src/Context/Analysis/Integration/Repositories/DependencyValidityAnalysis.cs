using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class DependencyValidityAnalysis : RepositoryAnalysis
    {
        public DependencyValidityAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable() => true;
        public override string ShortName => "Dep. Validity";

        public override RepositoryAnalysisGroup.Types Type =>
            RepositoryAnalysisGroup.Types.DependencyValidity;

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            foreach (var dependency in target.dependencies)
            {
                var isIssue = false;

                if (!dependency.IsValid)
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
