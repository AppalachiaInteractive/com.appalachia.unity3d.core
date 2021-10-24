using System;
using System.Collections.Generic;
using System.Text;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Preferences;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public class AssetDatabaseSaveManager : MonoBehaviour
    {
        private const string _PRF_PFX = nameof(AssetDatabaseSaveManager) + ".";

        private const string G_ = "Appalachia/Asset Database";

        [NonSerialized] public static bool QueuedNextFrame;
        [NonSerialized] public static bool QueuedSoon;
        [NonSerialized] public static int LastSaveAt;
        public static PREF<bool> _SAVE_ON_ENABLE;
        public static PREF<int> _SAVE_FRAME_DELAY;
        private static bool _explicitlyStarted;
        private static Dictionary<string, Action> _postActions;
        [NonSerialized] private static int _deferralDepth;
        [NonSerialized] private static int _suspensionDepth;
        private static PREF<bool> log;
        private static StringBuilder _pathBuilder;
        private static readonly ProfilerMarker _PRF_Log = new(_PRF_PFX + nameof(Log));

        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + nameof(Update));
        private static readonly ProfilerMarker _PRF_SaveAssetsNow = new(_PRF_PFX + nameof(SaveAssetsNow));

        private static readonly ProfilerMarker _PRF_StartAssetEditing =
            new(_PRF_PFX + nameof(StartAssetEditing));

        private static readonly ProfilerMarker _PRF_StopAssetEditing =
            new(_PRF_PFX + nameof(StopAssetEditing));

        public static bool ImportDeferred => _deferralDepth > 0;
        public static bool ImportSuspended => _suspensionDepth > 0;

        public void SaveAssetsNow()
        {
            using (_PRF_SaveAssetsNow.Auto())
            {
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

        private void OnEnable()
        {
            _SAVE_FRAME_DELAY = PREFS.REG(G_, "Save Frame Delay",                 30);
            _SAVE_ON_ENABLE = PREFS.REG(G_,   "Save On Enable (After Compiling)", true);
            log = PREFS.REG(G_,               "Log Deferrals",                    false);

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
                    Log("Flushing import pauses.  ");

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

        public static bool RequestSuspendImport(out IDisposable scope)
        {
            if (EditorApplication.isPlaying ||
                EditorApplication.isCompiling ||
                EditorApplication.isPaused ||
                EditorApplication.isUpdating ||
                Application.isBatchMode ||
                Application.isPlaying)
            {
                scope = null;
                return false;
            }

            scope = new DeferredAssetEditingScope();
            return true;
        }

        public static void SaveAssetsNextFrame(string key = null, Action post = null)
        {
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
            QueuedSoon = true;
        }

        /// <summary>
        ///     <para>
        ///         Starts importing Assets into the Asset Database. This lets you group several Asset imports together into one larger import.
        ///         Note:
        ///         Calling AssetDatabase.StartAssetEditing() places the Asset Database in a state that will prevent imports until
        ///         AssetDatabase.StopAssetEditing() is called.
        ///         This means that if an exception occurs between the two function calls, the AssetDatabase will be unresponsive.
        ///         Therefore, it is highly recommended that you place calls to AssetDatabase.StartAssetEditing() and AssetDatabase.StopAssetEditing() inside
        ///         either a try..catch block, or a try..finally block as needed.
        ///     </para>
        /// </summary>
        [MenuItem("Assets/Start Asset Editing")]
        public static void StartAssetEditing()
        {
            using (_PRF_StartAssetEditing.Auto())
            {
                AssetDatabaseManager.StartAssetEditing();
                _explicitlyStarted = true;
                _suspensionDepth += 1;
            }
        }

        [MenuItem("Assets/Stop Asset Editing")]
        public static void StopAssetEditing()
        {
            using (_PRF_StopAssetEditing.Auto())
            {
                AssetDatabase.StopAssetEditing();
                _explicitlyStarted = false;
                _suspensionDepth -= 1;
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

                Debug.LogWarning(
                    $"{prefix}| Deferral depth: {_deferralDepth:000} | Suspension depth: {_suspensionDepth:000}"
                );
            }
        }

        private class DeferredAssetEditingScope : IDisposable
        {
            public DeferredAssetEditingScope()
            {
                if (!ImportSuspended)
                {
                    AssetDatabase.StartAssetEditing();
                    _suspensionDepth += 1;
                    _deferralDepth += 1;
                    Log("Suspending import pause. ");
                }
                else
                {
                    _deferralDepth += 1;

                    //Log("Additional import pause scope. ");    
                }
            }

            void IDisposable.Dispose()
            {
                _deferralDepth -= 1;

                //Log("Disposing import pause scope. ");
            }
        }
    }
}
