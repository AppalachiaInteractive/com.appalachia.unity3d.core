#region

using System;

#endregion

namespace Appalachia.Core.Aspects
{
    public class DummyDisposable : IDisposable
    {
        private static DummyDisposable _instance;
        private static Func<IDisposable> _provider;

        public void Dispose()
        {
        }

        public IDisposable Start()
        {
            return this;
        }

        public static DummyDisposable instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DummyDisposable();
                }

                return _instance;
            }
        }

        public static Func<IDisposable> provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = () => instance;
                }

                return _provider;
            }
        }
    }
}