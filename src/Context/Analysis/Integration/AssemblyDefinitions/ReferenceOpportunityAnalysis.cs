using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public sealed class ReferenceOpportunityAnalysis : AssemblyDefinitionAnalysis
    {
        public ReferenceOpportunityAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        public override string ShortName => "Ref. Opportunity";

        public override AssemblyDefinitionAnalysisGroup.Types Type =>
            AssemblyDefinitionAnalysisGroup.Types.ReferenceOpportunity;

        protected override void AnalyzeIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            List<AnalysisMessage> messages)
        {
            var uniqueReferences = new HashSet<AssemblyDefinitionMetadata>();
            var uniqueOpportunities = new HashSet<AssemblyDefinitionMetadata>();

            for (var index = 0; index < target.references.Count; index++)
            {
                var reference = target.references[index];

                uniqueReferences.Add(reference.assembly);
            }

            var thisLevel = target.GetAssemblyReferenceLevel();

            foreach (var instance in AssemblyDefinitionMetadata.Instances)
            {
                if (!target.Name.StartsWith("Appalachia") || (thisLevel > 1000))
                {
                    break;
                }

                if (!instance.Name.StartsWith("Appalachia"))
                {
                    continue;
                }

                var instanceLevel = instance.GetAssemblyReferenceLevel();
                var oportunityCutoffLevel = instance.GetOpportunityCutoffLevel();
                
                if (uniqueReferences.Contains(instance) || uniqueOpportunities.Contains(instance))
                {
                    messages.Add(false, AnalysisMessagePart.Center(instance.Name, goodColor));
                    
                    continue;
                }

                if ((instanceLevel < oportunityCutoffLevel) && (instanceLevel < thisLevel))
                {
                    uniqueOpportunities.Add(instance);

                    var oppReff = new AssemblyDefinitionReference(instance);

                    target.opportunities.Add(oppReff);
                    
                    messages.Add(true, AnalysisMessagePart.Center(instance.Name, IssueColor));

                    SetColor(group, target, oppReff, this);
                }
            }
        }

        protected override void CorrectIssue(
            AssemblyDefinitionAnalysisGroup group,
            AssemblyDefinitionMetadata target,
            bool useTestFiles,
            bool reimport)
        {
            var referenceLookup = target.references.Select(r => r.guid).ToHashSet();

            foreach (var opportunity in target.opportunities)
            {
                if (!referenceLookup.Contains(opportunity.guid))
                {
                    target.references.Add(opportunity);
                }
            }

            WriteReferences(target);

            target.SaveFile(useTestFiles, reimport);
        }
    }
}
