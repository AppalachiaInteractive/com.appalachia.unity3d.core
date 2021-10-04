#region

using System;
using Appalachia.Editing.Preferences;
using UnityEditor;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class FrameEventSettings
    {
        static FrameEventSettings()
        {
        }

#if UNITY_EDITOR

        private const string G_ = "Static Native Disposal";
        private const string MENU_BASE_ = "Tools/Frame Start And End/" + G_ + "/";

        private const string _ENABLE_SUB_ = MENU_BASE_ + "Log Subscribe";
        private const string _ENABLE_UNSUB_ = MENU_BASE_ + "Log Unsubscribe";
        private const string _ENABLE_AWAKE_ = MENU_BASE_ + "Log Awake";
        private const string _ENABLE_START_ = MENU_BASE_ + "Log Start";
        private const string _ENABLE_ENABLE_ = MENU_BASE_ + "Log Enable";
        private const string _ENABLE_DISABLE_ = MENU_BASE_ + "Log Disable";
        private const string _ENABLE_QUIT_ = MENU_BASE_ + "Log Quit";
        private const string _ENABLE_DESTROY_ = MENU_BASE_ + "Log Destroy";

        [NonSerialized] public static readonly PREF<bool> _ENABLE_SUB = PREFS.REG(G_, "Subscribe", false);

        [NonSerialized]
        public static readonly PREF<bool> _ENABLE_UNSUB = PREFS.REG(G_, "Unsubscribe", false);

        [NonSerialized] public static readonly PREF<bool> _ENABLE_AWAKE = PREFS.REG(G_,   "Awake",   false);
        [NonSerialized] public static readonly PREF<bool> _ENABLE_START = PREFS.REG(G_,   "Start",   false);
        [NonSerialized] public static readonly PREF<bool> _ENABLE_ENABLE = PREFS.REG(G_,  "Enable",  false);
        [NonSerialized] public static readonly PREF<bool> _ENABLE_DISABLE = PREFS.REG(G_, "Disable", false);
        [NonSerialized] public static readonly PREF<bool> _ENABLE_QUIT = PREFS.REG(G_,    "Quit",    false);
        [NonSerialized] public static readonly PREF<bool> _ENABLE_DESTROY = PREFS.REG(G_, "Destroy", false);

        [MenuItem(_ENABLE_SUB_, true)]
        private static bool ENABLE_SUBv()
        {
            Menu.SetChecked(_ENABLE_SUB_, _ENABLE_SUB.v);
            return true;
        }

        [MenuItem(_ENABLE_SUB_, false)]
        public static void ENABLE_SUB()
        {
            _ENABLE_SUB.v = !_ENABLE_SUB.v;
        }

        private static bool ENABLE_UNSUBv()
        {
            Menu.SetChecked(_ENABLE_UNSUB_, _ENABLE_UNSUB.v);
            return true;
        }

        [MenuItem(_ENABLE_UNSUB_, false)]
        public static void ENABLE_UNSUB()
        {
            _ENABLE_UNSUB.v = !_ENABLE_UNSUB.v;
        }

        private static bool ENABLE_AWAKEv()
        {
            Menu.SetChecked(_ENABLE_AWAKE_, _ENABLE_AWAKE.v);
            return true;
        }

        [MenuItem(_ENABLE_AWAKE_, false)]
        public static void ENABLE_AWAKE()
        {
            _ENABLE_AWAKE.v = !_ENABLE_AWAKE.v;
        }

        private static bool ENABLE_STARTv()
        {
            Menu.SetChecked(_ENABLE_START_, _ENABLE_START.v);
            return true;
        }

        [MenuItem(_ENABLE_START_, false)]
        public static void ENABLE_START()
        {
            _ENABLE_START.v = !_ENABLE_START.v;
        }

        private static bool ENABLE_ENABLEv()
        {
            Menu.SetChecked(_ENABLE_ENABLE_, _ENABLE_ENABLE.v);
            return true;
        }

        [MenuItem(_ENABLE_ENABLE_, false)]
        public static void ENABLE_ENABLE()
        {
            _ENABLE_ENABLE.v = !_ENABLE_ENABLE.v;
        }

        private static bool ENABLE_DISABLEv()
        {
            Menu.SetChecked(_ENABLE_DISABLE_, _ENABLE_DISABLE.v);
            return true;
        }

        [MenuItem(_ENABLE_DISABLE_, false)]
        public static void ENABLE_DISABLE()
        {
            _ENABLE_DISABLE.v = !_ENABLE_DISABLE.v;
        }

        private static bool ENABLE_QUITv()
        {
            Menu.SetChecked(_ENABLE_QUIT_, _ENABLE_QUIT.v);
            return true;
        }

        [MenuItem(_ENABLE_QUIT_, false)]
        public static void ENABLE_QUIT()
        {
            _ENABLE_QUIT.v = !_ENABLE_QUIT.v;
        }

        private static bool ENABLE_DESTROYv()
        {
            Menu.SetChecked(_ENABLE_DESTROY_, _ENABLE_DESTROY.v);
            return true;
        }

        [MenuItem(_ENABLE_DESTROY_, false)]
        public static void ENABLE_DESTROY()
        {
            _ENABLE_DESTROY.v = !_ENABLE_DESTROY.v;
        }
#endif
    }
}
