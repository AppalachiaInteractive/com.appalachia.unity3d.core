using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class AssemblyNameAnalysis : AssemblyDefinitionAnalysis
    {
        public AssemblyNameAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override string ShortName => "Assembly Name";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.AssemblyName;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            if (!target.IsAppalachia)
            {
                return;
            }

            var isIssue = false;
            
            if (target.AssemblyCurrent != target.assemblyIdeal)
            {
                isIssue = true;
                SetColor(group, target, this);
            }

            messages.Add(
                isIssue,
                AnalysisMessagePart.Paired(
                    "Ideal Assembly Name",
                    target.assemblyIdeal,
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
            target.assetModel.name = target.assemblyIdeal;

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
