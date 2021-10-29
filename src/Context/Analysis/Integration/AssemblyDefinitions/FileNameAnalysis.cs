using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class FileNameAnalysis : AssemblyDefinitionAnalysis
    {
        public FileNameAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => false;
        public override string ShortName => "File Name";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.FileName;

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

            if (target.filenameCurrent != target.FilenameIdeal)
            {
                isIssue = true;
                SetColor(group, target, this);
            }

            messages.Add(
                isIssue,
                AnalysisMessagePart.Paired(
                    "Ideal File Name",
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
            AssetDatabaseManager.RenameAsset(target.Path, target.FilenameIdeal);
        }
    }
}
