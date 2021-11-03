using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class AssemblyReferenceLevelAnalysis : AssemblyDefinitionAnalysis
    {
        public AssemblyReferenceLevelAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => false;
        public override string ShortName => "Ref. Level";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.AssemblyReferenceLevel;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            var thisLevel = target.GetAssemblyReferenceLevel();

            for (var index = 0; index < target.references.Count; index++)
            {
                var reference = target.references[index];

                if (reference.assembly == null)
                {
                    continue;
                }

                var refLevel = reference.assembly.GetAssemblyReferenceLevel();

                var isIssue = false;
                var messageText = $"{refLevel.ToString().Bold()} <= {thisLevel.ToString().Italic()}";

                if (target.IsAppalachiaManaged && (refLevel > thisLevel))
                {
                    isIssue = true;
                    messageText = $"{refLevel.ToString().Bold()} > {thisLevel.ToString().Italic()}";

                    reference.HasReferenceLevelIssue = true;
                    SetColor(group, target, reference, this);
                }

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.Paired(
                        reference.DisplayName,
                        messageText,
                        isIssue,
                        IssueColor,
                        goodColor
                    )
                );
            }
        }

        protected override void CorrectIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            for (var index = target.references.Count - 1; index >= 0; index--)
            {
                var reference = target.references[index];

                if (reference.HasReferenceLevelIssue)
                {
                    target.references.RemoveAt(index);
                }
            }

            WriteReferences(target);

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
