using System;
using System.Collections;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.Utility.Execution.Progress;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaUIContext
    {
        #region Fields and Autoproperties

        [NonSerialized] private AppaContext _context;

        private AppaProgress _initializationProgress;
#pragma warning disable 649
        private bool _forceLock;
#pragma warning restore 649
        private bool _initialized;
        private bool _initializing;

        #endregion

        protected virtual bool ShouldBeLocked => false;

        public bool IsLocked
        {
            get
            {
                if (ShouldBeLocked || _forceLock)
                {
                    return true;
                }

                return false;
            }
        }

        /*
        public void ForceLock()
        {
            _forceLock = !_forceLock;
        }
        */

        public bool ShouldInitialize => !Initialized && !Initializing;

        public AppaProgress InitializationProgress
        {
            get => _initializationProgress;
            protected set => _initializationProgress = value;
        }

        public bool Initialized
        {
            get => _initialized;
            protected set => _initialized = value;
        }

        public bool Initializing
        {
            get => _initializing;
            protected set => _initializing = value;
        }

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }

        public IEnumerator Check()
        {
            if (ShouldInitialize)
            {
                return Initialize();
            }

            return default;
        }

        protected abstract IEnumerable<AppaProgress> OnInitialize(AppaProgressCounter p);

        // ReSharper disable once UnusedParameter.Global
        protected virtual IEnumerable<AppaProgress> OnPostInitialize(AppaProgressCounter p)
        {
            yield break;
        }

        protected virtual IEnumerable<AppaProgress> OnPreInitialize(AppaProgressCounter p)
        {
            yield break;
        }

        private IEnumerator Initialize()
        {
            if (Initialized || Initializing)
            {
                yield break;
            }

            Initialized = false;
            Initializing = true;

            var progressCounter = new AppaProgressCounter();

            foreach (var progress in OnPreInitialize(progressCounter))
            {
                var initializationProgress = progress;

                progressCounter.Interpret(ref initializationProgress);

                InitializationProgress = initializationProgress;

                yield return null;
            }

            foreach (var progress in OnInitialize(progressCounter))
            {
                var initializationProgress = progress;

                progressCounter.Interpret(ref initializationProgress);

                InitializationProgress = initializationProgress;

                yield return null;
            }

            foreach (var progress in OnPostInitialize(progressCounter))
            {
                var initializationProgress = progress;

                progressCounter.Interpret(ref initializationProgress);

                InitializationProgress = initializationProgress;

                yield return null;
            }

            Initialized = true;
            Initializing = false;
        }
    }
}
