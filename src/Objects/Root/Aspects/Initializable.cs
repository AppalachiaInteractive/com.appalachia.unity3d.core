using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable UnusedParameter.Global
// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public delegate void InitializationCompleteHandler(IInitializable target);

    public partial class AppalachiaObject : IInitializable
    {
        #region Fields and Autoproperties

        [SmartTitleGroup(
            BASE,
            "$" + nameof(GetTitle),
            "$" + nameof(GetSubtitle),
            "$" + nameof(GetFallbackTitle),
            "$" + nameof(GetFallbackSubtitle),
            true,
            Brand.Title.IsBold,
            false,
            titleColor: "$" + nameof(GetTitleColor),
            titleFont: Brand.Font.ObjectFont,
            subtitleColor: "$" + nameof(GetTitleColor),
            subtitleFont: Brand.Font.ObjectFont,
            titleSize: Brand.Title.Size,
            subtitleSize: Brand.Subtitle.Size,
            titleIcon: null,
            subtitleIcon: null,
            backgroundColor: "$" + nameof(GetBackgroundColor),
            titleHeight: Brand.Title.Height
        )]
        [SmartFoldoutGroup(
            GROUP,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [FormerlySerializedAs("_initializationData")]
        [SerializeField]
        private Initializer _initializer;

        [NonSerialized] private InitializationState _initializationState;

        #endregion

        private InitializationState initializationState
        {
            get
            {
                using (_PRF_initializationState.Auto())
                {
                    if (_initializationState == null)
                    {
                        _initializer ??= new Initializer();

                        _initializationState =
                            new InitializationState(async () => await Initialize(_initializer));
                        MarkAsModified();
                    }

                    return _initializationState;
                }
            }
        }

        /// <summary>
        ///     Runs immediately after <see cref="Initialize" /> to allow any necessary cleanup or subsequent initialization tasks.
        /// </summary>
        protected virtual void AfterInitialization()
        {
        }

        /// <summary>
        ///     Runs immediately prior to <see cref="Initialize" /> to allow any necessary synchronous preparation for the impending initialization task.
        /// </summary>
        protected virtual void BeforeInitialization()
        {
        }

        /// <summary>
        ///     Offers an opportunity to initialize any required components.
        /// </summary>
        /// <param name="initializer">An initializer that helps maintain initialization state.</param>
        /// <returns>An awaitable task.</returns>
        protected virtual async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                initializationState.InitializeSynchronous(InitializationComplete, this);
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        private AppaTask StartInitializing()
        {
            using (_PRF_StartInitializing.Auto())
            {
                var startTask = initializationState.Initialize(InitializationComplete, this);

                return startTask;
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        InitializationState IInitializable.initializationState => initializationState;

        async AppaTask IInitializable.StartInitializing()
        {
            await StartInitializing();
        }

        void IInitializable.InitializationExceptionHandler(Exception ex)
        {
            InitializationExceptionHandler(ex);
        }

        async AppaTask IInitializable.Initialize(Initializer initializer)
        {
            await Initialize(initializer);
        }

        void IInitializable.InitializeSynchronous()
        {
            InitializeSynchronous();
        }

        void IInitializable.BeforeInitialization()
        {
            BeforeInitialization();
        }

        void IInitializable.AfterInitialization()
        {
            AfterInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_StartInitializing =
            new ProfilerMarker(_PRF_PFX + nameof(StartInitializing));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public partial class AppalachiaRepository
    {
    }

    public abstract partial class AppalachiaObject
    {
    }

    public abstract partial class SingletonAppalachiaObject<T>
    {
    }

    public abstract partial class AppalachiaBehaviour<T> : IInitializable
    {
        #region Fields and Autoproperties

        [SmartTitleGroup(
            BASE,
            "$" + nameof(GetTitle),
            "$" + nameof(GetSubtitle),
            "$" + nameof(GetFallbackTitle),
            "$" + nameof(GetFallbackSubtitle),
            true,
            Brand.Title.IsBold,
            false,
            titleColor: "$" + nameof(GetTitleColor),
            titleFont: Brand.Font.ObjectFont,
            subtitleColor: "$" + nameof(GetTitleColor),
            subtitleFont: Brand.Font.ObjectFont,
            titleSize: Brand.Title.Size,
            subtitleSize: Brand.Subtitle.Size,
            titleIcon: null,
            subtitleIcon: null,
            backgroundColor: "$" + nameof(GetBackgroundColor),
            titleHeight: Brand.Title.Height
        )]
        [SmartFoldoutGroup(
            GROUP,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [FormerlySerializedAs("_initializationData")]
        [SerializeField]
        private Initializer _initializer;

        [NonSerialized] private InitializationState _initializationState;

        #endregion

        private InitializationState initializationState
        {
            get
            {
                using (_PRF_initializationState.Auto())
                {
                    if (_initializationState == null)
                    {
                        _initializer ??= new Initializer();

                        _initializationState =
                            new InitializationState(async () => await Initialize(_initializer));
                        MarkAsModified();
                    }

                    return _initializationState;
                }
            }
        }

        /// <summary>
        ///     Runs immediately after <see cref="Initialize(Appalachia.Core.Objects.Initialization.Initializer)" /> to allow any necessary cleanup or subsequent
        ///     initialization tasks.
        /// </summary>
        protected virtual void AfterInitialization()
        {
        }

        /// <summary>
        ///     Runs immediately prior to <see cref="Initialize(Appalachia.Core.Objects.Initialization.Initializer)" /> to allow any necessary synchronous
        ///     preparation for the impending initialization task.
        /// </summary>
        protected virtual void BeforeInitialization()
        {
        }

        /// <summary>
        ///     Offers an opportunity to initialize any required components.
        /// </summary>
        /// <param name="initializer">An initializer that helps maintain initialization state.</param>
        /// <returns>An awaitable task.</returns>
        protected virtual async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                initializationState.InitializeSynchronous(InitializationComplete, this as T);
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        private AppaTask StartInitializing()
        {
            using (_PRF_StartInitializing.Auto())
            {
                var startTask = initializationState.Initialize(InitializationComplete, this as T);

                return startTask;
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        InitializationState IInitializable.initializationState => initializationState;

        async AppaTask IInitializable.StartInitializing()
        {
            await StartInitializing();
        }

        void IInitializable.InitializationExceptionHandler(Exception ex)
        {
            InitializationExceptionHandler(ex);
        }

        async AppaTask IInitializable.Initialize(Initializer initializer)
        {
            await Initialize(initializer);
        }

        void IInitializable.InitializeSynchronous()
        {
            InitializeSynchronous();
        }

        void IInitializable.BeforeInitialization()
        {
            BeforeInitialization();
        }

        void IInitializable.AfterInitialization()
        {
            AfterInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_StartInitializing =
            new ProfilerMarker(_PRF_PFX + nameof(StartInitializing));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public abstract partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaBase : IInitializable
    {
        #region Fields and Autoproperties

        [SmartTitleGroup(
            BASE,
            "$" + nameof(GetTitle),
            "$" + nameof(GetSubtitle),
            "$" + nameof(GetFallbackTitle),
            "$" + nameof(GetFallbackSubtitle),
            true,
            Brand.Title.IsBold,
            false,
            titleColor: "$" + nameof(GetTitleColor),
            titleFont: Brand.Font.ObjectFont,
            subtitleColor: "$" + nameof(GetTitleColor),
            subtitleFont: Brand.Font.ObjectFont,
            titleSize: Brand.Title.Size,
            subtitleSize: Brand.Subtitle.Size,
            titleIcon: null,
            subtitleIcon: null,
            backgroundColor: "$" + nameof(GetBackgroundColor),
            titleHeight: Brand.Title.Height
        )]
        [SmartFoldoutGroup(
            GROUP,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [FormerlySerializedAs("_initializationData")]
        [SerializeField]
        private Initializer _initializer;

        [NonSerialized] private InitializationState _initializationState;

        #endregion

        private InitializationState initializationState
        {
            get
            {
                using (_PRF_initializationState.Auto())
                {
                    if (_initializationState == null)
                    {
                        _initializer ??= new Initializer();

                        _initializationState =
                            new InitializationState(async () => await Initialize(_initializer));

                        MarkAsModified();
                    }

                    return _initializationState;
                }
            }
        }

        /// <summary>
        ///     Runs immediately after <see cref="Initialize" /> to allow any necessary cleanup or subsequent initialization tasks.
        /// </summary>
        protected virtual void AfterInitialization()
        {
        }

        /// <summary>
        ///     Runs immediately prior to <see cref="Initialize" /> to allow any necessary synchronous preparation for the impending initialization task.
        /// </summary>
        protected virtual void BeforeInitialization()
        {
        }

        /// <summary>
        ///     Offers an opportunity to initialize any required components.
        /// </summary>
        /// <param name="initializer">An initializer that helps maintain initialization state.</param>
        /// <returns>An awaitable task.</returns>
        protected virtual async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                initializationState.InitializeSynchronous(InitializationComplete, this);
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        private AppaTask StartInitializing()
        {
            using (_PRF_StartInitializing.Auto())
            {
                var startTask = initializationState.Initialize(InitializationComplete, this);

                return startTask;
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        InitializationState IInitializable.initializationState => initializationState;

        async AppaTask IInitializable.StartInitializing()
        {
            await StartInitializing();
        }

        void IInitializable.InitializationExceptionHandler(Exception ex)
        {
            InitializationExceptionHandler(ex);
        }

        async AppaTask IInitializable.Initialize(Initializer initializer)
        {
            await Initialize(initializer);
        }

        void IInitializable.InitializeSynchronous()
        {
            InitializeSynchronous();
        }

        void IInitializable.BeforeInitialization()
        {
            BeforeInitialization();
        }

        void IInitializable.AfterInitialization()
        {
            AfterInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_StartInitializing =
            new ProfilerMarker(_PRF_PFX + nameof(StartInitializing));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaPlayable : IInitializable
    {
        #region Fields and Autoproperties

        [SmartTitleGroup(
            BASE,
            "$" + nameof(GetTitle),
            "$" + nameof(GetSubtitle),
            "$" + nameof(GetFallbackTitle),
            "$" + nameof(GetFallbackSubtitle),
            true,
            Brand.Title.IsBold,
            false,
            titleColor: "$" + nameof(GetTitleColor),
            titleFont: Brand.Font.ObjectFont,
            subtitleColor: "$" + nameof(GetTitleColor),
            subtitleFont: Brand.Font.ObjectFont,
            titleSize: Brand.Title.Size,
            subtitleSize: Brand.Subtitle.Size,
            titleIcon: null,
            subtitleIcon: null,
            backgroundColor: "$" + nameof(GetBackgroundColor),
            titleHeight: Brand.Title.Height
        )]
        [SmartFoldoutGroup(
            GROUP,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [FormerlySerializedAs("_initializationData")]
        [SerializeField]
        private Initializer _initializer;

        [NonSerialized] private InitializationState _initializationState;

        #endregion

        private InitializationState initializationState
        {
            get
            {
                using (_PRF_initializationState.Auto())
                {
                    if (_initializationState == null)
                    {
                        _initializer ??= new Initializer();

                        _initializationState =
                            new InitializationState(async () => await Initialize(_initializer));

                        /*MarkAsModified();*/
                    }

                    return _initializationState;
                }
            }
        }

        /// <summary>
        ///     Offers an opportunity to initialize any required components.
        /// </summary>
        /// <param name="initializer">An initializer that helps maintain initialization state.</param>
        /// <returns>An awaitable task.</returns>
        protected abstract AppaTask Initialize(Initializer initializer);

        /// <summary>
        ///     Runs immediately after <see cref="Initialize" /> to allow any necessary cleanup or subsequent initialization tasks.
        /// </summary>
        protected virtual void AfterInitialization()
        {
        }

        /// <summary>
        ///     Runs immediately prior to <see cref="Initialize" /> to allow any necessary synchronous preparation for the impending initialization task.
        /// </summary>
        protected virtual void BeforeInitialization()
        {
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                initializationState.InitializeSynchronous(InitializationComplete, this);
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        private AppaTask StartInitializing()
        {
            using (_PRF_StartInitializing.Auto())
            {
                var startTask = initializationState.Initialize(InitializationComplete, this);

                return startTask;
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        InitializationState IInitializable.initializationState => initializationState;

        async AppaTask IInitializable.StartInitializing()
        {
            await StartInitializing();
        }

        void IInitializable.InitializationExceptionHandler(Exception ex)
        {
            InitializationExceptionHandler(ex);
        }

        async AppaTask IInitializable.Initialize(Initializer initializer)
        {
            await Initialize(initializer);
        }

        void IInitializable.InitializeSynchronous()
        {
            InitializeSynchronous();
        }

        void IInitializable.BeforeInitialization()
        {
            BeforeInitialization();
        }

        void IInitializable.AfterInitialization()
        {
            AfterInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_StartInitializing =
            new ProfilerMarker(_PRF_PFX + nameof(StartInitializing));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
