using System.Collections.Generic;

namespace Appalachia.Core.Preferences
{
    public abstract class PREF_STATE_BASE
    {
        #region Preferences

        internal static List<PREF_BASE> allPreferences;

        #endregion

        public abstract void Awake();
    }
}
