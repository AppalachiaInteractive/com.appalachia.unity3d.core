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
            DependencyPresence = 0b00000001,
            DependencyVersions = 0b00000010,
            DependencyValidity = 0b00000100,
            PublishStatus =      0b00001000,
            DistributableSize =  0b00010000,
            FileSize =           0b00100000
        }

        public DependencyPresenceAnalysis DependencyPresence;
        public DependencyValidityAnalysis DependencyValidity;
        public DependencyVersionsAnalysis DependencyVersions;
        public PublishStatusAnalysis PublishStatus;
        public DistributableSizeAnalysis DistributableSize;
        public FileSizeAnalysis FileSize;

        internal Dictionary<string, RepositoryDependency> uniqueDependencies;

        public override Object Asset => Target.PackageJsonAsset;

        protected override void OnAnalyze()
        {
            Target.OnReanalyzeNecessary -= Reanalyze;
            Target.OnReanalyzeNecessary += Reanalyze;
                
            uniqueDependencies = new Dictionary<string, RepositoryDependency>();

            foreach (var dependency in Target.dependencies)
            {
                if (!uniqueDependencies.ContainsKey(dependency.name))
                {
                    uniqueDependencies.Add(dependency.name, dependency);
                }

                if (dependency.repository == null)
                {
                    dependency.repository = RepositoryMetadata.FindByName(dependency.name);
                }
            }
        }

        protected override void RegisterAllAnalysis()
        {
            DependencyPresence = RegisterAnalysisType(new DependencyPresenceAnalysis(this));

            DependencyValidity = RegisterAnalysisType(new DependencyValidityAnalysis(this));

            DependencyVersions = RegisterAnalysisType(new DependencyVersionsAnalysis(this));
            
            PublishStatus = RegisterAnalysisType(new PublishStatusAnalysis(this));

            DistributableSize = RegisterAnalysisType(new DistributableSizeAnalysis(this));
            
            FileSize = RegisterAnalysisType(new FileSizeAnalysis(this));
        }
    }
}
