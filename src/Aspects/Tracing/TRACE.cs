#region

using System;
using System.Diagnostics;
using Appalachia.Core.Preferences;

#endregion

namespace Appalachia.Core.Aspects.Tracing
{
    [DebuggerStepThrough]
    public static class TRACE
    {
        #region Constants and Static Readonly

        internal const string _TRACE_LOG_LABEL = "Enable Logging";

        [NonSerialized]
        private static readonly PREF<bool> _enabled = PREFS.REG(PKG.Prefs.Group, _TRACE_LOG_LABEL, false);

        #endregion

#if UNITY_EDITOR
        private const string ENABLED = PKG.Menu.Appalachia.Debug.Base + "Trace Logging/Enabled";

        [UnityEditor.MenuItem(ENABLED, true, PKG.Menu.Appalachia.Debug.Priority)]
        private static bool ENABLED_TOGGLE_V()
        {
            if (!_enabled.IsAwake)
            {
                return false;
            }

            UnityEditor.Menu.SetChecked(ENABLED, _enabled.Value);
            return true;
        }

        [UnityEditor.MenuItem(ENABLED, priority = PKG.Menu.Appalachia.Debug.Priority)]
        private static void ENABLED_TOGGLE()
        {
            if (!_enabled.IsAwake)
            {
                return;
            }

            _enabled.Value = !_enabled.Value;
        }
#endif
    }
}
