using UnityEngine;

namespace Appalachia.Core.Labeling
{
    public struct LabelAssignmentCollection
    {
        public LabelAssignmentCollection(string baseTerm, Vector3 multiplier, params LabelAssignmentTerm[] terms)
        {
            this.baseTerm = baseTerm;
            this.terms = terms;
            this.multiplier = multiplier;
        }

        public readonly string baseTerm;
        public readonly Vector3 multiplier;

        public readonly LabelAssignmentTerm[] terms;
    }
}