using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public static partial class AssetDatabaseManager
    {
        private static void ExecuteSave()
        {
            if (_explicitlyStarted)
            {
                return;
            }

            if (!ImportDeferred && ImportSuspended)
            {
                Log("Flushing import pauses.  ");

                var iteration = 0;
                while (ImportSuspended)
                {
                    AssetDatabase.StopAssetEditing();
                    _suspensionDepth -= 1;

                    iteration += 1;

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

        public static void SaveAssetsSoon()
        {
            QueuedSoon = true;
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

        public static void SaveAssetsNow()
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

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();

            QueuedSoon = false;
            QueuedNextFrame = false;
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
