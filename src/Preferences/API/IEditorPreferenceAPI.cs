namespace Appalachia.Core.Preferences.API
{
    public interface IEditorPreferenceAPI<T>
    {
        T Draw(string label, T value, T low, T high);

        T Draw(PREF<T> pref);

        T Get(string key, T defaultValue, T low, T high);

        void Save(PREF<T> pref);

        void Save(string key, T value, T low, T high);
    }
}
