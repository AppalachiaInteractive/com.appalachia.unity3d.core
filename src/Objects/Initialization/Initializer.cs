using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Initialization
{
    [Serializable]
    public class Initializer
    {
        public enum TagState
        {
            Serialized = 0,
            NonSerialized = 100
        }

        #region Constants and Static Readonly

        private const string GROUP = "Internal";

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private HashSet<string> _nonSerializedTagsHash;

        [NonSerialized] private HashSet<string> _tagsHash;

        private int delayFrames = 1;

        [NonSerialized] private List<string> _nonSerializedTags;

        [HideInInspector, SerializeField]
        private List<string> _tags;

        [NonSerialized] private object _object;

        [HideInInspector, SerializeField]
        private string _resetToken;

        #endregion

        internal string ResetToken => _resetToken;

        public void ConfigureInitializerDelay(int delayForXFrames)
        {
            using (_PRF_ConfigureInitializerDelay.Auto())
            {
                delayFrames = delayForXFrames;
            }
        }

        public async AppaTask Do(IUnitySerializable obj, string tag, TagState tagState, Action action)
        {
            using (_PRF_Do.Auto())
            {
                await InitializeInternal(obj, tag, false, tagState, action);
            }
        }

        public async AppaTask Do(IUnitySerializable obj, string tag, Action action)
        {
            using (_PRF_Do.Auto())
            {
                await InitializeInternal(obj, tag, false, TagState.Serialized, action);
            }
        }

        public async AppaTask Do(IUnitySerializable obj, string tag, bool force, Action action)
        {
            using (_PRF_Do.Auto())
            {
                await InitializeInternal(obj, tag, force, TagState.Serialized, action);
            }
        }

        public async AppaTask Do(
            IUnitySerializable obj,
            string tag,
            bool force,
            TagState tagState,
            Action action)
        {
            using (_PRF_Do.Auto())
            {
                await InitializeInternal(obj, tag, force, tagState, action);
            }
        }

        public async AppaTask<TC> Get<TC>(GameObject obj)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, typeof(TC).Name, false, TagState.Serialized, false);
            }
        }

        public async AppaTask<TC> Get<TC>(GameObject obj, string tag)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, tag, false, TagState.Serialized, false);
            }
        }

        public async AppaTask<TC> Get<TC>(GameObject obj, string tag, bool force)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, TagState.Serialized, false);
            }
        }

        public async AppaTask<TC> Get<TC>(GameObject obj, string tag, bool force, TagState tagState)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, tagState, false);
            }
        }

        public async AppaTask<TC> Get<TC>(MonoBehaviour obj)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, typeof(TC).Name, false, TagState.Serialized, false);
            }
        }

        public async AppaTask<TC> Get<TC>(MonoBehaviour obj, string tag)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, tag, false, TagState.Serialized, false);
            }
        }

        public async AppaTask<TC> Get<TC>(MonoBehaviour obj, string tag, bool force)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, TagState.Serialized, false);
            }
        }

        public async AppaTask<TC> Get<TC>(MonoBehaviour obj, string tag, bool force, TagState tagState)
            where TC : Component
        {
            using (_PRF_Get.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, tagState, false);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(GameObject obj)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, typeof(TC).Name, false, TagState.Serialized, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(GameObject obj, string tag)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, tag, false, TagState.Serialized, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(GameObject obj, string tag, bool force)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, TagState.Serialized, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(GameObject obj, string tag, bool force, TagState tagState)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, tagState, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(MonoBehaviour obj)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, typeof(TC).Name, false, TagState.Serialized, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(MonoBehaviour obj, string tag)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, tag, false, TagState.Serialized, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(MonoBehaviour obj, string tag, bool force)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, TagState.Serialized, true);
            }
        }

        public async AppaTask<TC> GetOrCreate<TC>(
            MonoBehaviour obj,
            string tag,
            bool force,
            TagState tagState)
            where TC : Component
        {
            using (_PRF_GetOrCreate.Auto())
            {
                return await GetInternal<TC>(obj, tag, force, tagState, true);
            }
        }

        public void Reset(object obj, string token)
        {
            using (_PRF_Reset.Auto())
            {
                _object = obj;

                if (ResetToken == token)
                {
                    return;
                }

                GetLoggingObjects(out var loggingName, out var loggingContext);

                AppaLog.Context.Initialize.Warn(
                    ZString.Format("Initializer was reset for [{0}].", loggingName),
                    loggingContext
                );
                _resetToken = token;

                ResetDirectly();
            }
        }

        public InitializationScope Scope(object obj, string tag, TagState tagState)
        {
            using (_PRF_Scope.Auto())
            {
                return InitializeScopeInternal(obj, tag, false, tagState);
            }
        }

        public InitializationScope Scope(object obj, string tag)
        {
            using (_PRF_Scope.Auto())
            {
                return InitializeScopeInternal(obj, tag, false, TagState.Serialized);
            }
        }

        public InitializationScope Scope(object obj, string tag, bool force)
        {
            using (_PRF_Scope.Auto())
            {
                return InitializeScopeInternal(obj, tag, force, TagState.Serialized);
            }
        }

        public InitializationScope Scope(object obj, string tag, bool force, TagState tagState)
        {
            using (_PRF_Scope.Auto())
            {
                return InitializeScopeInternal(obj, tag, force, tagState);
            }
        }

        public async AppaTask Yield()
        {
            await AppaTask.Yield();
        }

        internal bool HasInitialized(string tag, TagState tagState = TagState.Serialized)
        {
            using (_PRF_HasInitialized.Auto())
            {
                InitializeInternalCollections();

                if (tagState == TagState.Serialized)
                {
                    return _tagsHash.Contains(tag);
                }

                return _nonSerializedTagsHash.Contains(tag);
            }
        }

        private async AppaTask ExecuteDelay()
        {
            using (_PRF_ExecuteDelay.Auto())
            {
                var i = 0;

                while (i < delayFrames)
                {
                    await AppaTask.Yield(PlayerLoopTiming.Initialization);
                    i += 1;
                }
            }
        }

        private async AppaTask<TC> GetInternal<TC>(
            GameObject obj,
            string tag,
            bool force,
            TagState tagState,
            bool allowCreate)
            where TC : Component
        {
            using (_PRF_GetInternal.Auto())
            {
                TC foundComponent = null;

                Action getter = () =>
                {
                    foundComponent = obj.GetComponent<TC>();
                    if (allowCreate && (foundComponent == null))
                    {
                        foundComponent = obj.gameObject.AddComponent<TC>();
                    }
                };

                await InitializeInternal(obj, tag, force, tagState, getter);

                return foundComponent;
            }
        }

        private async AppaTask<TC> GetInternal<TC>(
            MonoBehaviour obj,
            string tag,
            bool force,
            TagState tagState,
            bool allowCreate)
            where TC : Component
        {
            using (_PRF_GetInternal.Auto())
            {
                TC foundComponent = null;

                Action getter = () =>
                {
                    foundComponent = obj.GetComponent<TC>();
                    if (allowCreate && (foundComponent == null))
                    {
                        foundComponent = obj.gameObject.AddComponent<TC>();
                    }
                };

                await InitializeInternal(obj, tag, force, tagState, getter);

                return foundComponent;
            }
        }

        private void GetLoggingObjects(out string loggingName, out Object loggingContext)
        {
            loggingName = _object is Object uobj1 ? uobj1.name : _object.GetType().Name;
            loggingContext = _object is Object uobj2 ? uobj2 : null;
        }

        private void HandleCaughtInitializationException(string tag, Exception ex)
        {
            using (_PRF_HandleCaughtInitializationException.Auto())
            {
                GetLoggingObjects(out var loggingName, out var loggingContext);
                AppaLog.Context.Initialize.Error(
                    ZString.Format(
                        "Failed to initialize for tag [{1}] of [{0}]",
                        loggingName.FormatNameForLogging(),
                        tag.FormatNameForLogging()
                    ),
                    loggingContext,
                    ex
                );
            }
        }

        private async AppaTask InitializeInternal(
            object obj,
            string tag,
            bool force,
            TagState tagState,
            Action action)
        {
            using (_PRF_InitializeInternal.Auto())
            {
                if (InternalHasPreviouslyInitialized(obj, tag, force, tagState))
                {
                    return;
                }

                try
                {
                    action();

                    MarkInitialized(tag, tagState);

                    await ExecuteDelay();
                }
                catch (Exception ex)
                {
                    HandleCaughtInitializationException(tag, ex);
                }

                MarkAsModified();
            }
        }

        private void InitializeInternalCollections()
        {
            using (_PRF_InitializeInternal.Auto())
            {
                _tags ??= new List<string>();
                _tagsHash ??= new HashSet<string>(_tags);
                _nonSerializedTags ??= new List<string>();
                _nonSerializedTagsHash ??= new HashSet<string>(_tags);

                if (_tagsHash.Count != _tags.Count)
                {
                    _tagsHash.Clear();
                    _tagsHash.AddRange2(_tags);
                }

                if (_nonSerializedTagsHash.Count != _nonSerializedTags.Count)
                {
                    _nonSerializedTagsHash.Clear();
                    _nonSerializedTagsHash.AddRange2(_nonSerializedTags);
                }
            }
        }

        private InitializationScope InitializeScopeInternal(
            object obj,
            string tag,
            bool force,
            TagState tagState)
        {
            using (_PRF_InitializeScopeInternal.Auto())
            {
                _object = obj;

                if (!force)
                {
                    if (HasInitialized(tag, tagState))
                    {
                        return new InitializationScope(this, tag, tagState, false);
                    }

                    GetLoggingObjects(out var loggingName, out var loggingContext);
                    AppaLog.Context.Initialize.Debug(
                        ZString.Format(
                            "Will initialize tag [{1}] of [{0}].",
                            loggingName.FormatNameForLogging(),
                            tag.FormatNameForLogging()
                        ),
                        loggingContext
                    );
                }
                else
                {
                    GetLoggingObjects(out var loggingName, out var loggingContext);
                    AppaLog.Context.Initialize.Debug(
                        ZString.Format(
                            "Initialization was forced for tag [{1}] of [{0}].",
                            loggingName.FormatNameForLogging(),
                            tag.FormatNameForLogging()
                        ),
                        loggingContext
                    );
                }

                return new InitializationScope(this, tag, tagState, true);
            }
        }

        private bool InternalHasPreviouslyInitialized(object obj, string tag, bool force, TagState tagState)
        {
            using (_PRF_InternalHasPreviouslyInitialized.Auto())
            {
                _object = obj;

                if (!force)
                {
                    if (HasInitialized(tag, tagState))
                    {
                        return true;
                    }

                    GetLoggingObjects(out var loggingName, out var loggingContext);
                    AppaLog.Context.Initialize.Debug(
                        ZString.Format(
                            "Will initialize tag [{1}] of [{0}].",
                            loggingName.FormatNameForLogging(),
                            tag.FormatNameForLogging()
                        ),
                        loggingContext
                    );
                }
                else
                {
                    GetLoggingObjects(out var loggingName, out var loggingContext);
                    AppaLog.Context.Initialize.Debug(
                        ZString.Format(
                            "Initialization was forced for tag [{1}] of [{0}].",
                            loggingName.FormatNameForLogging(),
                            tag.FormatNameForLogging()
                        ),
                        loggingContext
                    );
                }

                return false;
            }
        }

        private void MarkAsModified()
        {
#if UNITY_EDITOR
            if (_object is Object tobj)
            {
                tobj.MarkAsModified();
            }
            else if (_object is IUnitySerializable us)
            {
                us.MarkAsModified();
            }
#endif
        }

        private void MarkInitialized(string tag, TagState tagState = TagState.Serialized)
        {
            using (_PRF_MarkInitialized.Auto())
            {
                InitializeInternalCollections();

                if (tagState == TagState.Serialized)
                {
                    if (!_tagsHash.Contains(tag))
                    {
                        _tagsHash.Add(tag);
                        _tags.Add(tag);
                    }
                }
                else
                {
                    if (!_nonSerializedTagsHash.Contains(tag))
                    {
                        _nonSerializedTagsHash.Add(tag);
                        _nonSerializedTags.Add(tag);
                    }
                }
            }
        }

        private void ResetDirectly()
        {
            using (_PRF_ResetDirectly.Auto())
            {
                _tags = new List<string>();
                _tagsHash = new HashSet<string>();
                _nonSerializedTags = new List<string>();
                _nonSerializedTagsHash = new HashSet<string>();

                MarkAsModified();
            }
        }

        [Button, ButtonGroup(GROUP)]
        private void ResetInitializationData()
        {
            using (_PRF_Reset.Auto())
            {
                ResetDirectly();
            }
        }

        #region Nested type: InitializationScope

        public struct InitializationScope : IDisposable
        {
            public InitializationScope(
                Initializer initializer,
                string tag,
                TagState state,
                bool shouldInitialize)
            {
                _initializer = initializer;
                _tag = tag;
                _state = state;
                _shouldInitialize = shouldInitialize;
                _marked = false;
            }

            #region Fields and Autoproperties

            private readonly bool _shouldInitialize;
            private readonly Initializer _initializer;
            private readonly string _tag;
            private readonly TagState _state;

            private bool _marked;

            #endregion

            public bool ShouldInitialize => _shouldInitialize;

            public void MarkInitialized()
            {
                using (_PRF_MarkInitialized.Auto())
                {
                    _initializer.MarkInitialized(_tag, _state);
                    _marked = true;
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                using (_PRF_Dispose.Auto())
                {
                    _initializer.MarkAsModified();

                    if (_shouldInitialize && !_marked)
                    {
                        AppaLog.Context.Initialize.Warn(
                            ZString.Format(
                                "Did not mark [{0}] initialized.  Was this intentional?",
                                _tag.FormatNameForLogging()
                            )
                        );
                    }
                }
            }

            #endregion

            #region Profiling

            private static readonly ProfilerMarker _PRF_Dispose =
                new ProfilerMarker(_PRF_PFX + nameof(Dispose));

            #endregion
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(Initializer) + ".";

        private static readonly ProfilerMarker _PRF_ExecuteDelay =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteDelay));

        private static readonly ProfilerMarker _PRF_ConfigureInitializerDelay =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureInitializerDelay));

        private static readonly ProfilerMarker _PRF_GetInternal =
            new ProfilerMarker(_PRF_PFX + nameof(GetInternal));

        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));

        private static readonly ProfilerMarker _PRF_GetOrCreate =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrCreate));

        private static readonly ProfilerMarker _PRF_InitializeScopeInternal =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeScopeInternal));

        private static readonly ProfilerMarker _PRF_Scope = new ProfilerMarker(_PRF_PFX + nameof(Scope));

        private static readonly ProfilerMarker _PRF_HandleCaughtInitializationException =
            new ProfilerMarker(_PRF_PFX + nameof(HandleCaughtInitializationException));

        private static readonly ProfilerMarker _PRF_InternalHasPreviouslyInitialized =
            new ProfilerMarker(_PRF_PFX + nameof(InternalHasPreviouslyInitialized));

        private static readonly ProfilerMarker _PRF_ResetDirectly =
            new ProfilerMarker(_PRF_PFX + nameof(ResetDirectly));

        private static readonly ProfilerMarker _PRF_InitializeInternal =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeInternalCollections));

        private static readonly ProfilerMarker _PRF_Reset =
            new ProfilerMarker(_PRF_PFX + nameof(ResetInitializationData));

        private static readonly ProfilerMarker _PRF_Do = new ProfilerMarker(_PRF_PFX + nameof(Do));

        private static readonly ProfilerMarker _PRF_HasInitialized =
            new ProfilerMarker(_PRF_PFX + nameof(HasInitialized));

        private static readonly ProfilerMarker _PRF_MarkInitialized =
            new ProfilerMarker(_PRF_PFX + nameof(MarkInitialized));

        #endregion
    }
}
