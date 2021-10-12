#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Preferences;
using UnityEditor;

#endregion

namespace Appalachia.Core.Aspects.Tracing
{
    [AlwaysInitializeOnLoad]
    public static class TRACE
    {
#if UNITY_EDITOR

        public const string _TRACE_LOG_GROUPING = "Appalachia/Trace Logging";
        public const string _TRACE_LOG_LABEL = "Enable Logging";

        public const string MENU_BASE = "Tools/Trace Logging/";

        [NonSerialized]
        private static readonly PREF<bool> _enabled = PREFS.REG(
            _TRACE_LOG_GROUPING,
            _TRACE_LOG_LABEL,
            false
        );

        private const string ENABLED = MENU_BASE + "Enabled";

        [MenuItem(ENABLED, true)]
        private static bool ENABLED_TOGGLE_V()
        {
            if (!_enabled.IsAwake)
            {
                return false;
            }

            Menu.SetChecked(ENABLED, _enabled.Value);
            return true;
        }

        [MenuItem(ENABLED, priority = -4000)]
        private static void ENABLED_TOGGLE()
        {
            if (!_enabled.IsAwake)
            {
                return;
            }

            _enabled.Value = !_enabled.Value;
        }

        private static string[] _indents = new string[100];

        private static Dictionary<string, string> _typeLookup;
        private static Dictionary<string, Dictionary<int, int>> _stackDepthLookup;
#endif
    }
}
