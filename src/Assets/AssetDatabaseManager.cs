using System;
using System.Collections.Generic;
using System.Text;
using Appalachia.Core.Attributes;
using Appalachia.Core.Preferences;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    [InitializeOnLoad]
    [ExecutionOrder(9999)]
    public static partial class AssetDatabaseManager
    {
        public static PREF<int> _SAVE_FRAME_DELAY;
        public static PREF<bool> _SAVE_ON_ENABLE;

        [NonSerialized] public static int LastSaveAt;

        [NonSerialized] public static bool QueuedSoon;
        [NonSerialized] public static bool QueuedNextFrame;
        [NonSerialized] private static int _suspensionDepth;
        [NonSerialized] private static int _deferralDepth;

        private static bool _explicitlyStarted;

        private static readonly PREF<bool> log = PREFS.REG(G_,
            "Log Deferrals",
            false
        );

        private static Dictionary<string, Action> _postActions;

        private static StringBuilder _pathBuilder;

        public static bool ImportDeferred => _deferralDepth > 0;
        public static bool ImportSuspended => _suspensionDepth > 0;

        private const string G_ = "Appalachia/Asset Database";

        [ExecuteOnEnable]
        private static void OnEnable()
        {
            _SAVE_FRAME_DELAY = PREFS.REG(G_, "Save Frame Delay",                 30);
            _SAVE_ON_ENABLE = PREFS.REG(G_,   "Save On Enable (After Compiling)", true);

            if (_SAVE_ON_ENABLE.v)
            {
                AssetDatabase.SaveAssets();
            }

            LastSaveAt = 0;
            QueuedSoon = false;
            QueuedNextFrame = false;
            _suspensionDepth = 0;
            _deferralDepth = 0;
        }

        //private static Action execution;
        [ExecuteOnUpdate]
        private static void OnUpdate()
        {
            /*if (execution == null)
            {
                execution = StaticRoutine.CreateDelegate(typeof(AssetDatabaseManager), nameof(ExecuteSave));
            }

            execution();*/
            ExecuteSave();
        }

        [MenuItem("Assets/Start Asset Editing")]
        public static void StartAssetEditing()
        {
            AssetDatabase.StartAssetEditing();
            _explicitlyStarted = true;
            _suspensionDepth += 1;
        }

        [MenuItem("Assets/Stop Asset Editing")]
        public static void StopAssetEditing()
        {
            AssetDatabase.StopAssetEditing();
            _explicitlyStarted = false;
            _suspensionDepth -= 1;
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

        private static void Log(string prefix)
        {
            if (!log.v)
            {
                return;
            }

            Debug.LogWarning(
                $"{prefix}| Deferral depth: {_deferralDepth:000} | Suspension depth: {_suspensionDepth:000}"
            );
        }

        [MenuItem("Assets/Force Recompile C# Project")]
        public static void ForceRecompile()
        {
            CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.None);
        }
    }
}
