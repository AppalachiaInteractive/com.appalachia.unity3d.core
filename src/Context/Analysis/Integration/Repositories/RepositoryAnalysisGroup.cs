using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    [Serializable]
    public class RepositoryAnalysisGroup : AnalysisGroup<RepositoryAnalysisGroup, RepositoryMetadata,
        RepositoryAnalysisGroup.Types>
    {
        public enum Types
        {
            All = 00000,
            DependencyPresence = 00030,
            DependencyVersions = 00040,
            DependencyValidity = 00120
        }

        public DependencyPresenceAnalysis DependencyPresence;
        public DependencyValidityAnalysis DependencyValidity;
        public DependencyVersionsAnalysis DependencyVersions;

        internal Dictionary<string, RepositoryDependency> uniqueDependencies;

        public override Object Asset => Target.PackageJsonAsset;

        protected override void OnAnalyze()
        {
            uniqueDependencies = new Dictionary<string, RepositoryDependency>();
            
            foreach (var dependency in Target.dependencies)
            {
                uniqueDependencies.Add(dependency.name, dependency);

                dependency.repository = RepositoryMetadata.FindByName(dependency.name);
            }
        }

        protected override void RegisterAllAnalysis()
        {
            DependencyPresence = RegisterAnalysisType(new DependencyPresenceAnalysis(this));

            DependencyValidity = RegisterAnalysisType(new DependencyValidityAnalysis(this));

            DependencyVersions = RegisterAnalysisType(new DependencyVersionsAnalysis(this));
        }
    }
}
