using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class SortingAnalysis : AssemblyDefinitionAnalysis
    {
        public SortingAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override string ShortName => "Ref. Sorting";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.Sorting;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            for (var i = 0; i < (target.references.Count - 1); i++)
            {
                var ref1 = target.references[i];
                var ref2 = target.references[i + 1];

                var isIssue = false;

                if (ref1 > ref2)
                {
                    ref1.HasSortingIssue = true;
                    ref2.HasSortingIssue = true;

                    isIssue = true;

                    SetColor(group, target, ref1, ref2, this);
                }

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.Center(ref1.DisplayName, isIssue, IssueColor, goodColor)
                );
            }
        }

        protected override void CorrectIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            target.references.Sort();

            WriteReferences(target);

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
