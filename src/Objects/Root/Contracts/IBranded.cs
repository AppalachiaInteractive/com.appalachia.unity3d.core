namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IBranded
    {
        string GetBackgroundColor();

        string GetFallbackSubtitle();

        string GetFallbackTitle();

        string GetSubtitle();

        string GetTitle();

        string GetTitleColor();
    }
}
