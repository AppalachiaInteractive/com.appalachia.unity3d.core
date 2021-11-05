using System;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    [Serializable]
    public class AssemblyDefinitionAnalysisGroup : AnalysisGroup<AssemblyDefinitionAnalysisGroup,
        AssemblyDefinitionMetadata, AssemblyDefinitionAnalysisGroup.Types>
    {
        public enum Types
        {
            All = 0,
            AssemblyReferenceLevel = 1 << 0,
            ReferenceOpportunity = 1 << 1,
            AssemblyName = 1 << 2,
            FileName = 1 << 3,
            Namespace = 1 << 4,
            ReferenceStyle = 1 << 5,
            ReferenceValidity = 1 << 6,
            AssemblyReferenceSorting = 1 << 7,
            ReferenceDuplicates = 1 << 8,
            NamespaceFoldersExclusions = 1 << 9,
            NamespaceFoldersEncoding = 1 <<10,
        }

        public AssemblyNameAnalysis assemblyName;

        public AssemblyReferenceLevelAnalysis AssemblyReferenceLevel;
        public FileNameAnalysis fileName;
        public NamespaceAnalysis Namespace;
        public NamespaceFoldersEncodingAnalysis NamespaceFoldersEncoding;
        public NamespaceFoldersExclusionsAnalysis NamespaceFoldersExclusions;
        public ReferenceDuplicatesAnalysis ReferenceDuplicates;
        public ReferenceOpportunityAnalysis ReferenceOpportunity;
        public ReferenceStyleAnalysis ReferenceStyle;
        public ReferenceValidityAnalysis ReferenceValidity;
        public AssemblyReferenceSortingAnalysis assemblyReferenceSorting;

        public override Object Asset => Target.asset;

        protected override void OnAnalyze()
        {
            Target.InitializeForAnalysis();

            Target.dotSettings.CheckNamespaceFolderIssues(Target);

            Target.SetReferences();
        }

        protected override void RegisterAllAnalysis()
        {
            AssemblyReferenceLevel = RegisterAnalysisType(new AssemblyReferenceLevelAnalysis(this));
            assemblyName = RegisterAnalysisType(new AssemblyNameAnalysis(this));
            fileName = RegisterAnalysisType(new FileNameAnalysis(this));
            Namespace = RegisterAnalysisType(new NamespaceAnalysis(this));
            NamespaceFoldersEncoding = RegisterAnalysisType(new NamespaceFoldersEncodingAnalysis(this));
            NamespaceFoldersExclusions = RegisterAnalysisType(new NamespaceFoldersExclusionsAnalysis(this));
            ReferenceDuplicates = RegisterAnalysisType(new ReferenceDuplicatesAnalysis(this));
            ReferenceOpportunity = RegisterAnalysisType(new ReferenceOpportunityAnalysis(this));
            ReferenceStyle = RegisterAnalysisType(new ReferenceStyleAnalysis(this));
            ReferenceValidity = RegisterAnalysisType(new ReferenceValidityAnalysis(this));
            assemblyReferenceSorting = RegisterAnalysisType(new AssemblyReferenceSortingAnalysis(this));
        }
    }
}
