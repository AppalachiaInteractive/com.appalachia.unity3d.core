using System;
using System.Collections.Generic;
using Appalachia.Core.Collections.Native;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Core.Execution
{
    public sealed partial class CleanupManager : SingletonAppalachiaBehaviour<CleanupManager>, IDisposable
    {
        #region Static Fields and Autoproperties

        private static List<Action> _actions;

        private static List<object> _disposables;

        #endregion

        public static void Store(Action action)
        {
            using (_PRF_Store.Auto())
            {
                if (action == default)
                {
                    return;
                }

                _actions ??= new List<Action>();

                _actions.Add(action);
            }
        }

        public static void Store<T>(ref T disposable)
        where T : IDisposable
        {
            using (_PRF_Store.Auto())
            {
                if (Equals(disposable, default(T)))
                {
                    return;
                }

                _disposables ??= new List<object>();

                _disposables.Add(disposable);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();

            using (_PRF_WhenDestroyed.Auto())
            {
                Dispose();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                Dispose();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            using (_PRF_Dispose.Auto())
            {
                if (_disposables != null)
                {
                    for (var i = _disposables.Count - 1; i >= 0; i--)
                    {
                        var disposable = _disposables[i];

                        if ((disposable != null) && disposable is IDisposable d)
                        {
                            d.SafeDispose();
                        }

                        _disposables.RemoveAt(i);
                    }
                }

                if (_actions != null)
                {
                    for (var i = _actions.Count - 1; i >= 0; i--)
                    {
                        var action = _actions[i];

                        if (action != null)
                        {
                            try
                            {
                                action();
                            }
                            catch (Exception ex)
                            {
                                Context.Log.Error("Failed to execute cleanup action.", this, ex);
                            }
                        }

                        _actions.RemoveAt(i);
                    }
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        private static readonly ProfilerMarker _PRF_Store = new ProfilerMarker(_PRF_PFX + nameof(Store));

        #endregion
    }
}
