using System;
using System.Linq;
using Appalachia.CI.Integration.Assemblies;
using Appalachia.Core.Context.Analysis.Core;

namespace Appalachia.Core.Context.Analysis.Integration.AssemblyDefinitions
{
    public abstract class AssemblyDefinitionAnalysis : AnalysisType<AssemblyDefinitionAnalysisGroup,
        AssemblyDefinitionMetadata, AssemblyDefinitionAnalysisGroup.Types>
    {
        protected AssemblyDefinitionAnalysis(AssemblyDefinitionAnalysisGroup group) : base(group)
        {
        }

        protected void WriteReferences(AssemblyDefinitionMetadata target)
        {
            if (target.references.Count < target.referenceStrings.Count)
            {
                throw new NotSupportedException("Make sure to not lose references!");
            }
            
            target.referenceStrings.Clear();
            target.references.Sort();
            target.references = target.references.Distinct().ToList();

            for (var i = 0; i < target.references.Count; i++)
            {
                var reference = target.references[i];

                target.referenceStrings.Add(reference.guid);
            }

            target.assetModel.references = target.referenceStrings.ToArray();
        }
    }
}
