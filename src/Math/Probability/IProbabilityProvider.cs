namespace Appalachia.Core.Math.Probability
{
    public interface IProbabilityProvider
    {
        bool Enabled { get; }

        double Probability { get; }
    }
}
