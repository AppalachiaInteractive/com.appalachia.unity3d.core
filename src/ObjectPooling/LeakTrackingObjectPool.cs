#region

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

        private readonly ConditionalWeakTable<T, Tracker> _trackers = new();
        private readonly ObjectPool<T> _inner;

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
                    Debug.Fail($"{typeof(T).Name} was leaked. Created at: {Environment.NewLine}{_stack}");
                }
            }

            private readonly string _stack;
            private bool _disposed;

            public void Dispose()
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
