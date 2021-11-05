using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class DependencyPresenceAnalysis : RepositoryAnalysis
    {
        public DependencyPresenceAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable() => true;

        public override string ShortName => "Dep. Presence";

        public override RepositoryAnalysisGroup.Types Type =>
            RepositoryAnalysisGroup.Types.DependencyPresence;

        public override void ClearResults(RepositoryAnalysisGroup group, RepositoryMetadata target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            
            target.missingDependencies?.Clear();
        }

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            target.PopulateDependencies();

            var existingDependencies = target.dependencies.Select(d => d.name).ToHashSet();

            foreach (var assembly in target.assemblies)
            {
                assembly.SetReferences();
            }

            foreach (var assembly in target.assemblies)
            {
                foreach (var reference in assembly.references)
                {
                    if (reference.assembly == null)
                    {
                        continue;
                    }

                    var referenceRepo = reference.assembly.repository;

                    if ((referenceRepo == null) || (target == referenceRepo))
                    {
                        continue;
                    }

                    if (existingDependencies.Contains(referenceRepo.Name))
                    {
                        continue;
                    }

                    var newDependency = new RepositoryDependency(
                        referenceRepo.Name,
                        referenceRepo.PackageVersion
                    );

                    newDependency.SetMissing();

                    target.PopulateDependency(newDependency);

                    target.missingDependencies.Add(newDependency);
                    existingDependencies.Add(newDependency.Name);
                }
            }

            foreach (var dependency in target.dependencies.Concat(target.missingDependencies))
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
            var npmPackage = target.npmPackage;

            npmPackage.Dependencies ??= new SortedDictionary<string, string>();

            foreach (var dependency in target.dependencies.Concat(target.missingDependencies))
            {
                var refRepo = dependency.repository;

                if (refRepo == null)
                {
                    continue;
                }

                var packageName = refRepo.PackageName;
                var packageVersion = refRepo.PackageVersion;

                if (!npmPackage.Dependencies.ContainsKey(packageName))
                {
                    npmPackage.Dependencies.Add(packageName, packageVersion);
                }
            }

            target.missingDependencies.Clear();

            target.SavePackageJson(useTestFiles, reimport);
        }
    }
}
