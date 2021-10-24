using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class ReferenceValidityAnalysis : AssemblyDefinitionAnalysis
    {
        public ReferenceValidityAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override string ShortName => "Ref. Validity";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.ReferenceValidity;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            for (var index = 0; index < target.references.Count; index++)
            {
                var reference = target.references[index];

                var isIssue = false;

                if (!reference.IsValid)
                {
                    isIssue = true;

                    SetColor(group, target, reference, this);
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
            for (var i = target.references.Count - 1; i >= 0; i--)
            {
                var reference = target.references[i];

                if (reference.assembly == null)
                {
                    target.references.RemoveAt(i);
                }
            }

            WriteReferences(target);

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
