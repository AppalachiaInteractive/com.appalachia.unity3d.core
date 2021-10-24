using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Integration
{
    [Serializable]
    public class RepositoryAnalysis : AnalysisMetadata<RepositoryAnalysis,
        RepositoryMetadata, RepositoryAnalysis.Types>
    {
        public enum Types
        {
            All = 00000,
            DependencyPresence = 00030,
            DependencyVersions = 00040,
            DependencyValidity = 00120,
        }
        
        public AnalysisResult<Types> DependencyPresence;
        public AnalysisResult<Types> DependencyValidity;
        public AnalysisResult<Types> DependencyVersions;

        public override Object Asset => Target.PackageJsonAsset;

        protected override void ClearAnalysisResults()
        {
        }

        protected override void OnAnalyze()
        {
            var uniqueDependencies = new Dictionary<string, RepositoryDependency>();

            AnalzyeRepository(Target, uniqueDependencies);

            AnalyzeDependencies(
                this,
                Target,
                uniqueDependencies,
                DependencyValidity,
                DependencyPresence,
                DependencyVersions
            );
        }

        protected override void RegisterAllAnalysis()
        {
            DependencyPresence = RegisterAnalysis(
                "Dep. Presence",
                Types.DependencyPresence,
                CheckDependencyPresence,
                FixDependencyPresence
            );

            DependencyValidity = RegisterAnalysis(
                "Dep. Validity",
                Types.DependencyValidity,
                CheckDependencyValidity,
                FixDependencyValidity
            );

            DependencyVersions = RegisterAnalysis(
                "Dep. Versions",
                Types.DependencyVersions,
                CheckDependencyVersions,
                FixDependencyVersions
            );
        }

        private bool CheckDependencyPresence()
        {
            return Target.dependencies.Any(d => d.IsMissing);
        }

        private bool CheckDependencyValidity()
        {
            return Target.dependencies.Any(d => !d.IsValid);
        }

        private bool CheckDependencyVersions()
        {
            return Target.dependencies.Any(d => d.IsOutOfDate);
        }

        private void FixDependencyPresence(bool useTestFiles, bool reimport)
        {
            var changed = false;

            var npmPackage = Target.npmPackage;

            foreach(var dependency in Target.dependencies)
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
                Target.SavePackageJson(useTestFiles, reimport);
            }
        }

        private void FixDependencyValidity(bool useTestFiles, bool reimport)
        {
            var changed = false;

            var npmPackage = Target.npmPackage;

            var deps = Target.dependencies.ToArray();
            
            foreach(var dependency in deps)
            {
                if (!dependency.IsValid)
                {
                    changed = true;
                    Target.dependencies.Remove(dependency);
                    npmPackage.Dependencies.Remove(dependency.name);
                }
            }

            if (changed)
            {
                Target.SavePackageJson(useTestFiles, reimport);
            }
        }

        private void FixDependencyVersions(bool useTestFiles, bool reimport)
        {
            var changed = false;

            var npmPackage = Target.npmPackage;

            foreach(var dependency in Target.dependencies)
            {
                var refRepo = dependency.repository;

                if (refRepo != null)
                {
                    var packageName = refRepo.PackageName;
                    var packageVersion = refRepo.PackageVersion;

                    if (npmPackage.Dependencies.ContainsKey(packageName))
                    {
                        var currentVersion = npmPackage.Dependencies[packageName];

                        if (currentVersion != packageVersion)
                        {
                            changed = true;
                            npmPackage.Dependencies[packageName] = packageVersion;
                        }
                    }
                }
            }

            if (changed)
            {
                Target.SavePackageJson(useTestFiles, reimport);
            }
        }

        private void AnalyzeDependencies(
            RepositoryAnalysis analysis,
            RepositoryMetadata repository,
            Dictionary<string, RepositoryDependency> uniqueDependencies,
            AnalysisResult<Types> dependencyValidity,
            AnalysisResult<Types> dependencyPresence,
            AnalysisResult<Types> dependencyVersions)
        {
            foreach (var dep in uniqueDependencies)
            {
                repository.dependencies.Add(dep.Value);                
            }

            var deps = repository.dependencies.ToArray();
            
            foreach(var dependency in deps)
            {
                if (!dependency.IsValid)
                {
                    SetColor(analysis, repository, dependency, dependencyValidity);
                }

                if (dependency.IsMissing)
                {
                    SetColor(analysis, repository, dependency, dependencyPresence);
                }

                if (dependency.IsOutOfDate)
                {
                    SetColor(analysis, repository, dependency, dependencyVersions);
                }
            }
        }

        private void AnalzyeRepository(
            RepositoryMetadata repository,
            Dictionary<string, RepositoryDependency> uniqueDependencies)
        {
            var npmPackage = repository.npmPackage;

            if (npmPackage == null)
            {
                return;
            }

            if (npmPackage.Dependencies == null)
            {
                return;
            }

            foreach (var dependency in npmPackage.Dependencies)
            {
                var packageName = dependency.Key;
                var packageVersion = dependency.Value;
                
                var newDep = new RepositoryDependency(packageName, packageVersion);
                
                uniqueDependencies.Add(dependency.Key, newDep);

                var repoMatch = RepositoryMetadata.FindById(packageName);

                if (repoMatch != null)
                {
                    newDep.repository = repoMatch;
                }
            }
        }
    }
}
