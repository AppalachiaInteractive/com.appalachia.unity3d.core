using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
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
        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        #region Fields and Autoproperties

        [ShowIfGroup(GROUP_ROOT)]
        [SmartFoldoutGroup(
            GROUP_INTERNAL,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [SerializeField]
        [ShowIf(nameof(ShowMetadata))]
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
                        if (_initializer == null)
                        {
                            _initializer = new Initializer();
                        }

                        _initializationState = new InitializationState(
                            async () => { await Initialize(_initializer); }
                        );
                    }

                    return _initializationState;
                }
            }
        }

        protected internal async AppaTask ExecuteInitialization()
        {
            if (initializationState.hasInitializationFinished)
            {
                return;
            }

            BeforeInitialization();

            InitializePreferences();
            
            await initializationState.Initialize(InitializationComplete, this);

            AfterInitialization();

            initializationState.hasInitializationFinished = true;
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
        /// <param name="initializer">
        ///     Will help maintain initialization state by keeping track of which <see cref="Initializer" />.
        ///     <see cref="Initializer.Do(IUnitySerializable, string, Action)" /> calls have already been executed.
        /// </param>
        /// <returns>An awaitable task.</returns>
        protected virtual async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        protected Initializer GetInitializer()
        {
            using (_PRF_GetInitializer.Auto())
            {
                if (_initializer == null)
                {
                    _initializer = new Initializer();
                }

                return _initializer;
            }
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                if (initializationState.hasInitializationFinished)
                {
                    return;
                }

                BeforeInitialization();

                InitializePreferences();

                initializationState.InitializeSynchronous(InitializationComplete, this);

                AfterInitialization();

                initializationState.hasInitializationFinished = true;
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        InitializationState IInitializable.initializationState => initializationState;

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

        async AppaTask IInitializable.ExecuteInitialization()
        {
            await ExecuteInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetInitializer =
            new ProfilerMarker(_PRF_PFX + nameof(GetInitializer));

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public partial class AppalachiaRepository
    {
    }

    public abstract partial class SingletonAppalachiaObject<T>
    {
    }

    public abstract partial class AppalachiaBehaviour : IInitializable
    {
        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        #region Fields and Autoproperties

        [ShowIfGroup(GROUP_ROOT)]
        [SmartFoldoutGroup(
            GROUP_INTERNAL,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty]
        [SerializeField]
        [ShowIf(nameof(ShowMetadata))]
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
                        if (_initializer == null)
                        {
                            _initializer = new Initializer();
                            this.MarkAsModified();
                        }

                        _initializationState =
                            new InitializationState(async () => { await Initialize(_initializer); });
                        this.MarkAsModified();
                    }

                    return _initializationState;
                }
            }
        }

        public Initializer GetInitializer()
        {
            using (_PRF_GetInitializer.Auto())
            {
                if (_initializer == null)
                {
                    _initializer = new Initializer();
                }

                return _initializer;
            }
        }

        protected internal async AppaTask ExecuteInitialization()
        {
            if (initializationState.hasInitializationStarted)
            {
                return;
            }

            BeforeInitialization();

            InitializePreferences();

            await initializationState.Initialize(InitializationComplete, this);

            AfterInitialization();

            initializationState.hasInitializationFinished = true;
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
                if (initializationState.hasInitializationFinished)
                {
                    return;
                }

                BeforeInitialization();

                InitializePreferences();

                initializationState.InitializeSynchronous(InitializationComplete, this);

                AfterInitialization();

                initializationState.hasInitializationFinished = true;
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        #region IInitializable Members

        public virtual bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        InitializationState IInitializable.initializationState => initializationState;

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

        async AppaTask IInitializable.ExecuteInitialization()
        {
            await ExecuteInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetInitializer =
            new ProfilerMarker(_PRF_PFX + nameof(GetInitializer));

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public abstract partial class AppalachiaBehaviour<T>
    {
    }

    public abstract partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaBase : IInitializable
    {
        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        #region Fields and Autoproperties

        [ShowIfGroup(GROUP_ROOT)]
        [SmartFoldoutGroup(
            GROUP_INTERNAL,
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
        [ShowIf(nameof(ShowMetadata))]
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
                        if (_initializer == null)
                        {
                            _initializer = new Initializer();
                        }

                        _initializationState =
                            new InitializationState(async () => await Initialize(_initializer));

                        MarkAsModified();
                    }

                    return _initializationState;
                }
            }
        }

        protected internal async AppaTask ExecuteInitialization()
        {
            if (initializationState.hasInitializationFinished)
            {
                return;
            }

            BeforeInitialization();

            InitializePreferences();

            await initializationState.Initialize(InitializationComplete, this);

            AfterInitialization();

            initializationState.hasInitializationFinished = true;
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

        protected Initializer GetInitializer()
        {
            using (_PRF_GetInitializer.Auto())
            {
                if (_initializer == null)
                {
                    _initializer = new Initializer();
                    MarkAsModified();
                }

                return _initializer;
            }
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                if (initializationState.hasInitializationFinished)
                {
                    return;
                }

                BeforeInitialization();

                InitializePreferences();

                initializationState.InitializeSynchronous(InitializationComplete, this);

                AfterInitialization();

                initializationState.hasInitializationFinished = true;
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), null, ex);
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        InitializationState IInitializable.initializationState => initializationState;

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

        async AppaTask IInitializable.ExecuteInitialization()
        {
            await ExecuteInitialization();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetInitializer =
            new ProfilerMarker(_PRF_PFX + nameof(GetInitializer));

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable : IInitializable
    {
        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        #region Fields and Autoproperties

        [ShowIfGroup(GROUP_ROOT)]
        [SmartFoldoutGroup(
            GROUP_INTERNAL,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [SerializeField]
        [ShowIf(nameof(ShowMetadata))]
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
                        if (_initializer == null)
                        {
                            _initializer = new Initializer();
                        }

                        _initializationState =
                            new InitializationState(async () => await Initialize(_initializer));

                        /*MarkAsModified();*/
                    }

                    return _initializationState;
                }
            }
        }

        protected internal async AppaTask ExecuteInitialization()
        {
            if (initializationState.hasInitializationFinished)
            {
                return;
            }

            BeforeInitialization();

            InitializePreferences();

            await initializationState.Initialize(InitializationComplete, this);

            AfterInitialization();

            initializationState.hasInitializationFinished = true;
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

        protected Initializer GetInitializer()
        {
            using (_PRF_GetInitializer.Auto())
            {
                if (_initializer == null)
                {
                    _initializer = new Initializer();
                }

                return _initializer;
            }
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                if (initializationState.hasInitializationFinished)
                {
                    return;
                }

                BeforeInitialization();

                InitializePreferences();

                initializationState.InitializeSynchronous(InitializationComplete, this);

                AfterInitialization();

                initializationState.hasInitializationFinished = true;
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), null, ex);
            }
        }

        #region IInitializable Members

        public bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        InitializationState IInitializable.initializationState => initializationState;

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

        async AppaTask IInitializable.ExecuteInitialization()
        {
            await ExecuteInitialization(null);
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetInitializer =
            new ProfilerMarker(_PRF_PFX + nameof(GetInitializer));

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        #endregion
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T> : IInitializable
    {
        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        public event InitializationCompleteHandler InitializationComplete;

        #region Fields and Autoproperties

        [ShowIfGroup(GROUP_ROOT)]
        [SmartFoldoutGroup(
            GROUP_INTERNAL,
            false,
            Brand.Groups.BackgroundColor,
            true,
            Brand.Groups.LabelColor,
            true,
            Brand.Groups.ChildColor
        )]
        [HideLabel, InlineProperty, LabelWidth(0)]
        [SerializeField]
        [ShowIf(nameof(ShowMetadata))]
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
                        if (_initializer == null)
                        {
                            _initializer = new Initializer();
                            this.MarkAsModified();
                        }

                        _initializationState =
                            new InitializationState(async () => { await Initialize(_initializer); });
                        MarkAsModified();
                    }

                    return _initializationState;
                }
            }
        }

        protected internal async AppaTask ExecuteInitialization()
        {
            if (initializationState.hasInitializationStarted)
            {
                return;
            }

            BeforeInitialization();

            await initializationState.Initialize(InitializationComplete, this);

            AfterInitialization();

            initializationState.hasInitializationFinished = true;
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

        protected Initializer GetInitializer()
        {
            using (_PRF_GetInitializer.Auto())
            {
                if (_initializer == null)
                {
                    _initializer = new Initializer();
                }

                return _initializer;
            }
        }

        /// <summary>
        ///     Immediately and synchronously initializes the class.
        /// </summary>
        protected void InitializeSynchronous()
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                if (initializationState.hasInitializationFinished)
                {
                    return;
                }

                BeforeInitialization();

                initializationState.InitializeSynchronous(InitializationComplete, this);

                AfterInitialization();

                initializationState.hasInitializationFinished = true;
            }
        }

        private void InitializationExceptionHandler(Exception ex)
        {
            using (_PRF_InitializationExceptionHandler.Auto())
            {
                Context.Log.Error(ZString.Format("Initialization error: {0}", ex.Message), this, ex);
            }
        }

        #region IInitializable Members

        public virtual bool FullyInitialized => initializationState.hasInitializationFinished;

        public bool HasInitializationStarted => initializationState.hasInitializationStarted;

        public bool HasInvokedInitializationCompleted =>
            initializationState.hasInvokedInitializationCompleted;

        InitializationState IInitializable.initializationState => initializationState;

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

        async AppaTask IInitializable.ExecuteInitialization()
        {
            await ExecuteInitialization();
        }

        #endregion

        #region Profiling

        private static readonly string _PRF_PFX2 = typeof(T) + ".";

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX2 + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_GetInitializer =
            new ProfilerMarker(_PRF_PFX2 + nameof(GetInitializer));

        private static readonly ProfilerMarker _PRF_InitializationExceptionHandler =
            new ProfilerMarker(_PRF_PFX2 + nameof(InitializationExceptionHandler));

        private static readonly ProfilerMarker _PRF_initializationState =
            new ProfilerMarker(_PRF_PFX2 + nameof(initializationState));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX2 + nameof(InitializeSynchronous));

        #endregion
    }
}
