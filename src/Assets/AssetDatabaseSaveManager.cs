#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public class AssetDatabaseSaveManager : MonoBehaviour
    {
        #region Preferences

        public static PREF<int> _SAVE_FRAME_DELAY;
        public static PREF<bool> _SAVE_ON_ENABLE;
        private static PREF<bool> log;

        #endregion

        #region Static Fields and Autoproperties

        [NonSerialized] public static bool QueuedNextFrame;
        [NonSerialized] public static bool QueuedSoon;
        [NonSerialized] public static int LastSaveAt;
        [NonSerialized] private static AppaContext _context;
        private static bool _explicitlyStarted;
        private static Dictionary<string, Action> _postActions;
        [NonSerialized] private static int _deferralDepth;
        [NonSerialized] private static int _suspensionDepth;
        private static StringBuilder _pathBuilder;

        #endregion

        public static bool ImportDeferred => _deferralDepth > 0;
        public static bool ImportSuspended => _suspensionDepth > 0;

        private static AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(typeof(AssetDatabaseSaveManager));
                }

                return _context;
            }
        }

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (_explicitlyStarted)
                {
                    return;
                }

                if (!ImportDeferred && ImportSuspended)
                {
                    Log("Removing import suspension.");

                    while (ImportSuspended)
                    {
                        StopAssetEditing();
                        _suspensionDepth -= 1;

                        //Log($"Flushing import pause. Iteration: {iteration} ");
                    }

                    return;
                }

                if (!QueuedSoon && !QueuedNextFrame)
                {
                    return;
                }

                var waitThreshold = QueuedNextFrame ? 1 : _SAVE_FRAME_DELAY.v;

                var frameTime = Time.frameCount;
                var timeSinceLastSave = frameTime - LastSaveAt;

                if (timeSinceLastSave < waitThreshold)
                {
                    return;
                }

                SaveAssetsNow();
            }
        }

        private void OnEnable()
        {
            if (AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            _SAVE_FRAME_DELAY = PREFS.REG(PKG.Prefs.Group, "Save Frame Delay",                 30);
            _SAVE_ON_ENABLE = PREFS.REG(PKG.Prefs.Group,   "Save On Enable (After Compiling)", true);
            log = PREFS.REG(PKG.Prefs.Group,               "Log Deferrals",                    false);

            if (_SAVE_ON_ENABLE.v)
            {
                AssetDatabaseManager.SaveAssets();
            }

            LastSaveAt = 0;
            QueuedSoon = false;
            QueuedNextFrame = false;
            _suspensionDepth = 0;
            _deferralDepth = 0;
        }

        #endregion

        public static bool RequestSuspendImport(out IDisposable scope)
        {
            if (AppalachiaApplication.IsPlayingOrWillPlay ||
                AppalachiaApplication.IsCompiling ||
                AppalachiaApplication.IsPaused ||
                AppalachiaApplication.IsUpdating ||
                AppalachiaApplication.IsBatchMode)
            {
                scope = null;
                return false;
            }

            scope = new DeferredAssetEditingScope();
            return true;
        }

        public static void SaveAssetsNextFrame(string key = null, Action post = null)
        {
            if (AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            if ((key != null) && (post != null))
            {
                if (_postActions == null)
                {
                    _postActions = new Dictionary<string, Action>();
                }

                if (!_postActions.ContainsKey(key))
                {
                    _postActions.Add(key, post);
                }
            }

            QueuedNextFrame = true;
        }

        public static void SaveAssetsSoon()
        {
            if (AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            QueuedSoon = true;
        }

        public void SaveAssetsNow()
        {
            using (_PRF_SaveAssetsNow.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                LastSaveAt = Time.frameCount;

                if (_postActions != null)
                {
                    foreach (var postAction in _postActions)
                    {
                        var action = postAction.Value;

                        action();
                    }

                    _postActions.Clear();
                }

                AssetDatabaseManager.SaveAssets();

                AssetDatabaseManager.Refresh();

                QueuedSoon = false;
                QueuedNextFrame = false;
            }
        }

        private static void Log(string prefix)
        {
            using (_PRF_Log.Auto())
            {
                if (!log.v)
                {
                    return;
                }

                Context.Log.Warn(
                    ZString.Format(
                        "{0} | Deferral depth: {1:000} | Suspension depth: {2:000}",
                        prefix,
                        _deferralDepth,
                        _suspensionDepth
                    )
                );
            }
        }

        #region Nested type: DeferredAssetEditingScope

        private class DeferredAssetEditingScope : IDisposable
        {
            public DeferredAssetEditingScope()
            {
                if (!ImportSuspended)
                {
                    AssetDatabaseManager.StartAssetEditing();
                    _suspensionDepth += 1;
                    _deferralDepth += 1;
                    Log("Suspending import.");
                }
                else
                {
                    _deferralDepth += 1;

                    //Log("Additional import pause scope. ");    
                }
            }

            #region IDisposable Members

            void IDisposable.Dispose()
            {
                _deferralDepth -= 1;

                //Log("Disposing import pause scope. ");
            }

            #endregion
        }

        #endregion

        #region Menu Items

        /// <summary>
        ///     <para>
        ///         Starts importing Assets into the Asset Database. This lets you group several Asset imports together into one larger import.
        ///         Note:
        ///         Calling AssetDatabaseManager.StartAssetEditing() places the Asset Database in a state that will prevent imports until
        ///         AssetDatabaseManager.StopAssetEditing() is called.
        ///         This means that if an exception occurs between the two function calls, the AssetDatabase will be unresponsive.
        ///         Therefore, it is highly recommended that you place calls to AssetDatabaseManager.StartAssetEditing() and
        ///         AssetDatabaseManager.StopAssetEditing() inside
        ///         either a try..catch block, or a try..finally block as needed.
        ///     </para>
        /// </summary>
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + "Start Asset Editing",
            priority = PKG.Menu.Assets.Priority
        )]
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.RootTools.Base + "Assets/Start Asset Editing",
            priority = PKG.Menu.Assets.Priority
        )]
        public static void StartAssetEditing()
        {
            using (_PRF_StartAssetEditing.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                AssetDatabaseManager.StartAssetEditing();
                _explicitlyStarted = true;
                _suspensionDepth += 1;
            }
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + "Stop Asset Editing",
            priority = PKG.Menu.Assets.Priority
        )]
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.RootTools.Base + "Assets/Stop Asset Editing",
            priority = PKG.Menu.Assets.Priority
        )]
        public static void StopAssetEditing()
        {
            using (_PRF_StopAssetEditing.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                AssetDatabaseManager.StopAssetEditing();
                _explicitlyStarted = false;
                _suspensionDepth -= 1;
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AssetDatabaseSaveManager) + ".";
        private static readonly ProfilerMarker _PRF_Log = new(_PRF_PFX + nameof(Log));
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + nameof(Update));
        private static readonly ProfilerMarker _PRF_SaveAssetsNow = new(_PRF_PFX + nameof(SaveAssetsNow));

        private static readonly ProfilerMarker _PRF_StartAssetEditing =
            new(_PRF_PFX + nameof(StartAssetEditing));

        private static readonly ProfilerMarker _PRF_StopAssetEditing =
            new(_PRF_PFX + nameof(StopAssetEditing));

        #endregion
    }
}

#endif
