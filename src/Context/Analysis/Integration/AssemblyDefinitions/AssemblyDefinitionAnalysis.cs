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
            var refStringCount = target.referenceStrings.Count;
            var refCount = target.references.Count;

            var referenceDiff = refStringCount - refCount;
            
            if (referenceDiff > 1)
            {
                throw new NotSupportedException("Make sure to not lose references!");
            }
            
            target.referenceStrings.Clear();
            target.references.Sort();
            target.references = target.references.Distinct().ToList();

            for (var i = 0; i < target.references.Count; i++)
            {
                var reference = target.references[i];

                if (reference == null)
                {
                    throw new ArgumentException(
                        $"Cannot write a null reference for index [{i}] of [{target.Name}]."
                    );
                }

                target.referenceStrings.Add(reference.guid);
            }

            target.assetModel.references = target.referenceStrings.ToArray();
        }
    }
}
