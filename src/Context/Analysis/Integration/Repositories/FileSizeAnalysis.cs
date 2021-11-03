using System.Collections.Generic;
using System.IO;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Extensions;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class FileSizeAnalysis : RepositoryAnalysis
    {
        public FileSizeAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => false;
        public override bool IsCorrectable => false;
        public override string ShortName => "File Size";

        public override RepositoryAnalysisGroup.Types Type =>
            RepositoryAnalysisGroup.Types.FileSize;

        protected override void AnalyzeIssue(
            RepositoryAnalysisGroup group,
            RepositoryMetadata target,
            List<AnalysisMessage> messages)
        {
            var files = target.GetLargestFiles(20);

            foreach (var file in files)
            {
                var isIssue = file.Length > RepositoryMetadata.TARGET_FILE_MAX_SIZE;

                if (isIssue)
                {
                    SetColor(group, target, this);
                }

                var color = file.Length.ToGradient(
                    0,
                    RepositoryMetadata.TARGET_MAX_SIZE,
                    ColorPalette.Default.neutralToBad
                );

                messages.Add(
                    isIssue,
                    AnalysisMessagePart.PairedWithButton(file.RelativePath, file.Length.ToFileSize(), color,
                        "Select", color, () => AssetDatabaseManager.OpenFolderInExplorer(file.WindowsDirectoryPath)
                        )
                );
            }
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
