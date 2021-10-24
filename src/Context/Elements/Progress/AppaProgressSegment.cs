using System;

namespace Appalachia.Core.Context.Elements.Progress
{
    public sealed class AppaProgressSegment : IDisposable
    {
        private bool _disposed;
        private AppaProgressCounter _counter;

        internal AppaProgressSegment(AppaProgressCounter counter)
        {
            _counter = counter;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _counter.EndSegment();
            }
        }
    }
}