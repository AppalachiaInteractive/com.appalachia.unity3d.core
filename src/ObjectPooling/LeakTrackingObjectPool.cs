#region

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Appalachia.Utility.Strings;

#endregion

namespace Appalachia.Core.ObjectPooling
{
    public class LeakTrackingObjectPool<T>
        where T : class, new()
    {
        public LeakTrackingObjectPool(ObjectPool<T> inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            _inner = inner;
        }

        #region Fields and Autoproperties

        private readonly ConditionalWeakTable<T, Tracker> _trackers = new();
        private readonly ObjectPool<T> _inner;

        #endregion

        public T Get()
        {
            var value = _inner.Get();
            _trackers.Add(value, new Tracker());
            return value;
        }

        public void Return(T obj)
        {
            Tracker tracker;
            if (_trackers.TryGetValue(obj, out tracker))
            {
                _trackers.Remove(obj);
                tracker.Dispose();
            }

            _inner.Return(obj);
        }

        #region Nested type: Tracker

        private class Tracker : IDisposable
        {
            public Tracker()
            {
                _stack = Environment.StackTrace;
            }

            ~Tracker()
            {
                if (!_disposed && !Environment.HasShutdownStarted)
                {
                    Debug.Fail(
                        ZString.Format(
                            "{0} was leaked. Created at: {1}{2}",
                            typeof(T).Name,
                            Environment.NewLine,
                            _stack
                        )
                    );
                }
            }

            #region Fields and Autoproperties

            private readonly string _stack;
            private bool _disposed;

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        #endregion
    }
}
