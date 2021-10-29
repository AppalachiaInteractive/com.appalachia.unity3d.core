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
            All = 00000,
            AssemblyReferenceLevel = 00010,
            ReferenceOpportunity = 00020,
            AssemblyName = 00050,
            FileName = 00060,
            Namespace = 00070,
            ReferenceStyle = 00080,
            ReferenceValidity = 00090,
            Sorting = 00100,
            ReferenceDuplicates = 00110,
            NamespaceFoldersExclusions = 00130,
            NamespaceFoldersEncoding = 00140
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
        public SortingAnalysis Sorting;

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
            Sorting = RegisterAnalysisType(new SortingAnalysis(this));
        }
    }
}
