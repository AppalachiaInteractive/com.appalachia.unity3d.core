using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class ReferenceDuplicatesAnalysis : AssemblyDefinitionAnalysis
    {
        public ReferenceDuplicatesAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable() => true;

        public override string ShortName => "Ref. Duplicates";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.ReferenceDuplicates;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            var uniqueReferenceStrings = new HashSet<string>();

            for (var index = 0; index < target.references.Count; index++)
            {
                var reference = target.references[index];

                var isIssue = false;

                if (uniqueReferenceStrings.Contains(reference.guid))
                {
                    isIssue = true;

                    SetColor(group, target, reference, this);
                    reference.IsDuplicated = true;
                }
                else
                {
                    uniqueReferenceStrings.Add(reference.guid);
                }

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.Center(reference.DisplayName, isIssue, IssueColor, goodColor)
                );
            }
        }

        protected override void CorrectIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            WriteReferences(target);

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
