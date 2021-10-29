using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class NamespaceAnalysis : AssemblyDefinitionAnalysis
    {
        public NamespaceAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => false;
        public override string ShortName => "Namespace";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.Namespace;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            if (!target.IsAppalachia)
            {
                return;
            }

            var ns1 = target.rootNamespaceCurrent;
            var ns2 = target.rootNamespaceIdeal;
            var bothNull = string.IsNullOrWhiteSpace(ns1) && string.IsNullOrWhiteSpace(ns2);

            var hasIssue = !bothNull && (ns1 != ns2);

            var isIssue = false;

            if (hasIssue)
            {
                isIssue = true;
                SetColor(group, target, this);
            }

            messages.Add(
                isIssue,
                AnalysisMessagePart.Paired(
                    "Ideal Namespace",
                    target.rootNamespaceIdeal,
                    isIssue,
                    IssueColor,
                    goodColor
                )
            );
        }

        protected override void CorrectIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            target.assetModel.rootNamespace = target.rootNamespaceIdeal;

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
