using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Integration
{
    [Serializable]
    public class AssemblyDefinitionAnalysis : AnalysisMetadata<AssemblyDefinitionAnalysis,
        AssemblyDefinitionMetadata, AssemblyDefinitionAnalysis.Types>
    {
        public enum Types
        {
            All = 00000,
            AssemblyReferenceLevel = 00010,
            ReferenceOpportunity = 00020,
            NameAssembly = 00050,
            NameFile = 00060,
            Namespace = 00070,
            ReferenceStyle = 00080,
            ReferenceValidity = 00090,
            Sorting = 00100,
            ReferenceDuplicates = 00110,
            NamespaceFoldersExclusions = 00130,
            NamespaceFoldersEncoding = 00140
        }

        public AnalysisResult<Types> AssemblyReferenceLevel;
        public AnalysisResult<Types> NameAssembly;
        public AnalysisResult<Types> NameFile;
        public AnalysisResult<Types> Namespace;
        public AnalysisResult<Types> NamespaceFoldersEncoding;
        public AnalysisResult<Types> NamespaceFoldersExclusions;
        public AnalysisResult<Types> ReferenceDuplicates;
        public AnalysisResult<Types> ReferenceOpportunity;
        public AnalysisResult<Types> ReferenceStyle;
        public AnalysisResult<Types> ReferenceValidity;
        public AnalysisResult<Types> Sorting;

        public override Object Asset => Target.asset;

        protected override void ClearAnalysisResults()
        {
            RegisterAllAnalysis();
        }

        protected override void OnAnalyze()
        {
            var uniqueDependencies = new Dictionary<string, RepositoryDependency>();
            var uniqueReferences = new HashSet<AssemblyDefinitionMetadata>();
            var uniqueReferenceStrings = new HashSet<string>();
            var uniqueOpportunities = new HashSet<AssemblyDefinitionMetadata>();

            Target.InitializeForAnalysis();

            AnalyzeProject(
                this,
                Target,
                NameAssembly,
                NameFile,
                Namespace,
                NamespaceFoldersExclusions,
                NamespaceFoldersEncoding
            );

            GetDependencies(Target.repository, uniqueDependencies);

            AnalyzeReferences(
                this,
                Target,
                uniqueDependencies,
                uniqueReferences,
                uniqueReferenceStrings,
                uniqueOpportunities,
                ReferenceDuplicates,
                ReferenceValidity,
                AssemblyReferenceLevel,
                ReferenceStyle,
                Sorting,
                ReferenceOpportunity
            );
        }

        protected override void RegisterAllAnalysis()
        {
            NameAssembly = RegisterAnalysis(
                "Assembly Naming",
                Types.NameAssembly,
                CheckNameAssembly,
                FixNameAssembly
            );

            NameFile = RegisterAnalysis("File Naming", Types.NameFile, CheckNameFile, FixNameFile);

            Namespace = RegisterAnalysis("Namespace", Types.Namespace, CheckNamespace, FixNamespace);

            NamespaceFoldersExclusions = RegisterAnalysis(
                "NS Dir. Exclusions",
                Types.NamespaceFoldersExclusions,
                CheckNamespaceFoldersExclusions,
                FixNamespaceFoldersExclusions
            );

            NamespaceFoldersEncoding = RegisterAnalysis(
                "NS Dir. Formatting",
                Types.NamespaceFoldersEncoding,
                CheckNamespaceFoldersEncoding,
                FixNamespaceFoldersEncoding
            );

            ReferenceValidity = RegisterAnalysis(
                "Ref. Validity",
                Types.ReferenceValidity,
                CheckReferenceValidity,
                FixReferenceValidity
            );

            ReferenceStyle = RegisterAnalysis(
                "Ref. Style",
                Types.ReferenceStyle,
                CheckReferenceStyle,
                FixReferenceStyle
            );

            AssemblyReferenceLevel = RegisterAnalysis(
                "Dep. Level",
                Types.AssemblyReferenceLevel,
                CheckAssemblyReferenceLevel,
                FixAssemblyReferenceLevel
            );

            ReferenceOpportunity = RegisterAnalysis(
                "Dep. Opportunity",
                Types.ReferenceOpportunity,
                CheckReferenceOpportunity,
                FixReferenceOpportunity
            );

            Sorting = RegisterAnalysis("Ref. Sorting", Types.Sorting, CheckSorting, FixSorting);

            ReferenceDuplicates = RegisterAnalysis(
                "Ref. Duplicates",
                Types.ReferenceDuplicates,
                CheckReferenceDuplicates,
                FixReferenceDuplicates
            );
        }

        private void AnalyzeProject(
            AssemblyDefinitionAnalysis analysis,
            AssemblyDefinitionMetadata assembly,
            AnalysisResult<Types> nameAssembly,
            AnalysisResult<Types> nameFile,
            AnalysisResult<Types> ns,
            AnalysisResult<Types> namespaceFoldersExclusions,
            AnalysisResult<Types> namespaceFoldersFormatting)
        {
            if (nameAssembly.HasIssue)
            {
                SetColor(analysis, assembly, nameAssembly);
            }

            if (nameFile.HasIssue)
            {
                SetColor(analysis, assembly, nameFile);
            }

            if (ns.HasIssue)
            {
                SetColor(analysis, assembly, ns);
            }

            assembly.dotSettings.CheckNamespaceFolderIssues(assembly);

            if (namespaceFoldersExclusions.HasIssue)
            {
                SetColor(analysis, assembly, namespaceFoldersExclusions);
            }

            if (namespaceFoldersFormatting.HasIssue)
            {
                SetColor(analysis, assembly, namespaceFoldersFormatting);
            }
        }

        private void AnalyzeReferences(
            AssemblyDefinitionAnalysis analysis,
            AssemblyDefinitionMetadata assembly,
            Dictionary<string, RepositoryDependency> uniqueDependencies,
            HashSet<AssemblyDefinitionMetadata> uniqueReferences,
            HashSet<string> uniqueReferenceStrings,
            HashSet<AssemblyDefinitionMetadata> uniqueOpportunities,
            AnalysisResult<Types> referenceDuplicates,
            AnalysisResult<Types> referenceValidity,
            AnalysisResult<Types> dependencyLevel,
            AnalysisResult<Types> referenceStyle,
            AnalysisResult<Types> sorting,
            AnalysisResult<Types> referenceOpportunity)
        {
            assembly.SetReferences();

            for (var index = 0; index < assembly.references.Count; index++)
            {
                var reference = assembly.references[index];
                if (uniqueReferenceStrings.Contains(reference.guid))
                {
                    SetColor(analysis, assembly, reference, referenceDuplicates);
                    reference.isDuplicate = true;
                }
                else
                {
                    uniqueReferenceStrings.Add(reference.guid);
                }

                if (reference.assembly == null)
                {
                    SetColor(analysis, assembly, reference, referenceValidity);
                    continue;
                }

                uniqueReferences.Add(reference.assembly);

                var refRepo = reference.assembly.repository;

                if (refRepo == assembly.repository)
                {
                    continue;
                }

                var packageName = refRepo.PackageName;

                if (uniqueDependencies.ContainsKey(packageName))
                {
                    uniqueDependencies[packageName].repository = refRepo;
                }
                else
                {
                    var newDep = new RepositoryDependency(refRepo);
                    uniqueDependencies.Add(packageName, newDep);
                }
            }

            var thisLevel = assembly.GetAssemblyAssemblyReferenceLevel();

            for (var index = 0; index < assembly.references.Count; index++)
            {
                var reference = assembly.references[index];
                if (reference.assembly == null)
                {
                    continue;
                }

                var refLevel = reference.assembly.GetAssemblyAssemblyReferenceLevel();

                if (assembly.IsAppalachia && (refLevel > thisLevel))
                {
                    reference.isLevelIssue = true;
                    SetColor(analysis, assembly, reference, dependencyLevel);
                }

                if (!reference.IsGuidReference)
                {
                    SetColor(analysis, assembly, reference, referenceStyle);
                }
            }

            for (var i = 0; i < (assembly.references.Count - 1); i++)
            {
                var ref1 = assembly.references[i];
                var ref2 = assembly.references[i + 1];

                if (ref1 > ref2)
                {
                    ref1.outOfSorts = true;
                    ref2.outOfSorts = true;
                    SetColor(analysis, assembly, ref1, ref2, sorting);
                }
            }

            foreach (var instance in AssemblyDefinitionMetadata.Instances)
            {
                if (!assembly.Name.StartsWith("Appalachia") || (thisLevel > 1000))
                {
                    break;
                }

                if (!instance.Name.StartsWith("Appalachia"))
                {
                    continue;
                }

                if (uniqueReferences.Contains(instance) || uniqueOpportunities.Contains(instance))
                {
                    continue;
                }

                var instanceLevel = instance.GetAssemblyAssemblyReferenceLevel();
                var oportunityCutoffLevel = instance.GetOpportunityCutoffLevel();

                if ((instanceLevel < oportunityCutoffLevel) && (instanceLevel < thisLevel))
                {
                    uniqueOpportunities.Add(instance);
                    var oppReff = new AssemblyDefinitionReference(instance);
                    assembly.opportunities.Add(oppReff);

                    SetColor(analysis, assembly, oppReff, referenceOpportunity);
                }
            }
        }

        private bool CheckAssemblyReferenceLevel()
        {
            return Target.references.Any(d => d.isLevelIssue);
        }

        private bool CheckNameAssembly()
        {
            if (!Target.IsAppalachia)
            {
                return false;
            }

            return Target.AssemblyCurrent != Target.assemblyIdeal;
        }

        private bool CheckNameFile()
        {
            return Target.filenameCurrent != Target.FilenameIdeal;
        }

        private bool CheckNamespace()
        {
            if (!Target.IsAppalachia)
            {
                return false;
            }

            var ns1 = Target.rootNamespaceCurrent;
            var ns2 = Target.rootNamespaceIdeal;

            var bothNull = string.IsNullOrWhiteSpace(ns1) && string.IsNullOrWhiteSpace(ns2);

            return !bothNull && (ns1 != ns2);
        }

        private bool CheckNamespaceFoldersEncoding()
        {
            return Target.dotSettings?.AllFolders.Any(f => !f.excluded || f.encodingIssue) ?? false;
        }

        private bool CheckNamespaceFoldersExclusions()
        {
            return Target.dotSettings?.AllFolders.Any(f => !f.excluded || f.encodingIssue) ?? false;
        }

        private bool CheckReferenceDuplicates()
        {
            return Target.references?.Any(r => r.isDuplicate) ?? false;
        }

        private bool CheckReferenceOpportunity()
        {
            return Target.opportunities.Any();
        }

        private bool CheckReferenceStyle()
        {
            return Target.references?.Any(s => !s.IsGuidReference) ?? false;
        }

        private bool CheckReferenceValidity()
        {
            return Target.references?.Any(r => r.assembly == null) ?? false;
        }

        private bool CheckSorting()
        {
            return Target.references.Any(d => d.outOfSorts);
        }

        private void FixAssemblyReferenceLevel(bool useTestFiles, bool reimport)
        {
            for (var index = Target.references.Count - 1; index >= 0; index--)
            {
                var reference = Target.references[index];

                if (reference.isLevelIssue)
                {
                    Target.references.RemoveAt(index);
                }
            }

            WriteReferences();

            Target.SaveFile(useTestFiles, reimport);
        }

        private void FixNameAssembly(bool useTestFiles, bool reimport)
        {
            Target.assetModel.name = Target.assemblyIdeal;

            Target.SaveFile(useTestFiles, reimport);
        }

        private void FixNameFile(bool useTestFiles, bool reimport)
        {
            AssetDatabaseManager.RenameAsset(Target.Path, Target.FilenameIdeal);
        }

        private void FixNamespace(bool useTestFiles, bool reimport)
        {
            Target.assetModel.rootNamespace = Target.rootNamespaceIdeal;

            Target.SaveFile(useTestFiles, reimport);
        }

        private void FixNamespaceFoldersEncoding(bool useTestFiles, bool reimport)
        {
            Target.dotSettings.FixEncodingIssues();
            Target.SaveDotSettingsFile(useTestFiles);
        }

        private void FixNamespaceFoldersExclusions(bool useTestFiles, bool reimport)
        {
            Target.dotSettings.AddMissingFolders();
            Target.SaveDotSettingsFile(useTestFiles);
        }

        private void FixReferenceDuplicates(bool useTestFiles, bool reimport)
        {
            WriteReferences();

            Target.SaveFile(useTestFiles, reimport);
        }

        private void FixReferenceOpportunity(bool useTestFiles, bool reimport)
        {
            var referenceLookup = Target.references.Select(r => r.guid).ToHashSet();

            foreach(var opportunity in Target.opportunities)
            {
                if (!referenceLookup.Contains(opportunity.guid))
                {
                    Target.references.Add(opportunity);
                }
            }

            WriteReferences();

            Target.SaveFile(useTestFiles, reimport);
        }

        private void FixReferenceStyle(bool useTestFiles, bool reimport)
        {
            var allAssemblies = AssemblyDefinitionMetadata.Instances.ToList();

            var changed = false;

            Target.referenceStrings.Clear();

            for (var i = 0; i < Target.references.Count; i++)
            {
                var reference = Target.references[i];

                if (reference.IsGuidReference)
                {
                    continue;
                }

                changed = true;

                if (reference.assembly == null)
                {
                    for (var index = 0; index < allAssemblies.Count; index++)
                    {
                        var assemblyToCheck = allAssemblies[index];

                        if (assemblyToCheck.AssemblyCurrent == reference.guid)
                        {
                            reference.assembly = assemblyToCheck;
                            break;
                        }
                    }
                }

                reference.guid = reference.assembly?.guid;
                Target.referenceStrings.Add(reference.assembly?.guid);
            }

            WriteReferences();

            if (changed)
            {
                Target.SaveFile(useTestFiles, reimport);
            }
        }

        private void FixReferenceValidity(bool useTestFiles, bool reimport)
        {
            for (var i = Target.references.Count - 1; i >= 0; i--)
            {
                var reference = Target.references[i];

                if (reference.assembly == null)
                {
                    Target.references.RemoveAt(i);
                }
            }

            WriteReferences();

            Target.SaveFile(useTestFiles, reimport);
        }

        private void FixSorting(bool useTestFiles, bool reimport)
        {
            Target.references.Sort();

            WriteReferences();

            Target.SaveFile(useTestFiles, reimport);
        }

        private void GetDependencies(
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
                var newDep = new RepositoryDependency(dependency.Key, dependency.Value);
                uniqueDependencies.Add(dependency.Key, newDep);

                var repoMatch = RepositoryMetadata.FindById(dependency.Key);

                if (repoMatch != null)
                {
                    newDep.repository = repoMatch;
                }
            }
        }

        private void WriteReferences()
        {
            Target.referenceStrings.Clear();
            Target.references.Sort();
            Target.references = Target.references.Distinct().ToList();

            for (var i = 0; i < Target.references.Count; i++)
            {
                var reference = Target.references[i];

                Target.referenceStrings.Add(reference.guid);
            }

            Target.assetModel.references = Target.referenceStrings.ToArray();
        }
    }
}
