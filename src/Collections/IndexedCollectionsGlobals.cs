#region

using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Constants;

#endregion

namespace Appalachia.Core.Collections
{
#if UNITY_EDITOR
    public static class IndexedCollectionsGlobals
    {
        #region Constants and Static Readonly

        private const string GROUP = PKG.Menu.Appalachia.Tools.Base + "Indexed Collections/";
        private const string MENU_RECREATE = GROUP + "Can Recreate Collections";
        private const string MENU_SHOW = GROUP + "Show All Fields";

        #endregion

        #region Preferences

        [NonSerialized] private static PREF<bool> _RECREATE;

        [NonSerialized] private static PREF<bool> _SHOW;

        private static PREF<bool> RECREATE
        {
            get
            {
                if (_RECREATE == null)
                {
                    _RECREATE = PREFS.REG(PKG.Prefs.Group, "Allow Collection Recreation", false);
                }

                return _RECREATE;
            }
        }

        private static PREF<bool> SHOW
        {
            get
            {
                if (_SHOW == null)
                {
                    _SHOW = PREFS.REG(PKG.Prefs.Group, "Show Debugging Fields", false);
                }

                return _SHOW;
            }
        }

        #endregion

        public static bool CanRecreateBrokenCollections
        {
            get
            {
                var pref = APPASERIALIZE.InSerializationWindow ? _RECREATE : RECREATE;

                if (pref != null)
                {
                    if (pref.IsAwake)
                    {
                        return pref.Value;
                    }

                    if (!APPASERIALIZE.InSerializationWindow)
                    {
                        pref.WakeUp();
                        return pref.Value;
                    }
                }

                return false;
            }
        }

        public static bool ShouldShowDebuggingFields
        {
            get
            {
                var pref = APPASERIALIZE.InSerializationWindow ? _SHOW : SHOW;

                if (pref != null)
                {
                    if (pref.IsAwake)
                    {
                        return pref.Value;
                    }

                    if (!APPASERIALIZE.InSerializationWindow)
                    {
                        pref.WakeUp();
                        return pref.Value;
                    }
                }

                return false;
            }
        }

        [ExecuteOnEnable]
        private static void Initialize()
        {
            SHOW.WakeUp();
            RECREATE.WakeUp();
        }

        #region Menu Items

        [UnityEditor.MenuItem(MENU_RECREATE, priority = PKG.Menu.Appalachia.Tools.Priority)]
        private static void MENU_UI_CAN_RECREATE()
        {
            if (!RECREATE.IsAwake)
            {
                return;
            }

            RECREATE.Value = !RECREATE.Value;
        }

        [UnityEditor.MenuItem(MENU_RECREATE, true, PKG.Menu.Appalachia.Tools.Priority)]
        private static bool MENU_UI_CAN_RECREATE_VALIDATE()
        {
            if (!RECREATE.IsAwake)
            {
                return false;
            }

            UnityEditor.Menu.SetChecked(MENU_RECREATE, RECREATE.Value);
            return true;
        }

        [UnityEditor.MenuItem(MENU_SHOW, priority = PKG.Menu.Appalachia.Tools.Priority)]
        private static void MENU_UI_DEBUG()
        {
            if (!SHOW.IsAwake)
            {
                return;
            }

            SHOW.Value = !SHOW.Value;
        }

        [UnityEditor.MenuItem(MENU_SHOW, true, PKG.Menu.Appalachia.Tools.Priority)]
        private static bool MENU_UI_DEBUG_VALIDATE()
        {
            if (!SHOW.IsAwake)
            {
                return false;
            }

            UnityEditor.Menu.SetChecked(MENU_SHOW, SHOW.Value);
            return true;
        }

        #endregion
    }

#endif
}
