using System.Collections.Generic;

namespace Appalachia.Core.Preferences
{
    public abstract class PREF_STATE_BASE
    {
        internal static List<PREF_BASE> allPreferences;
        
        public abstract void Awake();
    }
}
