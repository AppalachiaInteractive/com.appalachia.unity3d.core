using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;
using Appalachia.Utility.Extensions;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class DistributableSizeAnalysis : RepositoryAnalysis
    {
        public DistributableSizeAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable() => false;
        public override bool IsCorrectable => false;
        public override string ShortName => "Dist. Size";

        public override RepositoryAnalysisGroup.Types Type =>
            RepositoryAnalysisGroup.Types.DistributableSize;

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            if (target.DistributableFile == null)
            {
                if (target.IsAppalachiaManaged)
                {
                    messages.Add(
                        true,
                        AnalysisMessagePart.Center(
                            "Could not find!",
                            IssueColor
                        )
                    );
                }
                
                return;
            }
            
            var isIssue = target.DistributableFile.Length > RepositoryMetadata.TARGET_MAX_SIZE;
            
            if (isIssue)
            {
                SetColor(group, target, this);
            }

            var currentSize = target.DistributableFile.Length;
            var targetSize = RepositoryMetadata.TARGET_MAX_SIZE;
            
            var currentSizeString = currentSize.ToFileSize();
            var targetSizeString = targetSize.ToFileSize();
            var messageText = $"{currentSizeString} {(currentSize > targetSize ? ">" : "<=" )} {targetSizeString}";

            var file = target.DistributableFile;
            messages.Add(
                isIssue,
                AnalysisMessagePart.PairedWith2Buttons(
                    target.DistributableFile.Name,
                    messageText,
                    isIssue ? IssueColor : goodColor,
                    "Select",
                    isIssue ? IssueColor : goodColor,
                    () => AssetDatabaseManager.SetSelection(file.RelativePath),
                    "Show",
                    isIssue ? IssueColor : goodColor,
                    () => AssetDatabaseManager.OpenFolderInExplorer(file.ParentWindowsDirectoryFullPath),
                    expandWidthRight: false,
                    widthRight: 95f,
                    widthButton1: 55f,
                    widthButton2: 45f
                )
            );
        }

        protected override void CorrectIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            bool useTestFiles,
            bool reimport)
        {
           
        }
    }
}
