using System.Collections.Generic;
using System.IO;
using System.Linq;
using Appalachia.CI.Integration.Repositories;
using Appalachia.Core.Context.Analysis.Core;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Extensions;

namespace Appalachia.Core.Context.Analysis.Integration.Repositories
{
    public sealed class DistributableSizeAnalysis : RepositoryAnalysis
    {
        public DistributableSizeAnalysis(RepositoryAnalysisGroup group) : base(group)
        {
        }

        public override bool IsAutoCorrectable => false;
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
                if (target.IsAppalachia)
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

            var currentSize = target.DistributableFile.Length.ToFileSize();
            var targetSize = RepositoryMetadata.TARGET_MAX_SIZE.ToFileSize();
            var messageText = $"{currentSize} > {targetSize}";
            
            messages.Add(
                isIssue,
                AnalysisMessagePart.Paired(
                    target.DistributableFile.Name,
                    messageText,
                    isIssue,
                    IssueColor,
                    goodColor
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
