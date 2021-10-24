using System.Collections;
using System.Collections.Generic;
using Appalachia.Core.Context.Elements.Progress;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaContext
    {
        private AppaProgress _initializationProgress;
        private bool _initialized;
        private bool _initializing;

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

        protected abstract IEnumerable<AppaProgress> OnInitialize(AppaProgressCounter p);

        protected virtual IEnumerable<AppaProgress> OnPostInitialize(AppaProgressCounter p)
        {
            yield break;
        }

        protected virtual IEnumerable<AppaProgress> OnPreInitialize(AppaProgressCounter p)
        {
            yield break;
        }

        public IEnumerator Check()
        {
            if (ShouldInitialize)
            {
                return Initialize();
            }

            return default;
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
