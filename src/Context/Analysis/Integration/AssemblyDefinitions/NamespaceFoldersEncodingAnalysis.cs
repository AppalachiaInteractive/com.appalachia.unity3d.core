using System.Collections.Generic;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class NamespaceFoldersEncodingAnalysis : AssemblyDefinitionAnalysis
    {
        public NamespaceFoldersEncodingAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => true;
        public override string ShortName => "NS Dir. Formatting";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.NamespaceFoldersEncoding;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            if (!target.IsAsset)
            {
                return;
            }

            foreach (var folder in target.dotSettings.AllFolders)
            {
                var isIssue = false;

                if (folder.HasEncodingIssue)
                {
                    isIssue = true;
                    SetColor(group, target, this);
                }

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.Left(folder.encoded, isIssue, IssueColor, goodColor)
                );
            }
        }

        protected override void CorrectIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            target.dotSettings.FixEncodingIssues();
            target.SaveDotSettingsFile(useTestFiles);
        }
    }
}
