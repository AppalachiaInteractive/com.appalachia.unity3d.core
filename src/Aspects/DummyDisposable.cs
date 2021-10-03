#region

using System;

#endregion

namespace Appalachia.Core.Aspects
{
    public class DummyDisposable : IDisposable
    {
        public void Dispose()
        {
        }

        public IDisposable Start()
        {
            return this;
        }
    }
}
