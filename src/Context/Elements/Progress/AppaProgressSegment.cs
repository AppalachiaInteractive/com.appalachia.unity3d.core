using System;

namespace Appalachia.Core.Context.Elements.Progress
{
    public sealed class AppaProgressSegment : IDisposable
    {
        internal AppaProgressSegment(AppaProgressCounter counter)
        {
            _counter = counter;
        }

        private AppaProgressCounter _counter;
        private bool _disposed;

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
