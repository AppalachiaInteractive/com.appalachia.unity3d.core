#region

using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Preferences;

#endregion

namespace Appalachia.Core.Collections
{
#if UNITY_EDITOR

    [UnityEditor.InitializeOnLoad]
    public static class IndexedCollectionsGlobals
    {
        [ExecuteOnEnable]
        private static void Initialize()
        {
            _UI_DEBUG.WakeUp();
        }

        #region MENU_ENABLE_

        [NonSerialized]
        public static readonly PREF<bool> _UI_DEBUG = PREFS.REG(
            PKG.Prefs.Group,
            "Show Debugging Fields",
            false
        );

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Indexed Collections/Show All Fields", true, PKG.Menu.Appalachia.Tools.Priority)]
        private static bool MENU_UI_DEBUG_VALIDATE()
        {
            if (!_UI_DEBUG.IsAwake)
            {
                return false;
            }

            UnityEditor.Menu.SetChecked(
                PKG.Menu.Appalachia.Tools.Base + "Indexed Collections/Show All Fields",
                _UI_DEBUG.Value
            );
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Indexed Collections/Show All Fields", priority = PKG.Menu.Appalachia.Tools.Priority)]
        private static void MENU_UI_DEBUG()
        {
            if (!_UI_DEBUG.IsAwake)
            {
                return;
            }

            _UI_DEBUG.Value = !_UI_DEBUG.Value;
        }

        #endregion
    }

#endif
}
