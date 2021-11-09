using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class ReferenceStyleAnalysis : AssemblyDefinitionAnalysis
    {
        public ReferenceStyleAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable() => false;

        public override string ShortName => "Ref. Style";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.ReferenceStyle;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            var shouldUseGuidReferences = target.IsAppalachiaManaged;
            
            for (var index = 0; index < target.references.Count; index++)
            {
                var reference = target.references[index];

                if (reference.assembly == null)
                {
                    continue;
                }

                var isIssue = false;

                if (reference.IsGuidReference != shouldUseGuidReferences)
                {
                    isIssue = true;
                    SetColor(group, target, reference, this);
                }

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.Paired(
                        reference.DisplayName,
                        reference.guid,
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
            var allAssemblies = AssemblyDefinitionMetadata.Instances.ToList();

            var changed = false;

            target.referenceStrings.Clear();

            for (var i = 0; i < target.references.Count; i++)
            {
                var reference = target.references[i];

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

                reference.guid = reference.assembly?.Id;
                target.referenceStrings.Add(reference.assembly?.Id);
            }

            WriteReferences(target);

            if (changed)
            {
                target.SaveFile(useTestFiles, reimport);
            }
        }
    }
}
