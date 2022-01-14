#region

using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Preferences;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
    [CallStaticConstructorInEditor]
    public static class FrameEventSettings
    {
        static FrameEventSettings()
        {
        }

#if UNITY_EDITOR

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_SUB = PREFS.REG(PKG.Prefs.Group, "Subscribe", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_UNSUB = PREFS.REG(PKG.Prefs.Group, "Unsubscribe", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_AWAKE = PREFS.REG(PKG.Prefs.Group, "Awake", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_START = PREFS.REG(PKG.Prefs.Group, "Start", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_ENABLE = PREFS.REG(PKG.Prefs.Group, "Enable", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_DISABLE = PREFS.REG(PKG.Prefs.Group, "Disable", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_QUIT = PREFS.REG(PKG.Prefs.Group, "Quit", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_DESTROY = PREFS.REG(PKG.Prefs.Group, "Destroy", false);

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "LogSubscribe", true)]
        private static bool ENABLE_SUBv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Subscribe", _ENABLE_SUB.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Subscribe", priority = PKG.Priority)]
        public static void ENABLE_SUB()
        {
            _ENABLE_SUB.v = !_ENABLE_SUB.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Unsubscribe", true)]
        private static bool ENABLE_UNSUBv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Unsubscribe", _ENABLE_UNSUB.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Unsubscribe", priority = PKG.Priority)]
        public static void ENABLE_UNSUB()
        {
            _ENABLE_UNSUB.v = !_ENABLE_UNSUB.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Awake", true)]
        private static bool ENABLE_AWAKEv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Awake", _ENABLE_AWAKE.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Awake", priority = PKG.Priority)]
        public static void ENABLE_AWAKE()
        {
            _ENABLE_AWAKE.v = !_ENABLE_AWAKE.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Start", true)]
        private static bool ENABLE_STARTv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Start", _ENABLE_START.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Start", priority = PKG.Priority)]
        public static void ENABLE_START()
        {
            _ENABLE_START.v = !_ENABLE_START.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Enable", true)]
        private static bool ENABLE_ENABLEv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Enable", _ENABLE_ENABLE.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Enable", priority = PKG.Priority)]
        public static void ENABLE_ENABLE()
        {
            _ENABLE_ENABLE.v = !_ENABLE_ENABLE.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Disable", true)]
        private static bool ENABLE_DISABLEv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Disable", _ENABLE_DISABLE.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Disable", priority = PKG.Priority)]
        public static void ENABLE_DISABLE()
        {
            _ENABLE_DISABLE.v = !_ENABLE_DISABLE.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Quit", true)]
        private static bool ENABLE_QUITv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Quit", _ENABLE_QUIT.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Quit", priority = PKG.Priority)]
        public static void ENABLE_QUIT()
        {
            _ENABLE_QUIT.v = !_ENABLE_QUIT.v;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Destroy", true)]
        private static bool ENABLE_DESTROYv()
        {
            UnityEditor.Menu.SetChecked(PKG.Menu.Appalachia.Tools.Base + "Log Destroy", _ENABLE_DESTROY.v);
            return true;
        }

        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Log Destroy", priority = PKG.Priority)]
        public static void ENABLE_DESTROY()
        {
            _ENABLE_DESTROY.v = !_ENABLE_DESTROY.v;
        }
#endif
    }
}
