using System;
using Appalachia.Core.Preferences;

namespace Appalachia.Core.Context.Interfaces
{
    public interface IPreferencesDrawer
    {
        void RegisterFilterPref<T>(ref PREF<T> pref, string grouping, string settingName, T defaultValue, Func<bool> enableIf);

        void RegisterFilterPref<T>(PREF<T> pref, Func<bool> enableIf = null);
    }
}
