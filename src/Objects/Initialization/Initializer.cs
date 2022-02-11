using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root.Contracts;
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

        #region Fields and Autoproperties

        [NonSerialized] private HashSet<string> _nonSerializedTagsHash;

        [NonSerialized] private HashSet<string> _tagsHash;

        [NonSerialized] private List<string> _nonSerializedTags;

        [HideInInspector, SerializeField]
        private List<string> _tags;

        [NonSerialized] private object _object;

        [HideInInspector, SerializeField]
        private string _resetToken;

        #endregion

        internal string ResetToken => _resetToken;

        public void Do(IUnitySerializable obj, string tag, TagState tagState, Action action)
        {
            InitializeInternal(obj, tag, false, tagState, action);
        }

        public void Do(IUnitySerializable obj, string tag, Action action)
        {
            InitializeInternal(obj, tag, false, TagState.Serialized, action);
        }

        public void Do(IUnitySerializable obj, string tag, bool force, Action action)
        {
            InitializeInternal(obj, tag, force, TagState.Serialized, action);
        }

        public void Do(IUnitySerializable obj, string tag, bool force, TagState tagState, Action action)
        {
            InitializeInternal(obj, tag, force, tagState, action);
        }

        public TC Get<TC>(
            GameObject obj,
            TC target,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                typeof(TC).Name,
                target == null,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            GameObject obj,
            TC target,
            string tag,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                tag,
                target == null,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            GameObject obj,
            TC target,
            string tag,
            bool force,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, TagState.Serialized, false, getComponentStrategy);
        }

        public TC Get<TC>(
            GameObject obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, tagState, false, getComponentStrategy);
        }

        public TC Get<TC>(Transform obj, TC target, GetComponentStrategy getComponentStrategy)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                typeof(TC).Name,
                target == null,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            Transform obj,
            TC target,
            string tag,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                tag,
                target == null,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            Transform obj,
            TC target,
            bool force,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                typeof(TC).Name,
                force,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            Transform obj,
            TC target,
            string tag,
            bool force,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, TagState.Serialized, false, getComponentStrategy);
        }

        public TC Get<TC>(
            Transform obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, tagState, false, getComponentStrategy);
        }

        public TC Get<TC>(
            MonoBehaviour obj,
            TC target,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                typeof(TC).Name,
                target == null,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                tag,
                target == null,
                TagState.Serialized,
                false,
                getComponentStrategy
            );
        }

        public TC Get<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            bool force,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, TagState.Serialized, false, getComponentStrategy);
        }

        public TC Get<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, tagState, false, getComponentStrategy);
        }

        public TC GetOrCreate<TC>(
            GameObject obj,
            TC target,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                typeof(TC).Name,
                target == null,
                TagState.Serialized,
                true,
                getComponentStrategy
            );
        }

        public TC GetOrCreate<TC>(
            GameObject obj,
            TC target,
            string tag,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                tag,
                target == null,
                TagState.Serialized,
                true,
                getComponentStrategy
            );
        }

        public TC GetOrCreate<TC>(
            GameObject obj,
            TC target,
            string tag,
            bool force,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, TagState.Serialized, true, getComponentStrategy);
        }

        public TC GetOrCreate<TC>(
            GameObject obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, tagState, true, getComponentStrategy);
        }

        public TC GetOrCreate<TC>(
            MonoBehaviour obj,
            TC target,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                typeof(TC).Name,
                target == null,
                TagState.Serialized,
                true,
                getComponentStrategy
            );
        }

        public TC GetOrCreate<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(
                obj,
                target,
                tag,
                target == null,
                TagState.Serialized,
                true,
                getComponentStrategy
            );
        }

        public TC GetOrCreate<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            bool force,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, TagState.Serialized, true, getComponentStrategy);
        }

        public TC GetOrCreate<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            GetComponentStrategy getComponentStrategy = GetComponentStrategy.CurrentObject)
            where TC : Component
        {
            return GetInternal(obj, target, tag, force, tagState, true, getComponentStrategy);
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

        private TC GetInternal<TC>(
            GameObject obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            bool allowCreate,
            GetComponentStrategy getComponentStrategy)
            where TC : Component
        {
            var foundComponent = target;

            void Getter()
            {
                using (_PRF_GetInternal_Getter.Auto())
                {
                    foundComponent = obj.Get<TC>(getComponentStrategy);
                    if (allowCreate && (foundComponent == null))
                    {
                        foundComponent = obj.gameObject.AddComponent<TC>();
                    }
                }
            }

            InitializeInternal(obj, tag, force, tagState, Getter);

            return foundComponent;
        }

        private TC GetInternal<TC>(
            Transform obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            bool allowCreate,
            GetComponentStrategy getComponentStrategy)
            where TC : Component
        {
            return GetInternal(
                obj.gameObject,
                target,
                tag,
                force,
                tagState,
                allowCreate,
                getComponentStrategy
            );
        }

        private TC GetInternal<TC>(
            MonoBehaviour obj,
            TC target,
            string tag,
            bool force,
            TagState tagState,
            bool allowCreate,
            GetComponentStrategy getComponentStrategy)
            where TC : Component
        {
            return GetInternal(
                obj.gameObject,
                target,
                tag,
                force,
                tagState,
                allowCreate,
                getComponentStrategy
            );
        }

        private void GetLoggingObjects(out string loggingName, out Object loggingContext)
        {
            using (_PRF_GetLoggingObjects.Auto())
            {
                loggingName = _object is Object uobj1 ? uobj1.name : _object.GetType().Name;
                loggingContext = _object is Object uobj2 ? uobj2 : null;
            }
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

        private void InitializeInternal(object obj, string tag, bool force, TagState tagState, Action action)
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
            using (_PRF_InitializeInternalCollections.Auto())
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
            using (_PRF_MarkAsModified.Auto())
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

        [Button]
        private void ResetInitializationData()
        {
            using (_PRF_ResetInitializationData.Auto())
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

        private static readonly ProfilerMarker _PRF_GetInternal_Getter =
            new ProfilerMarker(_PRF_PFX + nameof(GetInternal) + ".Getter");

        private static readonly ProfilerMarker _PRF_GetLoggingObjects =
            new ProfilerMarker(_PRF_PFX + nameof(GetLoggingObjects));

        private static readonly ProfilerMarker _PRF_InitializeInternalCollections =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeInternalCollections));

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX + nameof(MarkAsModified));

        private static readonly ProfilerMarker _PRF_ResetInitializationData =
            new ProfilerMarker(_PRF_PFX + nameof(ResetInitializationData));

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
            new ProfilerMarker(_PRF_PFX + nameof(InitializeInternal));

        private static readonly ProfilerMarker _PRF_Reset =
            new ProfilerMarker(_PRF_PFX + nameof(ResetInitializationData));

        private static readonly ProfilerMarker _PRF_HasInitialized =
            new ProfilerMarker(_PRF_PFX + nameof(HasInitialized));

        private static readonly ProfilerMarker _PRF_MarkInitialized =
            new ProfilerMarker(_PRF_PFX + nameof(MarkInitialized));

        #endregion
    }
}
