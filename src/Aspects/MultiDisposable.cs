/*
#region

using System;
using Appalachia.Core.Pooling;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Base.Aspects
{
    public sealed partial class AspectSets
    {

#pragma warning disable 612
    public class MultiDisposable : SelfPoolingObject<MultiDisposable>, IDisposable
#pragma warning restore 612
    {
        private int _count;

        private bool _reverseOrder;
        private IDisposable _0;
        private IDisposable _1;
        private IDisposable _2;
        private IDisposable _3;
        private IDisposable _4;
        private IDisposable _5;
        private IDisposable _6;
        private IDisposable _7;
        private IDisposable _8;
        private IDisposable _9;

        public void Dispose()
        {
            if (_reverseOrder)
            {
                DisposeReverse();
                return;
            }

            if (_count == 0)
            {
                OnDisposal();
                return;
            }

            for (var i = 0; i < _count; i++)
            {
                if (i == 0) _0?.Dispose();
                if (i == 1) _1?.Dispose();
                if (i == 2) _2?.Dispose();
                if (i == 3) _3?.Dispose();
                if (i == 4) _4?.Dispose();
                if (i == 5) _5?.Dispose();
                if (i == 6) _6?.Dispose();
                if (i == 7) _7?.Dispose();
                if (i == 8) _8?.Dispose();
                if (i == 9) _9?.Dispose();
            }

            OnDisposal();
        }

        public void DisposeReverse()
        {
            if (_count == 0)
            {
                OnDisposal();
                return;
            }

            for (var index = 0; index < _count; index++)
            {
                var i = _count - index - 1;

                if (i == 0) _0?.Dispose();
                if (i == 1) _1?.Dispose();
                if (i == 2) _2?.Dispose();
                if (i == 3) _3?.Dispose();
                if (i == 4) _4?.Dispose();
                if (i == 5) _5?.Dispose();
                if (i == 6) _6?.Dispose();
                if (i == 7) _7?.Dispose();
                if (i == 8) _8?.Dispose();
                if (i == 9) _9?.Dispose();
            }

            OnDisposal();
        }

        public void Set(
            IDisposable _0,IDisposable _1, bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                _count = 2;
                _reverseOrder = reverseOrder;
            }
        }


        public void Set(
            IDisposable _0,IDisposable _1, IDisposable _2, bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                _count = 3;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,IDisposable _1, IDisposable _2, IDisposable _3, bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                _count = 4;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,IDisposable _1, IDisposable _2, IDisposable _3, IDisposable _4, bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                this._4 = _4;
                _count = 5;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,
            IDisposable _1,
            IDisposable _2,
            IDisposable _3,
            IDisposable _4,
            IDisposable _5,
            bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                this._4 = _4;
                this._5 = _5;
                _count = 6;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,
            IDisposable _1,
            IDisposable _2,
            IDisposable _3,
            IDisposable _4,
            IDisposable _5,
            IDisposable _6,
            bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                this._4 = _4;
                this._5 = _5;
                this._6 = _6;
                _count = 7;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,
            IDisposable _1,
            IDisposable _2,
            IDisposable _3,
            IDisposable _4,
            IDisposable _5,
            IDisposable _6,
            IDisposable _7,
            bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                this._4 = _4;
                this._5 = _5;
                this._6 = _6;
                this._7 = _7;
                _count = 8;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,
            IDisposable _1,
            IDisposable _2,
            IDisposable _3,
            IDisposable _4,
            IDisposable _5,
            IDisposable _6,
            IDisposable _7,
            IDisposable _8,
            bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                this._4 = _4;
                this._5 = _5;
                this._6 = _6;
                this._7 = _7;
                this._8 = _8;
                _count = 9;
                _reverseOrder = reverseOrder;
            }
        }

        public void Set(
            IDisposable _0,
            IDisposable _1,
            IDisposable _2,
            IDisposable _3,
            IDisposable _4,
            IDisposable _5,
            IDisposable _6,
            IDisposable _7,
            IDisposable _8,
            IDisposable _9,
            bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                this._0 = _0;
                this._1 = _1;
                this._2 = _2;
                this._3 = _3;
                this._4 = _4;
                this._5 = _5;
                this._6 = _6;
                this._7 = _7;
                this._8 = _8;
                this._9 = _9;
                _count = 10;
                _reverseOrder = reverseOrder;
            }
        }


        private static readonly ProfilerMarker _PRF_MultiDisposable_Set = new ProfilerMarker("MultiDisposable.Set");
        public void Set(IDisposable[] args, bool reverseOrder = false)
        {
            using (_PRF_MultiDisposable_Set.Auto())
            {
                var length = args.Length;

                if (length > 10)
                {
                    throw new NotSupportedException("Too many.  Implement chaining.");
                }

                if (length > 0) _0 = args[0];
                if (length > 1) _1 = args[1];
                if (length > 2) _2 = args[2];
                if (length > 3) _3 = args[3];
                if (length > 4) _4 = args[4];
                if (length > 5) _5 = args[5];
                if (length > 6) _6 = args[6];
                if (length > 7) _7 = args[7];
                if (length > 8) _8 = args[8];
                if (length > 9) _9 = args[9];

                _count = args.Length;
                _reverseOrder = reverseOrder;
            }
        }


        private static readonly ProfilerMarker _PRF_MultiDisposable_Reset = new ProfilerMarker("MultiDisposable.Reset");
        public override void Reset()
        {
            _0 = null;
            _1 = null;
            _2 = null;
            _3 = null;
            _4 = null;
            _5 = null;
            _6 = null;
            _7 = null;
            _8 = null;
            _9 = null;
            _count = 0;
            _reverseOrder = false;
        }

        public override void Initialize()
        {
        }

        protected virtual void OnDisposal()
        {
            AspectSets._currentDepth -= 1;

            Return();
        }
    }
    }
}
*/


