namespace Appalachia.Core.Probability
{
    public interface IProbabilityProvider
    {
        bool Enabled { get; }

        double Probability { get; }
    }
}
