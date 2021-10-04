namespace Appalachia.Core.Labels
{
    public struct LabelAssignmentTerm
    {
        public LabelAssignmentTerm(string term, float allowedMagnitude)
        {
            this.term = term;
            this.allowedMagnitude = allowedMagnitude;
        }

        public readonly string term;
        public readonly float allowedMagnitude;

        public override string ToString()
        {
            return $"{term} ({allowedMagnitude:F1}m)";
        }
    }
}
