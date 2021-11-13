namespace Appalachia.Core.Preferences.API
{
    public interface IPAPI<T>
    {
        T Draw(string key, string label, T value, T low, T high);

        T Get(string key, T defaultValue, T low, T high);

        void Save(string key, T value, T low, T high);
    }
}
